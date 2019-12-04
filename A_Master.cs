///<summary> NOTE: TO ENABLE THIS CLASS ADD THE FOLLOWING CODE inside Awake() in GlobalPlayerManifest.
///<code>new GameObject("ModManagersGO").AddComponent(typeof(A_Master));</code>
///</summary>

using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

///<summary> Master Class for implementing mods into the standalone client.</summary>
public class A_Master : MonoBehaviour
{
	private void Awake()
	{
    //Ensure our mods always remain singletons, this occurs in every mod class.
		if (A_Master.instance == null)
		{
			A_Master.instance = this;
      //Assign the modsObject we attach each mod component to.
			A_Master.modsObject = base.gameObject;
      //Create a new output log for every client startup.
			this.OutputLogStartup();
			A_Master.instance.WriteToOutputLog("Master Class initialized.");
		}
		else if (A_Master.instance != this)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	///<summary> Start() Method in A_Master is used to initialize the global variables with their saved values in the registry
  ///if these values exist.</summary>
	private void Start()
	{
	this.displayBulletTracers = (this.toggleDisplayBulletTracers = Convert.ToBoolean(PlayerPrefs.GetInt("Mod_TracerToggle", 0)));
		this.displayDMGNumbers = (this.toggleDisplayDMGNumbers = Convert.ToBoolean(PlayerPrefs.GetInt("Mod_DamageNumberToggle", 0)));
		this.displayFullEXP = (this.toggleDisplayFullEXP = Convert.ToBoolean(PlayerPrefs.GetInt("Mod_FullEXP", 0)));
		this.displayLevelCapEXP = (this.toggleDisplayLevelCapEXP = Convert.ToBoolean(PlayerPrefs.GetInt("Mod_LevelCapEXP", 0)));
		this.pvpFeatures = (this.togglepvpFeatures = Convert.ToBoolean(PlayerPrefs.GetInt("Mod_PvPFeatures", 0)));
		this.aggroBar = (this.toggleAggroBar = Convert.ToBoolean(PlayerPrefs.GetInt("Mod_AggroBar", 0)));
		this.colourCloak = (this.toggleColourCloak = Convert.ToBoolean(PlayerPrefs.GetInt("Mod_ColourCloak", 0)));
		this.permaDaytime = (this.togglePermaDaytime = Convert.ToBoolean(PlayerPrefs.GetInt("Mod_PermaDaytime", 0)));
    
    //Add the mod components.
		this.LoadMods();
    //Enable them separately rather than as they are added.
		this.HandleModSettings();
	}

	///<summary> Update() in A_Master is used to loop until the player object is created so we can save a reference to it. (A_Master exists
  before the player) and is constantly checking if the player is AFK. (Mass being a high value)</summary>
	private void Update()
	{
		this.GetReferenceToPlayerGameObject();
		this.CheckPlayerAFKState();
	}

