# You Can Heal

[Source on GitHub](https://github.com/Benjamin-Navarrete/peak-you-can-heal)

Injury slowly heals on its own, so if you go too long without bandages or a med kit you still recover. Default is 1% per minute: a full injury bar takes 100 minutes, so healing items are still worth picking up. The bar drops in 2.5% steps, so the first one shows after 2.5 minutes.

## Config
`BepInEx/config/dralfredo.YouCanHeal.cfg` (created on first launch), or the Config editor in r2modman:
- `PercentPerTick` - Injury healed per tick, in % (default 1)
- `TickSeconds` - seconds between ticks (default 60)
- `CooldownAfterHit` - seconds without healing after a new injury (default 10)
- `OnlyWhileConscious` - no healing while passed out (default true)

Client-side. Only affects your own character.
