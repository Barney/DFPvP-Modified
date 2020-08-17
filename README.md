# DFPvP-Modified
**Research to do ahead of time:**
1. Find Method which converts obfuscated strings to english.
2. Find the two variables for Health / Armour HP in DFHUD.

## Personal Install Guide
Step by step guide to building the modpack up, since some features are so intertwined into other obfuscated classes this **is** necesasry.
## 1. Add A_Master Class
Note: LoadMods() will throw errors temporarily as we haven't added the other mod classes yet, the best approach to this is to simply leave the code out for now. I will include the code for copy / paste reference here.
```cs
A_Master.modsObject.AddComponent<A_PvPClass>();
```
## 2. Add A_PlayerClass
This is more of a struct and is just a copy paste, required before adding A_PvPClass.
## 3. Add A_PvPClass
#### Required:
2. Find the Obfuscated method that converts object obfuscated strings to english. (Searching strings should make this easy)
3. Find the DFHUD variables for Health and Armour (Search the HP/Armour Bar to make this easy)
#### Errors:
Note: PvPUpdate() will throw errors, many are replacing obfuscated variables with the new ones listed above.
## 4. Add ShadowAndOutlineClass
Simple Copy/Paste.
## 5. Add A_DMGNum Class
Simple Copy/Paste.
## 6. GlobalPlayerManifest Edit
Proceed to GlobalPlayerManifest's Awake() method and add the following line at the top. We need this to load our Master class.
```cs
new GameObject("ModManagersGO").AddComponent(typeof(A_Master));
```
## 7. A_Master Edit
Proceed to LoadMods() in A_Master and add the following lines back. We need this to load the mods.
```cs
A_Master.modsObject.AddComponent<A_PvPClass>();
```
## 8. HUD Edit
Proceed to HUD's Awake() method and add the following code at the top. We need this to assign barTexture in the pvp class.
```cs
try {
	A_PvPClass.barTexture = this.barTexture;
}
catch (Exception) {}
```
## 13. SFSMultiplayer Edit
Proceed to SFSMultiplayer found in Assembly-CSharp-firstpass, then to the parseData method. Copy/Paste the following code at the top:
```cs
if (A_PvPClass.isEnabledAndNotNull()) {
	A_PvPClass.instance.ParsePvPData(message, sender);
}
```
## 14. SmartFoxHandler Edit
Proceed to SmartFoxHandler's SendMPData (Hashtable,bool) as parameters. method and paste the following code above the if(leave) check.
```cs
if (A_PvPClass.isEnabledAndNotNull()) {
	text += A_PvPClass.instance.PvPHPUpdate();
	if (A_PvPClass.instance.WasHit()) {
		text += A_PvPClass.instance.PvPHitTaken();
	}
}
```
## 15. Implementing ApplyDamage Function.
Search Damage Reduction and implement the following code inside that if check. Damage is the first parameter in the function. Make sure to do this after the multiplier.
```cs
if (A_PvPClass.isEnabledAndNotNull()) {
	A_PvPClass.instance.ApplyDamage(Convert.ToInt32(DF34_c24aeffadba9fe40db1196672e2d581d0b0d126b));
}
```
