# DFPvP-Modified
## Personal Install Guide
Step by step guide to building the modpack up, since some features are so intertwined into other obfuscated classes this **is** necesasry.
## 1. Add A_Master Class
#### Required:
1. Find obfuscated settings menu check in DFHUD.
#### Errors:
Note: OnGUI() will throw errors, we need to find the if check for having the menu open found in DFHUD, find this prior, it should look like this.
```cs
if (DFHUD.DF34_16e3b6190b4a022282fee58e9f6162bbed6a1bb5 == 1)
```
Note: LoadMods() will throw errors temporarily as we haven't added the other mod classes yet, the best approach to this is to simply leave the code out for now. I will include the code for copy / paste reference here.
```cs
A_Master.modsObject.AddComponent<A_PvPClass>();
A_Master.modsObject.AddComponent<A_AggroWarning>();
A_Master.modsObject.AddComponent<A_ColourCloak>();
```
## 2. Add A_ColourCloak Class
This class should work just copy/paste.
## 3. Add A_AggroWarning Class
#### Required:
1. Find obfuscated rushSize variable (easy)
#### Errors:
Note: OnGUI() will throw an error, find obfuscated rushSize.
```cs
string text = "Zombies Remaining: " + DF34_68ac6f88d0583a466fc24ef7af066ef0e96db03d.rushSize.ToString();
```
Note: Update() will throw an error, find obfuscated rushSize prior, the line looks like this.
```cs
this.aggroWarningBarLength = (float)DF34_68ac6f88d0583a466fc24ef7af066ef0e96db03d.rushSize / (float)this.maxAggro * 100f;
```
## 4. Add A_PlayerClass
This is more of a struct and is just a copy paste, required before adding A_PvPClass.
## 5. Add A_PvPClass
#### Required:
2. Find the Obfuscated method that converts object obfuscated strings to english. (Searching strings should make this easy)
3. Find the DFHUD variables for Health and Armour (Search the HP/Armour Bar to make this easy)
#### Errors:
Note: PvPUpdate() will throw errors, many are replacing obfuscated variables with the new ones listed above.
## 6. Add A_DMGNum Class
Simple Copy/Paste.
## 7. Add A_Tracer Class
Simply Copy/Paste.
## 8. GlobalPlayerManifest Edit
Proceed to GlobalPlayerManifest's Awake() method and add the following line at the top. We need this to load our Master class.
```cs
new GameObject("ModManagersGO").AddComponent(typeof(A_Master));
```
## 9. A_Master Edit
Proceed to LoadMods() in A_Master and add the following lines back. We need this to load the mods.
```cs
A_Master.modsObject.AddComponent<A_PvPClass>();
A_Master.modsObject.AddComponent<A_AggroWarning>();
A_Master.modsObject.AddComponent<A_ColourCloak>();
```
## 10. HUD Edit
Proceed to HUD's Awake() method and add the following code at the top. We need this to assign barTexture in the master class.
```cs
try {
	A_Master.barTexture = this.barTexture;
}
catch (Exception) {}
```
## 11. DFHUD Edit
Proceed to DFHUD, we need to place our SaveSettings() call inside the settings menu "Done" and Escape/F1 checks.
Tip: Search the "Done" obfuscated string in Analyzer until you find the GUI.Button check for it that looks like the following, we should place 2 SaveModSettings Statement under each check at the top.

**Check1:**
```cs
if (GUI.Button(new Rect((float)(Screen.width / 2 - 50), (float)(Screen.height / 2 - 155 + 20 + 20 + 50 + 50 + 50 + 50 + 30 + 25 + 10 + 25), 100f, 40f), DF34_cb3702026e3b238888511d123c370319a167f1b3.DF34_29a493389248ee1d36499e9e02a3e6e1ef7af5dcf()) && DFHUD.DF34_c61d5d65b34a63e819842df7084e3d207f0d9eb8 == 0)
```
**Check2:** (Right below it)
```cs
if ((Input.GetKey(KeyCode.Escape) || Input.GetKey(KeyCode.F1)) && !DF34_62a1ed6e520076729e120dcd3be015a26e74abc0.DF34_5cbe0a8fc1a58790e94b00f10e5185ba8893149c)
```
The code to place under each is:
```cs
A_Master.instance.SaveModSettings();
```


