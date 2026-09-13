# You Can Heal

**By dr. alfredo**

Injury slowly heals on its own, so if you go too long without finding bandages or a med kit you still recover. It's slow on purpose: a full injury bar takes 100 minutes, so healing items are still worth picking up.

## Defaults
- 1% of Injury healed every 60 seconds
- 10 second pause after taking a new injury
- Only while conscious

## Config
Edit `BepInEx/config/dralfredo.YouCanHeal.cfg` (created on first launch), or use the Config editor in r2modman:
- `PercentPerTick` - how much Injury is healed per tick (default 1)
- `TickSeconds` - seconds between ticks (default 60)
- `CooldownAfterHit` - seconds without healing after a new injury (default 10)
- `OnlyWhileConscious` - don't heal while passed out (default true)

Note: the game moves the status bar in 2.5% steps, so you'll see it drop every 2.5 minutes at default settings.

Client-side. Only affects your own character; other players don't need it.
