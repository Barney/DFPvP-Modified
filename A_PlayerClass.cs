using System;
using System.Collections.Generic;
using SmartFoxClientAPI.Data;
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

	private void DetermineBarColor(float a)
	{
		if (a >= 75f)
		{
			GUI.color = A_Master.GREEN;
			return;
		}
		if (a < 75f)
		{
			GUI.color = A_Master.YELLOW;
			return;
		}
		if (a < 50f)
		{
			GUI.color = A_Master.ORANGE;
			return;
		}
		if (a < 25f)
		{
			GUI.color = A_Master.RED;
		}
	}

	private void OnGUI()
	{
		try
		{
			float num = 200f;
			GUIStyle guistyle = new GUIStyle();
			guistyle.font = DFHUD.hudFont;
			guistyle.normal.textColor = A_Master.RED;
			new GUIStyle().font = DFHUD.miniFont;
			foreach (KeyValuePair<string, A_PlayerClass> pair in this.playersOnScreen)
			{
				A_PlayerClass value = pair.Value;
				if (this.smartFoxObject != null && this.smartFoxObject.UsersInInstance.Contains(value.userID))
				{
					GUI.Label(new Rect((float)(Screen.width - 165), num, 100f, 20f), value.name, guistyle);
					num += 20f;
					GUI.color = A_Master.RED;
					GUI.DrawTexture(new Rect((float)(Screen.width - 165), num, (float)value.hp, 5f), A_PvPClass.barTexture);
					num += 10f;
					GUI.color = Color.cyan;
					GUI.DrawTexture(new Rect((float)(Screen.width - 165), num, (float)value.armour, 5f), A_PvPClass.barTexture);
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

	public static bool isEnabledAndNotNull()
	{
		return A_PvPClass.instance != null && A_PvPClass.instance.enabled;
	}

	private void OnEnable()
	{
		A_Master.instance.WriteToOutputLog("PvP Features have been enabled!");
	}

	private void OnDisable()
	{
		A_Master.instance.WriteToOutputLog("PvP Features have been disabled!");
		base.StopAllCoroutines();
	}

	private void Update()
	{
		if (this.smartFoxObject == null)
		{
			this.smartFoxObject = (SFSMultiplayer)GameObject.Find("SmartFoxObject").GetComponent(typeof(SFSMultiplayer));
		}
	}

	private void SendError(string message)
	{
		A_Master.instance.WriteToOutputLog("ERROR in A_PvPClass: " + message);
	}

	public string PvPHPUpdate()
	{
		string hp = DFHUD.DF34_e87ba399deaf3e82d6243a9eb1337765fd97817c.ToString();
		string ar = DFHUD.DF34_d54a5b283003efb3ba74882767bb88a6054bd95f.ToString();
		return "|B$" + hp + "^" + ar;
	}

	public string PvPHitTaken()
	{
		//Send damage update and clear the data for the next update
		string result = "|C$" + this.damageTaken + "^" + "(" + A_Master.instance.getMainPlayer().transform.position.x + "," + 2 + "," + A_Master.instance.getMainPlayer().transform.position.z + ")";

		this.damageTaken = 0;
		this.wasHit = false;
		return result;
	}

	public void ApplyDamage(int damageTaken)
	{
		this.damageTaken += damageTaken;
		this.wasHit = true;
	}

	public bool WasHit()
	{
		return this.wasHit;
	}

	public void ParsePvPData(string message, User sender)
	{
		//A_Master.instance.WriteToOutputLog("Message from: " + sender.GetName() + " is: " + message);
		int indexHPUpdate = message.IndexOf("|B$");
		if (indexHPUpdate > -1)
		{
			//A_Master.instance.WriteToOutputLog("We have the HP Update Data");
			string[] percentages = message.Substring(indexHPUpdate + 3).Split('|')[0].Split('^');
			//A_Master.instance.WriteToOutputLog("Name: " + sender.GetName() + " HP Percent: " + Convert.ToInt32(percentages[0]) + " Armour Percent: " + Convert.ToInt32(percentages[1]));
		
			if (!this.playersOnScreen.ContainsKey(sender.GetName()))
			{
				this.playersOnScreen.Add(sender.GetName(), new A_PlayerClass(sender.GetName(), Convert.ToInt32(percentages[0]), Convert.ToInt32(percentages[1]), "Nobody", sender.GetId()));
			}
			else
			{
				A_PlayerClass a_PlayerClass = A_PvPClass.instance.playersOnScreen[sender.GetName()];
				a_PlayerClass.hp = Convert.ToInt32(percentages[0]);
				a_PlayerClass.armour = Convert.ToInt32(percentages[1]);
				a_PlayerClass.lastHitBy = "Nobody";
				a_PlayerClass.userID = sender.GetId();
				A_PvPClass.instance.playersOnScreen[sender.GetName()] = a_PlayerClass;
			}
			//Check for Hit Data
			int hitTakenUpdate = message.IndexOf("|C$");
			if (hitTakenUpdate > -1)
			{
				//A_Master.instance.WriteToOutputLog("We have a damage update");
				string[] damageData = message.Substring(hitTakenUpdate + 3).Split('|')[0].Split('^');
				string damageNum = damageData[0];
				//A_Master.instance.WriteToOutputLog("Damage is: " + damageNum + " Coordinates are: " + damageData[1]);
				string[] coordinateData = damageData[1].TrimStart('(').TrimEnd(')').Split(',');
				float x = Convert.ToSingle(coordinateData[0]);
				float y = Convert.ToSingle(coordinateData[1]);
				float z = Convert.ToSingle(coordinateData[2]);
				//A_Master.instance.WriteToOutputLog("X: " + x + " Y: " + y + " Z: " + z);
				
				//Do damage number
				A_DMGNum a_DMGNum = (A_DMGNum)new GameObject("newDMGNumGO").AddComponent(typeof(A_DMGNum));
				a_DMGNum.damageCritValue = Convert.ToSingle(damageNum);
				a_DMGNum.damageValue = Convert.ToSingle(damageNum);
				a_DMGNum.numberScreenPosition = Camera.mainCamera.WorldToScreenPoint(new Vector3(x, y, z));
			}
		}
	}

	public static A_PvPClass instance;

	public string[] lastUpdateValues = new string[3];

	public Dictionary<string, A_PlayerClass> playersOnScreen = new Dictionary<string, A_PlayerClass>(20);

	public static Texture2D barTexture;

	private int lastHP;

	public SFSMultiplayer smartFoxObject;

	private int damageTaken;

	private bool wasHit;
}
