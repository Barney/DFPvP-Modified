# DFPvP-Modified
**Research to do ahead of time:**
1. Find Method which converts obfuscated strings to english.
2. Find proper rushSize variable for the Aggro Bar.
3. Find the check for if the settings menu is open. (In DFHUD)
4. Find the two variables for Health / Armour HP in DFHUD.
5. Find the RayCast class for weapons to implement DMG Numbers / Tracers.
6. Find the Method controling the weather.
7. I didn't include converting looting UI to include stats/colour since its alot of effort.

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
## 6. Add ShadowAndOutlineClass
Simple Copy/Paste.
## 7. Add A_DMGNum Class
Simple Copy/Paste.
## 8. Add A_Tracer Class
Simply Copy/Paste.
## 9. GlobalPlayerManifest Edit
Proceed to GlobalPlayerManifest's Awake() method and add the following line at the top. We need this to load our Master class.
```cs
new GameObject("ModManagersGO").AddComponent(typeof(A_Master));
```
## 10. A_Master Edit
Proceed to LoadMods() in A_Master and add the following lines back. We need this to load the mods.
```cs
A_Master.modsObject.AddComponent<A_PvPClass>();
A_Master.modsObject.AddComponent<A_AggroWarning>();
A_Master.modsObject.AddComponent<A_ColourCloak>();
```
## 11. HUD Edit
Proceed to HUD's Awake() method and add the following code at the top. We need this to assign barTexture in the master class.
```cs
try {
	A_Master.barTexture = this.barTexture;
}
catch (Exception) {}
```
## 12. DFHUD Edit
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

Next, we need to find the "Done" button for when we exit the inventory, this can be found by searching the "Done" obfuscated string. Once we find it, place the following code inside it, this is used to update the rainbow cloak if a user switches jackets.
```cs
if (A_ColourCloak.isEnabledAndNotNull())
				{
					A_ColourCloak.instance.UpdateSelectedSkinnedMeshRenderer();
				}
}
```
## 13. SFSMultiplayer Edit
Proceed to SFSMultiplayer found in Assembly-CSharp-firstpass, then to the parseData method. Copy/Paste the following code under this line:
```cs
char c = '$';
```
Code to Copy:
```cs
if (A_PvPClass.isEnabledAndNotNull() && message.Contains("PvPUpdate"))
	{
		try
		{
			A_Master.instance.WriteToOutputLog("PvPUpdate Received! Message: " + message);
			string[] array = message.Split(new char[]
			{
				'^'
			});
			if (!A_PvPClass.instance.getPlayersOnScreen().ContainsKey(sender.GetName()))
			{
				A_PvPClass.instance.getPlayersOnScreen().Add(sender.GetName(), new A_PlayerClass(sender.GetName(), int.Parse(array[1]), int.Parse(array[2]), sender.GetId()));
			}
			else
			{
				A_PlayerClass a_PlayerClass = A_PvPClass.instance.getPlayersOnScreen()[sender.GetName()];
				a_PlayerClass.setHP(int.Parse(array[1]));
				a_PlayerClass.setArmour(int.Parse(array[2]));
				a_PlayerClass.setUserID(sender.GetId());
				A_PvPClass.instance.getPlayersOnScreen()[sender.GetName()] = a_PlayerClass;
			}
			if (array.Length == 7)
			{
				A_DMGNum a_DMGNum = (A_DMGNum)new GameObject("newDMGNumGO").AddComponent(typeof(A_DMGNum));
				a_DMGNum.damageCritValue = float.Parse(array[3]);
				a_DMGNum.damageValue = float.Parse(array[3]);
				a_DMGNum.numberScreenPosition = Camera.mainCamera.WorldToScreenPoint(new Vector3(float.Parse(array[3]), float.Parse(array[4]), float.Parse(array[5])));
			}
		}
		catch (Exception ex)
		{
			A_Master.instance.WriteToOutputLog("Error in SFSMultiplayer PvPUpdate: " + ex.StackTrace);
		}
		return false;
	}
```
Then, in the JoinInstance method, paste the following code under the following lines. This code simply tells us all the players in the instance.

**Change1**
Place under:
```cs
this.smartFoxHandler.SendMessage("SystemMessage", "Entering new area instance...");
```

**Change 2**
Place under:
```cs
this.smartFoxHandler.SendMessage("SystemMessage", "Changing instance to meet " + this.MPInstanceFollow + "...");
```

