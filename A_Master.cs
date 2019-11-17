using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

//Controls all the mods and base functionality.
public class A_Master : MonoBehaviour
{
	private void Awake()
	{
		if (A_Master.instance == null)
		{
			A_Master.instance = this;
			A_Master.modsObject = base.gameObject;
			this.OutputLogStartup();
			A_Master.instance.WriteToOutputLog("Master Class initialized.");
      
      //Add mod components.
			this.LoadMods();
		}
		else if (A_Master.instance != this)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}
	private void Start()
	{
   //Get the user settings and initialize the variables.
		this.displayBulletTracers = this.intToBool(PlayerPrefs.GetInt("TracerToggle", 0));
		this.displayDMGNumbers = this.intToBool(PlayerPrefs.GetInt("DamageNumberToggle", 0));
		this.displayFullEXP = this.intToBool(PlayerPrefs.GetInt("FullEXP", 0));
		this.displayLevelCapEXP = this.intToBool(PlayerPrefs.GetInt("LevelCapEXP", 0));
		this.pvpFeatures = this.intToBool(PlayerPrefs.GetInt("PvPFeatures", 0));
	}

	private void Update()
	{
		this.GetReferenceToPlayerGameObject();
		this.CheckPlayerAFKState();
	}

	private void OnGUI()
	{
		this.watermarkStyle.normal.textColor = this.watermarkColor;
		this.watermarkStyle.fontSize = 12;
		GUI.skin = DFHUD.mainSkin;
		GUI.Label(new Rect((float)Screen.width - 130f, (float)Screen.height - 17f, 300f, 50f), A_Master.modpackVersion, this.watermarkStyle);
	//Displays the custom settings in the settings menu if this is open.
  if (DFHUD.DF34_16e3b6190b4a022282fee58e9f6162bbed6a1bb5 == 1)
		{
			GUI.Box(new Rect((float)(Screen.width / 2 + 150), (float)(Screen.height / 2 - 155), 220f, 180f), "Mod Settings");
			GUI.Label(new Rect((float)(Screen.width / 2 + 170), (float)(Screen.height / 2 - 155 + 20 + 15), 200f, 40f), "DMG Numbers:");
			this.displayDMGNumbers = GUI.Toggle(new Rect((float)(Screen.width / 2 + 320), (float)(Screen.height / 2 - 155 + 20 + 10), 20f, 20f), this.displayDMGNumbers, string.Empty);
			GUI.Label(new Rect((float)(Screen.width / 2 + 170), (float)(Screen.height / 2 - 155 + 20 + 45), 200f, 40f), "Bullet Tracers:");
			this.displayBulletTracers = GUI.Toggle(new Rect((float)(Screen.width / 2 + 320), (float)(Screen.height / 2 - 155 + 20 + 40), 20f, 20f), this.displayBulletTracers, string.Empty);
			GUI.Label(new Rect((float)(Screen.width / 2 + 170), (float)(Screen.height / 2 - 155 + 20 + 75), 200f, 40f), "Show Full EXP:");
			this.displayFullEXP = GUI.Toggle(new Rect((float)(Screen.width / 2 + 320), (float)(Screen.height / 2 - 155 + 20 + 70), 20f, 20f), this.displayFullEXP, string.Empty);
			GUI.Label(new Rect((float)(Screen.width / 2 + 170), (float)(Screen.height / 2 - 155 + 20 + 105), 200f, 40f), "Show LvL Cap EXP:");
			this.displayLevelCapEXP = GUI.Toggle(new Rect((float)(Screen.width / 2 + 320), (float)(Screen.height / 2 - 155 + 20 + 100), 20f, 20f), this.displayLevelCapEXP, string.Empty);
			GUI.Label(new Rect((float)(Screen.width / 2 + 170), (float)(Screen.height / 2 - 155 + 20 + 135), 200f, 40f), "PvP Features:");
			this.pvpFeatures = GUI.Toggle(new Rect((float)(Screen.width / 2 + 320), (float)(Screen.height / 2 - 155 + 20 + 130), 20f, 20f), this.pvpFeatures, string.Empty);
		}
	}

	private void GetReferenceToPlayerGameObject()
	{
		if (this.mainCamera == null)
		{
			this.mainCamera = (Camera)GameObject.Find("MainCamera").GetComponent(typeof(Camera));
		}
	}

	//Check if the player is AFK by mass.
	private void CheckPlayerAFKState()
	{
		if (this.player0GameObject != null)
		{
			if (this.player0GameObject.rigidbody.mass < 9999f)
			{
				this.playerIsAFK = false;
				return;
			}
			if (this.player0GameObject.rigidbody.mass >= 9999f)
			{
				this.playerIsAFK = true;
			}
		}
	}

	//Debug tool, can be used to write errors to a local in LocalLow/Creaky Corpse
	public void WriteToOutputLog(string line)
	{
		StreamWriter streamWriter = new StreamWriter(Application.persistentDataPath + "/OutputLog.txt", true);
		streamWriter.WriteLine(DateTime.Now + " - " + line);
		streamWriter.Close();
	}

	//Startup log writing.
	public void OutputLogStartup()
	{
		StreamWriter streamWriter = new StreamWriter(Application.persistentDataPath + "/OutputLog.txt", false);
		streamWriter.WriteLine("Dead Frontier Modpack Output Log");
		streamWriter.WriteLine("Unity Version: " + Application.unityVersion);
		streamWriter.WriteLine("Modpack Version: " + A_Master.modpackVersion);
		streamWriter.WriteLine("--------------------------------");
		streamWriter.Close();
	}

	//Save player preferences to registry.
	public void SaveModSettings()
	{
		PlayerPrefs.SetInt("TracerToggle", this.boolToInt(this.displayBulletTracers));
		PlayerPrefs.SetInt("DamageNumberToggle", this.boolToInt(this.displayDMGNumbers));
		PlayerPrefs.SetInt("FullEXP", this.boolToInt(this.displayFullEXP));
		PlayerPrefs.SetInt("LevelCapEXP", this.boolToInt(this.displayLevelCapEXP));
		PlayerPrefs.SetInt("PvPFeatures", this.boolToInt(this.pvpFeatures));
		this.HandleModSettings();
	}

	private int boolToInt(bool value)
	{
		if (value)
		{
			return 1;
		}
		return 0;
	}

	private bool intToBool(int value)
	{
		return value != 0;
	}

	//Add the mod classes as components.
	private void LoadMods()
	{
		A_Master.instance.WriteToOutputLog("Adding mod components.");
		A_Master.modsObject.AddComponent<A_PvPClass>();
	}

	//Enables or disables mods based on their current toggle. (Stops the update loops and coroutines)
	private void HandleModSettings()
	{
		if (this.pvpFeatures && !A_PvPClass.instance.enabled)
		{
			A_PvPClass.instance.enabled = true;
			return;
		}
		if (!this.pvpFeatures && A_PvPClass.instance.enabled)
		{
			A_PvPClass.instance.enabled = false;
		}
	}

	public static A_Master instance;

	public GameObject player0GameObject;

	public bool playerIsAFK;

	public Dictionary<string, string> TranslatedObfuscatedStrings;

	public Camera mainCamera;

	private Color watermarkColor = new Color(1f, 1f, 1f, 0.4f);

	private GUIStyle watermarkStyle = new GUIStyle();

	public static GameObject modsObject;

	public static string modpackVersion = "V36 PvP Client V0.1";

	public bool displayDMGNumbers;

	public bool displayBulletTracers;

	public bool displayFullEXP;

	public bool displayLevelCapEXP;

	public bool pvpFeatures;
}
