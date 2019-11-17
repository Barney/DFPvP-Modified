using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_PvPClass : MonoBehaviour
{
	private void Awake()
	{
		if (A_PvPClass.instance == null)
		{
			A_PvPClass.instance = this;
			A_Master.instance.WriteToOutputLog("PvP Class initialized.");
		}
		else if (A_PvPClass.instance != this)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

  //Checks for health change every 2 minutes and prepare a SmartFox game update to send that change since damage dealt between the two clients is not accurate.
	private IEnumerator PvPHPUpdate()
	{
		for (;;)
		{
      //Check for other users and ensure the player data is properly loaded since this script is loaded on startup.
			if (((SFSMultiplayer)GameObject.Find("SmartFoxObject").GetComponent(typeof(SFSMultiplayer))).UsersInInstance.Count > 0 && !DF34_6d6b8d6e9c7a8c0b2348af0e038e8df6d0752dd4.DF34_830bbe668bdf0a93412e1253e38b452f191dac88("GameplayStats_Health").Equals("-1"))
			{
      //These two obfuscated integers are the percentage value for health and armour, we send these to display the bars.
				string text = DFHUD.DF34_7c1c08c1eb7fe9f98e5219e85a2b812c6285fce0.ToString();
				string text2 = DFHUD.DF34_1da8681ac7ff0526a20a20c66866e6b3b21216db.ToString();
        
        //Separator is a carror, PvPUpdate is the header the Smartfox parser checks for our special gameplay update.
				this.SendUpdate(string.Concat(new string[]
				{
					"PvPUpdate^",
					text,
					"^",
					text2
				}));
        
        //I don't believe I use this, reminder to delete later.
				this.lastUpdateValues[0] = text;
				this.lastUpdateValues[1] = text2;
			}
			yield return new WaitForSeconds(2f);
		}
		yield break;
	}

	//Calls the Smartfox UserUpdate method to send the data.
	private void SendUpdate(string text)
	{
		try
		{
			((SFSMultiplayer)GameObject.Find("SmartFoxObject").GetComponent(typeof(SFSMultiplayer))).SendUserUpdate(text);
		}
		catch (Exception ex)
		{
			A_Master.instance.WriteToOutputLog("ERROR: " + ex.StackTrace);
		}
	}

	//Used to determine DF's colour based on percentage but currently this isn't used.
	private void DetermineBarColor(float a)
	{
		if (a >= 75f)
		{
			GUI.color = DFHUD.DF34_85f394d56c31be1061070c29f6070fb7542127c7;
			return;
		}
		if (a < 75f)
		{
			GUI.color = DFHUD.DF34_aca8da775e5959b078124746cf86066951509f6f;
			return;
		}
		if (a < 50f)
		{
			GUI.color = DFHUD.DF34_eb1fb5fe60de11ea9450b18f9ccd7641e37238f4;
			return;
		}
		if (a < 25f)
		{
			GUI.color = DFHUD.DF34_51b9424922e895314a865e16d9dff24aae525426;
		}
	}

	private void OnGUI()
	{
    
		float num = 200f;
		GUIStyle guistyle = new GUIStyle();
		guistyle.font = DFHUD.hudFont;
		guistyle.normal.textColor = DFHUD.DF34_51b9424922e895314a865e16d9dff24aae525426;
		new GUIStyle().font = DFHUD.miniFont;
    
    //We need to draw the HUD for each user in the instance that is using the modified client.
		foreach (KeyValuePair<string, A_PlayerClass> keyValuePair in this.playersOnScreen)
		{
			if (((SFSMultiplayer)GameObject.Find("SmartFoxObject").GetComponent(typeof(SFSMultiplayer))).UsersInInstance.Contains(keyValuePair.Value.userID))
			{
				A_PlayerClass value = keyValuePair.Value;
				GUI.Label(new Rect((float)(Screen.width - 165), num, 100f, 20f), value.name, guistyle);
				num += 20f;
				GUI.color = DFHUD.DF34_51b9424922e895314a865e16d9dff24aae525426;
				GUI.DrawTexture(new Rect((float)(Screen.width - 165), num, (float)value.hp, 5f), A_PvPClass.barTexture);
				num += 10f;
				GUI.color = Color.cyan;
				GUI.DrawTexture(new Rect((float)(Screen.width - 165), num, (float)value.armour, 5f), A_PvPClass.barTexture);
				GUI.color = Color.white;
				num += 20f;
			}
		}
	}

	//This enumerator checks every second for damage taken and sends a different gameplay update with the damage taken.
	private IEnumerator PvPHitTaken()
	{
		for (;;)
		{
			if (((SFSMultiplayer)GameObject.Find("SmartFoxObject").GetComponent(typeof(SFSMultiplayer))).UsersInInstance.Count > 0 && !DF34_6d6b8d6e9c7a8c0b2348af0e038e8df6d0752dd4.DF34_830bbe668bdf0a93412e1253e38b452f191dac88("GameplayStats_Health").Equals("-1"))
			{
       //Initialization
				if (this.lastHP == 0)
				{
					this.lastHP = int.Parse(DF34_6d6b8d6e9c7a8c0b2348af0e038e8df6d0752dd4.DF34_830bbe668bdf0a93412e1253e38b452f191dac88("GameplayStats_Health")) + int.Parse(DF34_6d6b8d6e9c7a8c0b2348af0e038e8df6d0752dd4.DF34_830bbe668bdf0a93412e1253e38b452f191dac88("GameplayStats_Armour"));
				}
				else
				{
          //Calculate current total health by adding HP + Armour together.
					int current = int.Parse(DF34_6d6b8d6e9c7a8c0b2348af0e038e8df6d0752dd4.DF34_830bbe668bdf0a93412e1253e38b452f191dac88("GameplayStats_Health")) + int.Parse(DF34_6d6b8d6e9c7a8c0b2348af0e038e8df6d0752dd4.DF34_830bbe668bdf0a93412e1253e38b452f191dac88("GameplayStats_Armour"));
					if (this.lastHP != current)
					{
            //Send the damage taken and the current player position since we're going to display a damage number where the hit actually occurred.
						string text = (this.lastHP - current).ToString();
						string text2 = string.Concat(new object[]
						{
							"(",
							this.mainPlayer.transform.position.x,
							",",
							2,
							",",
							this.mainPlayer.transform.position.z,
							")"
						});
            
            //PvPHit is the header for this one, carrot character to split again.
						this.SendUpdate(string.Concat(new string[]
						{
							"PvPHit^",
							text,
							"^",
							text2
						}));
            //Update lastHP after the update was sent out.
						this.lastHP = current;
					}
				}
			}
			yield return new WaitForSeconds(1f);
		}
		yield break;
	}

	//Get the reference to the main player gameobject, called in the update.
	private void GetReferenceToPlayer()
	{
		if (this.mainPlayer == null)
		{
			this.mainPlayer = GameObject.Find("Player0");
		}
	}

	private void Update()
	{
		this.GetReferenceToPlayer();
	}

	//Modular enable/disable so the client acts normal when disabled.
	public static bool isEnabledAndNotNull()
	{
		return A_PvPClass.instance != null && A_PvPClass.instance.enabled;
	}

	private void OnEnable()
	{
		A_Master.instance.WriteToOutputLog("PvP Features have been enabled!");
		base.StartCoroutine(this.PvPHPUpdate());
		base.StartCoroutine(this.PvPHitTaken());
	}
	private void OnDisable()
	{
		A_Master.instance.WriteToOutputLog("PvP Features have been disabled!");
		base.StopAllCoroutines();
	}

  //singleton
	public static A_PvPClass instance;

	//unused iirc
	public string[] lastUpdateValues = new string[3];

	//All players currently in the instance using the client.
	public Dictionary<string, A_PlayerClass> playersOnScreen = new Dictionary<string, A_PlayerClass>(20);

	//initialized by the HUD class.
	public static Texture2D barTexture;

	private int lastHP;
  
	public GameObject mainPlayer;
}