Code to Copy:
```cs
this.smartFoxHandler.SendMessage("SystemMessage", userList.Count - 1 + " users in instance: " + text.Trim().TrimEnd(new char[]
		{
			','
		}));
```
**Note:** You will get an error regarding enumerator2, replace this block with the following.
```cs
foreach(string a in lastPlayersMet.Keys) {
	if(a == user.GetName()) {
		num3 += 2;
	}
}
```
## 14. SmartFoxHandler Edit
Proceed to SmartFoxHandler's HandleUserEnterRoom method and paste the following code at the top. This sets our countdown for PvPUpdate to send updates when a user joins the room, it is done this way to prevent multiple users joining at once and causing a crash.
```cs
try
	{
		if (A_PvPClass.isEnabledAndNotNull())
		{
			A_PvPClass.instance.setCountdown(2);
		}
	}
	catch (Exception)
	{
	}
```
## 15. Implementing Damage Numbers and Tracers.
This implementation is harder but easy to do with some searching. We need to find the method that contains the RayCast check for hitting players, enemies, or nothing, it looks similar to this.
```cs
private void DF34_76aef0c1433e15a6d16a7af7ff53d849f72105c4(object direction, object DF34_c3706496a498466304614facdf3df3facab92bd8, object force, object DF34_deea0477bade932bd6780926541d17d7d96cf018, object crit, object range, object DF34_9547af042872a3167e0d5c5ef2c3b5dd83639e5d)
```
The first change we need to make is under this if statement, it will look the same, we need this for the tracers.
```cs
if (Physics.Raycast(this.gNozz.transform.position, (Vector3)direction, out raycastHit, RuntimeServices.UnboxSingle(range), num))
```
Paste the following code at the top of it:
```cs
if (A_Master.instance.getTracers() && this.playerNum == 0)
			{
				((A_Tracer)new GameObject("TracerGO").AddComponent(typeof(A_Tracer))).CreateProjectileLine(this.gNozz.transform.position, direction2, Vector3.Distance(raycastHit.point, this.gNozz.transform.position));
			}
```
The second change we need to make is under a collider.CompareTag check which looks like this. We need this for the damage numbers.
```cs
if (raycastHit.collider.CompareTag(DF34_cb3702026e3b238888511d123c370319a167f1b3.DF34_bbac4ace293db48e4ef5fc061327880324aef45cf()))
```
Or for reference, written as in english:
```cs
if (raycastHit.collider.CompareTag("Enemy"))
```
Once we find this check, paste the following code at the top of it.
```cs
if (A_Master.instance.getDMGNumbers() && this.playerNum == 0)
				{
					A_DMGNum a_DMGNum = (A_DMGNum)new GameObject("newDMGNumGO").AddComponent(typeof(A_DMGNum));
					a_DMGNum.damageValue = RuntimeServices.UnboxSingle(DF34_c3706496a498466304614facdf3df3facab92bd8);
					a_DMGNum.numberScreenPosition = A_Master.instance.getMainCamera().WorldToScreenPoint(raycastHit.point);
					a_DMGNum.damageCritValue = RuntimeServices.UnboxSingle(crit);
				}
```
Finally, to add tracers for missed shots, add an else if after the following if block:
```cs
if (Physics.Raycast(this.gNozz.transform.position, (Vector3)direction, out raycastHit, RuntimeServices.UnboxSingle(range), num))
```
Code to place:
```cs
else if (A_Master.instance.getTracers() && this.playerNum == 0)
		{
			((A_Tracer)new GameObject("TracerGO").AddComponent(typeof(A_Tracer))).CreateProjectileLine(this.gNozz.transform.position, (Vector3)direction, RuntimeServices.UnboxSingle(range));
		}
```
## 16. Implementing Perma Daytime
I found this method by searching the obfuscated string for "Sun", it should look something like this.
```cs
public static void DF34_aef4c77c3fb9165115235c4a08ecc053e74e03c5(object DF34_b71f8c6d9e8cebcd927647c37613dec5027216f2)
```
At the top of this method once you find it, add the following code at the top.
```cs
if (A_Master.instance != null && A_Master.instance.getPermaDaytime() && (DF34_b71f8c6d9e8cebcd927647c37613dec5027216f2.Equals("night") || DF34_b71f8c6d9e8cebcd927647c37613dec5027216f2.Equals("rain") || DF34_b71f8c6d9e8cebcd927647c37613dec5027216f2.Equals("fog")))
		{
			DF34_b71f8c6d9e8cebcd927647c37613dec5027216f2 = "day";
		}
```