  ///<summary> OnGUI() in A_Master is used to display basic UI elements such as the watermark as well as the modified settings menu
  ///if it is open.</summary>
	private void OnGUI()
	{
    //Watermark display
		this.watermarkStyle.normal.textColor = this.watermarkColor;
		this.watermarkStyle.fontSize = 12;
		GUI.skin = DFHUD.mainSkin;
		GUI.Label(new Rect((float)Screen.width - 130f, (float)Screen.height - 17f, 300f, 50f), "V36 PvP Client V0.3", this.watermarkStyle);
		
    /*This is a check to see if the settings menu is open, if it is we display the mod settings. We use a system of temporary toggles here
    so that the mods don't actually enable until the user presses "Done" or exits the settings menu for safe enabling/disabling.*/
    if (DFHUD.DF34_16e3b6190b4a022282fee58e9f6162bbed6a1bb5 == 1)
		{
			GUI.Box(new Rect((float)(Screen.width / 2 + 150), (float)(Screen.height / 2 - 155), 220f, 280f), "Mod Settings");
			GUI.Label(new Rect((float)(Screen.width / 2 + 170), (float)(Screen.height / 2 - 155 + 20 + 15), 200f, 40f), "DMG Numbers:");
			this.toggleDisplayDMGNumbers = GUI.Toggle(new Rect((float)(Screen.width / 2 + 320), (float)(Screen.height / 2 - 155 + 20 + 10), 20f, 20f), this.toggleDisplayDMGNumbers, string.Empty);
			GUI.Label(new Rect((float)(Screen.width / 2 + 170), (float)(Screen.height / 2 - 155 + 20 + 45), 200f, 40f), "Bullet Tracers:");
			this.toggleDisplayBulletTracers = GUI.Toggle(new Rect((float)(Screen.width / 2 + 320), (float)(Screen.height / 2 - 155 + 20 + 40), 20f, 20f), this.toggleDisplayBulletTracers, string.Empty);
			GUI.Label(new Rect((float)(Screen.width / 2 + 170), (float)(Screen.height / 2 - 155 + 20 + 75), 200f, 40f), "Show Full EXP:");
			this.toggleDisplayFullEXP = GUI.Toggle(new Rect((float)(Screen.width / 2 + 320), (float)(Screen.height / 2 - 155 + 20 + 70), 20f, 20f), this.toggleDisplayFullEXP, string.Empty);
			GUI.Label(new Rect((float)(Screen.width / 2 + 170), (float)(Screen.height / 2 - 155 + 20 + 105), 200f, 40f), "Show LvL Cap EXP:");
			this.toggleDisplayLevelCapEXP = GUI.Toggle(new Rect((float)(Screen.width / 2 + 320), (float)(Screen.height / 2 - 155 + 20 + 100), 20f, 20f), this.toggleDisplayLevelCapEXP, string.Empty);
			GUI.Label(new Rect((float)(Screen.width / 2 + 170), (float)(Screen.height / 2 - 155 + 20 + 135), 200f, 40f), "PvP Features:");
			this.togglepvpFeatures = GUI.Toggle(new Rect((float)(Screen.width / 2 + 320), (float)(Screen.height / 2 - 155 + 20 + 130), 20f, 20f), this.togglepvpFeatures, string.Empty);
			GUI.Label(new Rect((float)(Screen.width / 2 + 170), (float)(Screen.height / 2 - 155 + 20 + 165), 200f, 40f), "Aggro Warning:");
			this.toggleAggroBar = GUI.Toggle(new Rect((float)(Screen.width / 2 + 320), (float)(Screen.height / 2 - 155 + 20 + 160), 20f, 20f), this.toggleAggroBar, string.Empty);
			GUI.Label(new Rect((float)(Screen.width / 2 + 170), (float)(Screen.height / 2 - 155 + 20 + 195), 200f, 40f), "Colour Cloak:");
			this.toggleColourCloak = GUI.Toggle(new Rect((float)(Screen.width / 2 + 320), (float)(Screen.height / 2 - 155 + 20 + 190), 20f, 20f), this.toggleColourCloak, string.Empty);
			GUI.Label(new Rect((float)(Screen.width / 2 + 170), (float)(Screen.height / 2 - 155 + 20 + 225), 200f, 40f), "Perma Daylight: ");
			this.togglePermaDaytime = GUI.Toggle(new Rect((float)(Screen.width / 2 + 320), (float)(Screen.height / 2 - 155 + 20 + 220), 20f, 20f), this.togglePermaDaytime, string.Empty);
		}
	}

	///<summary>Method used to get the Main Players game object and Camera if we don't have it already, this is usually assigned on startup.
  ///</summary>
	private void GetReferenceToPlayerGameObject()
	{
		if (this.player0GameObject == null)
		{
			this.player0GameObject = GameObject.Find("Player0");
		}
		if (this.mainCamera == null)
		{
			this.mainCamera = (Camera)GameObject.Find("MainCamera").GetComponent(typeof(Camera));
		}
	}

	///<summary>Method used to check if the player is AFK, the game sets the player's mass to 9999 if they are.</summary>
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

	///<summary> Method used to print a string to the output log for debug purposes.</summary>
	public void WriteToOutputLog(string line)
	{
		StreamWriter streamWriter = new StreamWriter(Application.persistentDataPath + "/OutputLog.txt", true);
		streamWriter.WriteLine(DateTime.Now + " - " + line);
		streamWriter.Close();
	}

	///<summary>Method used to create and print the header of the output log when the game is started.</summary>
	public void OutputLogStartup()
	{
		StreamWriter streamWriter = new StreamWriter(Application.persistentDataPath + "/OutputLog.txt", false);
		streamWriter.WriteLine("Dead Frontier Modpack Output Log");
		streamWriter.WriteLine("Unity Version: " + Application.unityVersion);
		streamWriter.WriteLine("Modpack Version: " + modpackVersion);
		streamWriter.WriteLine("--------------------------------");
		streamWriter.Close();
	}

