using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary> This class controls the PvP Mod's behaviour, its functionality depends on code implemented inside SFSMultiplayer's
///parseData method.</summary>
public class A_PvPClass : MonoBehaviour
{
	private void Awake()
	{
		if (A_PvPClass.instance == null)
		{
			A_PvPClass.instance = this;
			A_Master.instance.WriteToOutputLog("PvP Class initialized.");
			base.enabled = false;
		}
		else if (A_PvPClass.instance != this)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	///<summary> When PvP Updates are enabled, this enumerator runs every second sending HP updates if the player isn't AFK or
	///if they receive damage, an HP update and a damage update. This Update can either have 3 indexes (0,health,armour) or 7
	///indexes (0,health,armour,damagedealt,x,y,z)</summary>
	private IEnumerator PvPHPUpdate()
	{
		
		for (;;)
		{
		try
		{	//If multiplayer exists, we are not alone and our HP was properly initialized, run.
			if (this.smartFoxObject != null && this.smartFoxObject.UsersInInstance.Count > 0 && !DF34_6d6b8d6e9c7a8c0b2348af0e038e8df6d0752dd4.DF34_830bbe668bdf0a93412e1253e38b452f191dac88("GameplayStats_Health").Equals("-1"))
			{
				//If we haven't ever sent a PvPUpdate, update our lastHP and send an update.
				if (this.lastHP == 0)
				{
					//Full HP = Health + Armour
					this.lastHP = DFHUD.DF34_7c1c08c1eb7fe9f98e5219e85a2b812c6285fce0 + DFHUD.DF34_1da8681ac7ff0526a20a20c66866e6b3b21216db;
					this.SendUpdate("PvPUpdate^" + DFHUD.DF34_7c1c08c1eb7fe9f98e5219e85a2b812c6285fce0.ToString() + "^" + DFHUD.DF34_1da8681ac7ff0526a20a20c66866e6b3b21216db.ToString());
					A_Master.instance.WriteToOutputLog("Sent first PvPUpdate.");
				}
				else
				{
					//Full HP = Health + Armour
					int num = DFHUD.DF34_7c1c08c1eb7fe9f98e5219e85a2b812c6285fce0 + DFHUD.DF34_1da8681ac7ff0526a20a20c66866e6b3b21216db;
					//If our HP has changed from the last, prepare a damage number update.
					if (this.lastHP != num)
					{
						string hp = DFHUD.DF34_7c1c08c1eb7fe9f98e5219e85a2b812c6285fce0.ToString();
						string armour = DFHUD.DF34_1da8681ac7ff0526a20a20c66866e6b3b21216db.ToString();
						string damage = (this.lastHP - num).ToString();
						Vector3 position = A_Master.instance.getMainPlayer().transform.position;
						string str3 = position.x.ToString();
						string str2 = "^4^";
						position = A_Master.instance.getMainPlayer().transform.position;
						string damagePos = str3 + str2 + position.z.ToString();
						this.SendUpdate(string.Concat(new string[]
						{
							"PvPUpdate^",
							hp,
							"^",
							armour,
							"^",
							damage,
							"^",
							damagePos
						}));
						//Update the last values after we send.
						this.lastHP = num;
						this.lastUpdateValues[0] = hp;
						this.lastUpdateValues[1] = armour;
						A_Master.instance.WriteToOutputLog("Damage detected, sent PvPUpdate.");
					}
					/*If the Player isn't AFK but hasn't received any damage, we continuously send health only updates
					//incase more users join.*/
					else if (!A_Master.instance.isPlayerAFK())
					{
						this.SendUpdate("PvPUpdate^" + DFHUD.DF34_7c1c08c1eb7fe9f98e5219e85a2b812c6285fce0.ToString() + "^" + DFHUD.DF34_1da8681ac7ff0526a20a20c66866e6b3b21216db.ToString());
						A_Master.instance.WriteToOutputLog("Sent PvP update as player is not AFK.");
					}
				}
			}
		}
		catch (Exception ex)
		{
			this.SendError("PvPHPUpdate() - " + ex.StackTrace);
		}
		//Loop every second.
		yield return new WaitForSeconds(1f);
	}
	yield break;
	}

	///<summary>Method which calls the multiplayer SendUserUpdate with our custom update.</summary>
	private void SendUpdate(string text)
	{
		try
		{
			this.smartFoxObject.SendUserUpdate(text);
		}
		catch (Exception ex)
		{
			this.SendError("SendUpdate() - " + ex.StackTrace);
		}
	}

	///<summary> OnGUI() in A_PvPClass displays the name,health, and armour of each user sending PvPUpdates below the minimap.</summary>
	private void OnGUI()
	{
		try
		{
			
			float num = 200f;
			GUIStyle guistyle = new GUIStyle();
			guistyle.font = DFHUD.hudFont;
			guistyle.normal.textColor = DFHUD.DF34_51b9424922e895314a865e16d9dff24aae525426;
			new GUIStyle().font = DFHUD.miniFont;
			
			//For each user who sent us a pvpupdate at some point...
			foreach (KeyValuePair<string, A_PlayerClass> keyValuePair in this.playersOnScreen)
			{
				/*If we are connected to multiplayer and this user is also in the multiplayer instance with us, display their data.
				We never remove players, but if they leave and rejoin their data is simply updated with every update.*/
				if (this.smartFoxObject != null && this.smartFoxObject.UsersInInstance.Contains(keyValuePair.Value.getUserID()))
				{
					//Draw their data on the screen.
					A_PlayerClass value = keyValuePair.Value;
					GUI.Label(new Rect((float)(Screen.width - 165), num, 165f, 20f), value.getName(), guistyle);
					num += 20f;
					GUI.color = Color.cyan;
					GUI.DrawTexture(new Rect((float)(Screen.width - 165), num, (float)value.getArmour(), 5f), A_Master.barTexture);
					num += 10f;
					GUI.color = DFHUD.DF34_51b9424922e895314a865e16d9dff24aae525426;
					GUI.DrawTexture(new Rect((float)(Screen.width - 165), num, (float)value.getHP(), 5f), A_Master.barTexture);
					GUI.color = Color.white;
					num += 20f;
				}
			}
		}
		catch (Exception ex)
		{
			this.SendError("OnGUI() - " + ex.StackTrace);
		}
	}

	///<summary>Method used to make sure the Mod Component is created and the mod is enabled.</summary>
	public static bool isEnabledAndNotNull()
	{
		return A_PvPClass.instance != null && A_PvPClass.instance.enabled;
	}

	///<summary>OnEnable() in A_PvPClass starts the update enumerators.</summary>
	private void OnEnable()
	{
		A_Master.instance.WriteToOutputLog("PvP Features have been enabled!");
		base.StartCoroutine(this.PvPUpdate());
	}

	///<summary>OnDisable() in A_PvPClass stops all the enumerators.</summary>
	private void OnDisable()
	{
		A_Master.instance.WriteToOutputLog("PvP Features have been disabled!");
		base.StopAllCoroutines();
	}

	///<summary>Update() in A_PvPClass finds the SFSMultiplayer Object if it doesn't exist.</summary>
	private void Update()
	{
		if (this.smartFoxObject == null)
		{
			this.smartFoxObject = (SFSMultiplayer)GameObject.Find("SmartFoxObject").GetComponent(typeof(SFSMultiplayer));
		}
	}

	///<summary>Method for Debugging that puts the class name.</summary>
	private void SendError(string message)
	{
		A_Master.instance.WriteToOutputLog("ERROR in A_PvPClass: " + message);
	}

	//Getter for the player dictionary.
	public Dictionary<string, A_PlayerClass> getPlayersOnScreen(){ return this.playersOnScreen; }

	//Singleton instance.
	public static A_PvPClass instance;

	//Srray storing the last update values of a PvPUpdate.
	private string[] lastUpdateValues = new string[3];

	//Dictionary storing Players who entered our instance with the modpack.
	private Dictionary<string, A_PlayerClass> playersOnScreen = new Dictionary<string, A_PlayerClass>(20);

	//Integer storing the lastHP update.
	private int lastHP;

	//Holder for the SmartFox object.
	private SFSMultiplayer smartFoxObject;
}
