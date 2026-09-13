# You Can Heal

**By dr. alfredo**

Injury slowly heals on its own, so if you go too long without finding bandages or a med kit you still recover. It's slow on purpose: a full injury bar takes 100 minutes, so healing items are still worth picking up.

## Heads up: it looks like nothing happens at first
PEAK stores statuses in 2.5% steps, so the injury bar can only drop 2.5% at a time. At the default rate (1% per minute) the mod quietly accumulates healing and the bar drops 2.5% every 2.5 minutes. Taking a new injury pauses healing for 10 seconds but does not reset what's accumulated. Check the BepInEx log for "You Can Heal loaded" if you want to confirm it's running.

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

Client-side. Only affects your own character; other players don't need it.
