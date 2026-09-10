# v0.6.2
- Thunderstore listing retry (same build as 0.6.1)

# v0.6.1
- Restored a usable default window size on Valheim 1.0 (Unity 6 was saving ~300x200 because Screen size is not ready in the plugin constructor)

# v0.6.0
- Updated for Valheim 1.0 (Unity 6)
- BepInEx dependency updated to denikson-BepInExPack_Valheim-5.4.2350
- Main-menu Open Config button uses TextMeshPro labels (the old Unity UI Text match never succeeded)
- Console reload command now patches Terminal.InputText (Console inherits Terminal; InputText is no longer on Console)
- Null-safe menu button setup so a missing Show Player.log control no longer throws on the title screen

# v0.5.0
- Uploaded to Thunderstore from Nexus Mods (aedenthorn Configuration Manager)
