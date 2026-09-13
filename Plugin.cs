// You Can Heal - mod para PEAK, por dr. alfredo
//
// Qué hace: la herida (Injury) baja sola muy despacio con el tiempo.
// Cómo lo hace: PEAK tiene una clase CharacterAfflictions que cada frame
// llama a UpdateNormalStatuses() para bajar veneno, calor, sueño, etc.
// Nosotros nos "colgamos" de esa función con Harmony y, justo después de
// que corra, restamos un poquito de Injury.
//
// Para cambiar valores sin recompilar: BepInEx/config/dralfredo.YouCanHeal.cfg

using BepInEx;                 // BaseUnityPlugin, [BepInPlugin]
using BepInEx.Configuration;   // ConfigEntry: valores editables desde el .cfg
using HarmonyLib;              // Harmony: parchea funciones del juego en runtime
using UnityEngine;             // Time.deltaTime, Time.time, Mathf

namespace YouCanHeal
{
    // El GUID "dralfredo.YouCanHeal" es el identificador único del mod.
    // Con él se nombra el archivo de config y se registra el parche de Harmony.
    [BepInPlugin("dralfredo.YouCanHeal", "You Can Heal (dr. alfredo)", "1.0.2")]
    public class Plugin : BaseUnityPlugin
    {
        // ----- Opciones de configuración (se leen del .cfg) -----

        // Cuánto % de la barra de Injury se cura por cada tick. Default: 1.
        internal static ConfigEntry<float> PercentPerTick;

        // Cada cuántos segundos ocurre un tick. Default: 60.
        // 1% cada 60 s => una barra entera (100%) tarda 100 minutos.
        internal static ConfigEntry<float> TickSeconds;

        // Tras recibir daño nuevo, cuántos segundos esperar antes de volver
        // a curar. Evita que la curación "pelee" con una caída en curso.
        internal static ConfigEntry<float> CooldownAfterHit;

        // true = no cura mientras estás desmayado en el suelo.
        internal static ConfigEntry<bool> OnlyWhileConscious;

        // Awake() lo llama BepInEx una sola vez cuando carga el mod.
        private void Awake()
        {
            // Config.Bind(sección, clave, valor por defecto, descripción).
            // Si el .cfg no existe, BepInEx lo crea con estos defaults.
            PercentPerTick = Config.Bind("General", "PercentPerTick", 1f,
                "How much Injury (in %) is healed per tick.");
            TickSeconds = Config.Bind("General", "TickSeconds", 60f,
                "Seconds between healing ticks.");
            CooldownAfterHit = Config.Bind("General", "CooldownAfterHit", 10f,
                "Seconds without healing after taking a new injury.");
            OnlyWhileConscious = Config.Bind("General", "OnlyWhileConscious", true,
                "If true, no healing while passed out.");

            // PatchAll() busca en este ensamblado todas las clases con
            // [HarmonyPatch] y aplica sus parches. Solo hay uno, abajo.
            new Harmony("dralfredo.YouCanHeal").PatchAll();

            // Esto sale en BepInEx/LogOutput.log; sirve para confirmar que cargó.
            Logger.LogInfo($"You Can Heal loaded: {PercentPerTick.Value}% every {TickSeconds.Value}s");
        }
    }

    // Parche sobre CharacterAfflictions.UpdateNormalStatuses (método privado
    // del juego; Harmony lo encuentra por nombre aunque sea privado).
    [HarmonyPatch(typeof(CharacterAfflictions), "UpdateNormalStatuses")]
    internal static class UpdateNormalStatusesPatch
    {
        // "Postfix" = nuestro código corre DESPUÉS del original, cada frame.
        // __instance es el CharacterAfflictions del personaje que se está
        // actualizando (hay uno por jugador en la partida).
        static void Postfix(CharacterAfflictions __instance)
        {
            var character = __instance.character;

            // Solo tocamos NUESTRO personaje. Los de los demás jugadores se
            // simulan en sus propios PCs; modificarlos aquí no tendría efecto
            // real y podría desincronizar.
            if (character == null || !character.IsLocal) return;

            // Opcional: no curar mientras estás desmayado.
            if (Plugin.OnlyWhileConscious.Value && !character.data.fullyConscious) return;

            // Si no hay herida, no hay nada que curar.
            if (__instance.GetCurrentStatus(CharacterAfflictions.STATUSTYPE.Injury) <= 0f) return;

            // LastAddedStatus devuelve el Time.time de la última vez que subió
            // la herida. Si fue hace muy poco, respetamos el cooldown.
            if (Time.time - __instance.LastAddedStatus(CharacterAfflictions.STATUSTYPE.Injury)
                < Plugin.CooldownAfterHit.Value) return;

            // La barra de estados del juego va de 0 a 1 (1% = 0.01).
            // Calculamos cuánto restar POR SEGUNDO y lo multiplicamos por
            // Time.deltaTime (segundos desde el frame anterior) para que sea
            // independiente de los FPS. Mathf.Max evita dividir por cero.
            float perSecond = (Plugin.PercentPerTick.Value / 100f) / Mathf.Max(1f, Plugin.TickSeconds.Value);

            // SubtractStatus acumula internamente y solo mueve la barra en
            // pasos de 0.025 (2.5%). Por eso en pantalla se ve bajar a
            // escalones, aunque el ritmo promedio es el configurado.
            // decreasedNaturally: true = tratarlo como decaimiento natural
            // (igual que el veneno o el calor), no como si usaras un objeto.
            __instance.SubtractStatus(CharacterAfflictions.STATUSTYPE.Injury,
                perSecond * Time.deltaTime, fromRPC: false, decreasedNaturally: true);
        }
    }
}
