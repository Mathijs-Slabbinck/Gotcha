# TargetInfo

The targetInfo page is for the User (when logged in and in game that is still ongoing)

**When the player is still alive:**
This page will show the following info about the target:
- image (if turned on, otherwise placeholder (different per gender))
- real name (if turned on)
- nickname (if turned on)
- weapon (if turned on)
- hunter info (if Show Hunter is turned on): image, name, username (depending on settings)
- game rules (if any custom rules are set, plus auto-generated rules for active game modes)
- living players count (if turned on)
- living player names (if turned on)
- dead players list

**When the player has been eliminited:**
This page will show a summary with the follwing info:
- kills (listed, can be clicked trough)
- killer
- time of death
- living players count
- living player names (if turned on, or if ShowLivingPlayerNamesToDeath is on)
- dead players list

**When the player is a spectator:**
Shows game stats, living players, and dead players regardless of rule settings (since they are spectating, not playing).