	///<summary>Method that saves the users settings to the registry, this is called from the DFHUD class when the user presses "Done"
  ///or exists the settings menu, we then call HandleModSettings() to enable/disable mods if necessary.</summary>
	public void SaveModSettings()
	{
		A_Master.instance.WriteToOutputLog("Saving Settings to registry.");
    
    //Since we use temporary toggles, we assign the temporary toggles to the real ones when the user is done modifying their settings.
		this.aggroBar = this.toggleAggroBar;
		this.pvpFeatures = this.togglepvpFeatures;
		this.colourCloak = this.toggleColourCloak;
		this.permaDaytime = this.togglePermaDaytime;
		this.displayBulletTracers = this.toggleDisplayBulletTracers;
		this.displayDMGNumbers = this.toggleDisplayDMGNumbers;
		this.displayFullEXP = this.toggleDisplayFullEXP;
		this.displayLevelCapEXP = this.toggleDisplayLevelCapEXP;
    
    //Save to registry.
		PlayerPrefs.SetInt("Mod_TracerToggle", Convert.ToInt32(this.displayBulletTracers));
		PlayerPrefs.SetInt("Mod_DamageNumberToggle", Convert.ToInt32(this.displayDMGNumbers));
		PlayerPrefs.SetInt("Mod_FullEXP", Convert.ToInt32(this.displayFullEXP));
		PlayerPrefs.SetInt("Mod_LevelCapEXP", Convert.ToInt32(this.displayLevelCapEXP));
		PlayerPrefs.SetInt("Mod_PvPFeatures", Convert.ToInt32(this.pvpFeatures));
		PlayerPrefs.SetInt("Mod_AggroBar", Convert.ToInt32(this.aggroBar));
		PlayerPrefs.SetInt("Mod_ColourCloak", Convert.ToInt32(this.colourCloak));
		PlayerPrefs.SetInt("Mod_PermaDaytime", Convert.ToInt32(this.permaDaytime));
    
    //Check which mods need to be enabled or disabled.
		this.HandleModSettings();
	}
  
	///<summary> Method used to add each mod class as a component to the modsObject stored in this class.</summary>
	private void LoadMods()
	{
		A_Master.instance.WriteToOutputLog("Adding mod components.");
		A_Master.modsObject.AddComponent<A_PvPClass>();
		A_Master.modsObject.AddComponent<A_AggroWarning>();
		A_Master.modsObject.AddComponent<A_ColourCloak>();
	}

	///<summary> In the past a major performance problem for the modpack was the many update loops each mod class ran all the time.
  ///Enabling/DIsabling mods like this allows us to disable active mods while keeping them attached to the modsObject for easy toggling.
  ///</summary>
	private void HandleModSettings()
	{
		if (this.pvpFeatures && !A_PvPClass.instance.enabled)
		{
			A_PvPClass.instance.enabled = true;
		}
		if (!this.pvpFeatures && A_PvPClass.instance.enabled)
		{
			A_PvPClass.instance.enabled = false;
		}
		if (this.aggroBar && !A_AggroWarning.instance.enabled)
		{
			A_AggroWarning.instance.enabled = true;
		}
		if (!this.aggroBar && A_AggroWarning.instance.enabled)
		{
			A_AggroWarning.instance.enabled = false;
		}
		if (this.colourCloak && !A_ColourCloak.instance.enabled)
		{
			A_ColourCloak.instance.enabled = true;
		}
		if (!this.colourCloak && A_ColourCloak.instance.enabled)
		{
			A_ColourCloak.instance.enabled = false;
		}
	}

	//Singleton instance of the master class. Assigned by Awake()
	public static A_Master instance;

	//Main Player Game Object. Assigned by GetReferenceToPlayerGameObject()
	public GameObject player0GameObject;

	//Toggles true/false depending on the player mass, assigned by CheckPlayerAFKState()
	public bool playerIsAFK;

  //Reference to the main camera, assigned by GetReferenceToPlayerGameObject()
	public Camera mainCamera;

  //Watermark stuff.
	private Color watermarkColor = new Color(1f, 1f, 1f, 0.4f);
	private GUIStyle watermarkStyle = new GUIStyle();

	// GameObject containing all the mod classes. Assigned by Awake()
	public static GameObject modsObject;

	//Watermark String.
	public const string modpackVersion = "V36 PvP Client V0.3";

  //Real Mod toggles.
	public bool displayDMGNumbers;
	public bool displayBulletTracers;
	public bool displayFullEXP;
	public bool displayLevelCapEXP;
	public bool pvpFeatures;
	public bool aggroBar;
	public bool colourCloak;
	public bool permaDaytime;

	//Temporary Mod toggles used in the settings menu.
	public bool toggleDisplayDMGNumbers;
	public bool toggleDisplayBulletTracers;
	public bool toggleDisplayFullEXP;
	public bool toggleDisplayLevelCapEXP;
	public bool toggleAggroBar;
	public bool togglepvpFeatures;
	public bool toggleColourCloak;
	public bool togglePermaDaytime;
}
