# DFPvP-Modified
## Personal Install Guide
Step by step guide to building the modpack up, since some features are so intertwined into other obfuscated classes this **is** necesasry.
### 1. Add A_Master Class
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
### 2. Add A_ColourCloak Class
This class should work just copy/paste.
### 3. Add A_AggroWarning Class
#### Required:
1. Find obfuscated colours from DFHUD.
2. Find obfuscated rushSize variable (easy)
#### Errors:
Note: DetermineAggroBarColor(float) will throw an error, find the obfuscated colour variables in DFHUD prior, they should look like this.
```cs
if (valuePercentage >= 75f)
	{
		GUI.color = DFHUD.DF34_51b9424922e895314a865e16d9dff24aae525426;
	}
	if (valuePercentage < 75f)
	{
		GUI.color = DFHUD.DF34_eb1fb5fe60de11ea9450b18f9ccd7641e37238f4;
	}
	if (valuePercentage < 50f)
	{
		GUI.color = DFHUD.DF34_aca8da775e5959b078124746cf86066951509f6f;
	}
	if (valuePercentage < 25f)
	{
		GUI.color = DFHUD.DF34_85f394d56c31be1061070c29f6070fb7542127c7;
	}
  ```
Note: OnGUI() will throw an error, find obfuscated rushSize and RED colour prior, the lines look like this.
```cs
GUI.color = DFHUD.DF34_51b9424922e895314a865e16d9dff24aae525426;
string text = "Zombies Remaining: " + DF34_68ac6f88d0583a466fc24ef7af066ef0e96db03d.rushSize.ToString();
```
Note: Update() will throw an error, find obfuscated rushSize prior, the line looks like this.
```cs
this.aggroWarningBarLength = (float)DF34_68ac6f88d0583a466fc24ef7af066ef0e96db03d.rushSize / (float)this.maxAggro * 100f;
```
### 4. Add A_PlayerClass
This is more of a struct and is just a copy paste, required before adding A_PvPClass.
### 5. Add A_PvPClass
#### Required:
1. Find the RED colour in DFHUD, we found this above, just replace it.
2. Find the Obfuscated method that converts object obfuscated strings to english. (Searching strings should make this easy)
3. Find the DFHUD variables for Health and Armour (Search the HP/Armour Bar to make this easy)
#### Errors:
Note: PvPUpdate() will throw errors, many are replacing obfuscated variables with the new ones listed above.

### 6. GlobalPlayerManifest Edit
Proceed to GlobalPlayerManifest's Awake() method and add the following line at the top.
```cs
new GameObject("ModManagersGO").AddComponent(typeof(A_Master));
```
