# You Can Heal

A small [PEAK](https://store.steampowered.com/app/3527290/PEAK/) mod: Injury slowly heals on its own.

[![Thunderstore](https://img.shields.io/thunderstore/v/DrAlfredo/YouCanHeal?label=Thunderstore&color=23ffac&labelColor=0f0f1f)](https://thunderstore.io/c/peak/p/DrAlfredo/YouCanHeal/)
[![Downloads](https://img.shields.io/thunderstore/dt/DrAlfredo/YouCanHeal?label=downloads&labelColor=0f0f1f)](https://thunderstore.io/c/peak/p/DrAlfredo/YouCanHeal/)
[![License](https://img.shields.io/badge/license-MIT-blue)](LICENSE)

## Why

In vanilla PEAK, Injury never goes away on its own. If your group runs out of bandages and med kits, you carry it to the top. This mod adds a very slow passive recovery: enough to matter over a long run, not enough to make healing items pointless.

At default settings a full Injury bar takes **100 minutes** to clear. A bandage still saves you 25 minutes, a med kit still saves you more than a run.

## How it behaves

- Heals **1% of Injury per minute** while you are conscious.
- Taking a new injury pauses healing for **10 seconds**. Progress is not lost.
- PEAK stores statuses in 2.5% steps, so the bar visibly drops **2.5% every 2.5 minutes**. Nothing happens for the first 2.5 minutes; that is expected.
- **Client-side.** Only your own character is affected. Other players do not need the mod.

## Install

**r2modman / Thunderstore Mod Manager:** search for *YouCanHeal* under PEAK and install. Launch with *Start modded*.

**Manual:** install [BepInExPack_PEAK](https://thunderstore.io/c/peak/p/BepInEx/BepInExPack_PEAK/), then drop `YouCanHeal.dll` into `BepInEx/plugins/`.

## Config

The file `BepInEx/config/dralfredo.YouCanHeal.cfg` is created on first launch. Edit it directly or through the Config editor in r2modman.

| Key | Default | Meaning |
|---|---|---|
| `PercentPerTick` | `1` | Injury healed per tick, in percent |
| `TickSeconds` | `60` | Seconds between ticks |
| `CooldownAfterHit` | `10` | Seconds without healing after a new injury |
| `OnlyWhileConscious` | `true` | Do not heal while passed out |

Examples: `PercentPerTick = 2.5` and `TickSeconds = 60` gives a visible drop every minute and a full bar in 40 minutes. `TickSeconds = 120` halves the rate.

## How it works

One Harmony postfix on `CharacterAfflictions.UpdateNormalStatuses`, the method PEAK already uses every frame to decay poison, heat and drowsiness. If the local character has Injury and was not hit in the last `CooldownAfterHit` seconds, it calls the game's own `SubtractStatus(Injury, rate * deltaTime, decreasedNaturally: true)`. The game's accumulator handles the 2.5% steps, network sync and UI. See [Plugin.cs](Plugin.cs); every line is commented.

## Building

Requires the .NET SDK, PEAK installed through Steam and BepInExPack_PEAK in an r2modman profile. The project file references the game and BepInEx assemblies from those locations; adjust the paths in [YouCanHeal.csproj](YouCanHeal.csproj) if yours differ.

```
dotnet build -c Release
```

Output: `bin/Release/netstandard2.1/YouCanHeal.dll`. To package for Thunderstore, copy the DLL into `pkg/` next to `manifest.json`, `icon.png` and `README.md`, and zip those four files plus `CHANGELOG.md`.

## License

[MIT](LICENSE)
