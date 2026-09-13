# You Can Heal

[Thunderstore](https://thunderstore.io/c/peak/p/DrAlfredo/YouCanHeal/) · [Source](https://github.com/Benjamin-Navarrete/peak-you-can-heal)

Injury slowly heals on its own, so if you go too long without bandages or a med kit you still recover. Default is 1% per minute: a full injury bar takes 100 minutes, so healing items are still worth picking up. The bar drops in 2.5% steps, so the first one shows after 2.5 minutes.

## Config
`BepInEx/config/dralfredo.YouCanHeal.cfg` (created on first launch), or the Config editor in r2modman:
- `PercentPerTick` - Injury healed per tick, in % (default 1)
- `TickSeconds` - seconds between ticks (default 60)
- `CooldownAfterHit` - seconds without healing after a new injury (default 10)
- `OnlyWhileConscious` - no healing while passed out (default true)

Client-side. Only affects your own character.

## Building from source

Requires the .NET SDK, PEAK installed via Steam, and BepInExPack_PEAK installed in an r2modman profile (the project references its DLLs).

```
dotnet build -c Release
```

The DLL lands in `bin/Release/netstandard2.1/YouCanHeal.dll`. Copy it into `pkg/` next to `manifest.json`, `icon.png` and `README.md`, zip the four files, and import the zip in r2modman (Settings > Profile > Import local mod) or upload it to Thunderstore.

## How it works

Harmony postfix on `CharacterAfflictions.UpdateNormalStatuses`. Every frame, if the local character has Injury and hasn't been hit recently, it calls `SubtractStatus(Injury, rate * deltaTime, decreasedNaturally: true)`. See `Plugin.cs`, it's heavily commented.

## License

MIT
