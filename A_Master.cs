using System;
using System.IO;
using UnityEngine;

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
		}
		else if (A_Master.instance != this)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	private void Start()
	{
		this.LoadMods();
	}

	private void Update()
	{
		this.GetReferenceToPlayerGameObject();
		this.CheckPlayerAFKState();
	}

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

	private void CheckPlayerAFKState()
	{
		if (this.player0GameObject != null)
		{
			this.playerIsAFK = false;
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

	public void WriteToOutputLog(string line)
	{
		StreamWriter streamWriter = new StreamWriter(Application.persistentDataPath + "/OutputLog.txt", true);
		streamWriter.WriteLine(line);
		streamWriter.Close();
	}

	private void OutputLogStartup()
	{
		StreamWriter streamWriter = new StreamWriter(Application.persistentDataPath + "/OutputLog.txt", false);
		streamWriter.WriteLine("Dead Frontier Modpack Output Log");
		streamWriter.WriteLine("Unity Version: " + Application.unityVersion);
		streamWriter.WriteLine("Modpack Version: " + this.modpackVersion);
		streamWriter.WriteLine("--------------------------------");
		streamWriter.Close();
	}

	public A_Master()
	{
		this.playerIsAFK = true;
		this.watermarkColor = new Color(1f, 1f, 1f, 0.4f);
		this.watermarkStyle = new GUIStyle();
		this.modpackVersion = "V51 PvP Client V0.5";
	}

	public void SaveModSettings()
	{
		A_Master.instance.WriteToOutputLog("Saving Settings to registry.");
	}

	private void LoadMods()
	{
		A_Master.instance.WriteToOutputLog("Adding mod components.");
		A_Master.modsObject.AddComponent<A_PvPClass>();
	}

	public Camera getMainCamera()
	{
		return this.mainCamera;
	}

	public GameObject getMainPlayer()
	{
		return this.player0GameObject;
	}

	public bool isPlayerAFK()
	{
		return this.playerIsAFK;
	}


	public static A_Master instance;

	private GameObject player0GameObject;

	private bool playerIsAFK;

	private Camera mainCamera;

	private Color watermarkColor;

	private GUIStyle watermarkStyle;

	private static GameObject modsObject;

	private string modpackVersion;


	public static Texture2D barTexture;

	public static Color RED = new Color(0.823f, 0.012f, 0f, 1f);

	public static Color ORANGE = new Color(1f, 0.282f, 0f, 1f);

	public static Color YELLOW = new Color(1f, 0.8f, 0f, 1f);

	public static Color GREEN = new Color(0.07f, 1f, 0f, 1f);

}
