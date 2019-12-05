using System;
using UnityEngine;

///<summary> Class containing the functionality of the Aggro Warning bar.</summary>
public class A_AggroWarning : MonoBehaviour
{
	private void Awake()
	{
		if (A_AggroWarning.instance == null)
		{
			A_Master.instance.WriteToOutputLog("Aggro Warning Mod initialized.");
			base.enabled = false;
			A_AggroWarning.instance = this;
		}
		else if (A_AggroWarning.instance != this)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}
  
  ///<summary> OnGUI() in A_AggroWarning displays a bar representing an aggro spike at the top of the screen.</summary>
	private void OnGUI()
	{
		try
		{
      //If we have a rushSize greater than 0 which means we have an aggro spike, display the bar.
			if (DF34_68ac6f88d0583a466fc24ef7af066ef0e96db03d.rushSize > 0)
			{
				float num = 0f;
				if (Screen.width < 1280)
				{
					num = 50f;
				}
				GUI.Box(new Rect((float)Screen.width / 2f - 75f, 38f + num, 150f, 7f), "");
				this.DetermineAggroBarColor(this.aggroWarningBarLength);
				GUI.DrawTexture(new Rect((float)Screen.width / 2f - 75f, 38f + num, this.aggroWarningBarLength * 1.5f, 7f), A_Master.barTexture);
				GUI.skin = DFHUD.hudSkin;
				GUI.color = A_Master.RED;
				GUI.Label(new Rect((float)Screen.width / 2f - 54f, 15f + num, 120f, 30f), "Aggro Warning");
				string text = "Zombies Remaining: " + DF34_68ac6f88d0583a466fc24ef7af066ef0e96db03d.rushSize.ToString();
				GUI.Label(new Rect((float)Screen.width / 2f - this.CenterRemainingCount(text), 50f + num, 180f, 30f), text);
				GUI.color = Color.white;
			}
		}
		catch (Exception ex)
		{
			this.SendError(ex.StackTrace);
		}
	}
	private void OnEnable()
	{
		A_Master.instance.WriteToOutputLog("Aggro Warning Mod has been enabled!");
	}

	private void OnDisable()
	{
		A_Master.instance.WriteToOutputLog("Aggro Warning Mod has been disabled!");
	}
	///<summary> Update() in A_AggroWarning updates the amount of zombies remaining on the bar.</summary>
	private void Update()
	{
		this.aggroWarningBarLength = (float)DF34_68ac6f88d0583a466fc24ef7af066ef0e96db03d.rushSize / (float)this.maxAggro * 100f;
	}

	///<summary> Method used to determine if the mod component is created and enabled by the user.</summary>
	public static bool isEnabledAndNotNull()
	{
		return A_AggroWarning.instance != null && A_AggroWarning.instance.enabled;
	}

  ///<summary> Method used to determine the colour of the aggro bar based on the amount of enemies remaining.</summary>
	private void DetermineAggroBarColor(float valuePercentage)
	{
		if (valuePercentage >= 75f)
		{
			GUI.color = A_Master.RED;
		}
		if (valuePercentage < 75f)
		{
			GUI.color = A_Master.RED;
		}
		if (valuePercentage < 50f)
		{
			GUI.color = A_Master.YELLOW;
		}
		if (valuePercentage < 25f)
		{
			GUI.color = A_Master.GREEN;
		}
	}

	///<summary> Method used to center the aggro bar depending on its length.</summary>
	private float CenterRemainingCount(string label)
	{
		return (float)label.Length * 3.5f;
	}

	private void SendError(string message)
	{
		A_Master.instance.WriteToOutputLog("ERROR in A_AggroWarning: " + message);
	}

	//Position of the aggro bar.
	private Vector2 pos = new Vector2((float)Screen.width / 2f - 125f, (float)Screen.height / 2f - 25f);

	//Size of the aggro bar.
	private Vector2 size = new Vector2(100f, 20f);

	//Aggro spikes can only have a max of 150.
	private int maxAggro = 150;

	// Length in percentage of the aggro bar.
	private float aggroWarningBarLength;
  
	public static A_AggroWarning instance;
}
