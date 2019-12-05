using System;
using System.Collections;
using UnityEngine;

///<summary>Class containing the functionality of a custom blue cloak, coat or jacket.</summary>
[Serializable]
public class A_ColourCloak : MonoBehaviour
{
	public void Awake()
	{
		if (A_ColourCloak.instance == null)
		{
			A_Master.instance.WriteToOutputLog("Rainbow Clothing Mod initialized.");
			base.enabled = false;
			A_ColourCloak.instance = this;
		}
		else if (A_ColourCloak.instance != this)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	///<summary> Method called to update our mesh renderer when clicking "Done" after exiting the inventory.</summary>
	public void UpdateSelectedSkinnedMeshRenderer()
	{
		base.StartCoroutine(this.WaitToUpdate());
	}

  ///<summary> Method used to update the renderer after 1 second.</summary>
	private IEnumerator WaitToUpdate()
	{
		yield return new WaitForSeconds(1f);
		try
		{
			if (base.enabled)
			{
				foreach (Renderer renderer in A_Master.instance.getMainPlayer().GetComponentsInChildren<Renderer>())
				{
					if (renderer.name.ToLower().Contains("cloak") || renderer.name.ToLower().Contains("coat") || renderer.name.ToLower().Contains("jacket"))
					{
						this.meshRenderer = renderer.renderer;
						base.StartCoroutine(this.SetColour());
					}
				}
			}
		}
		catch (Exception ex)
		{
			this.SendError("WaitToUpdate() - " + ex.StackTrace);
		}
		yield return null;
		yield break;
	}

	public void OnDisable()
	{
		A_Master.instance.WriteToOutputLog("Rainbow Clothing Mod has been disabled!");
		base.StopAllCoroutines();
    //Put the original item back on.
		this.SetOriginal();
		this.meshRenderer = null;
	}

	public static bool isEnabledAndNotNull()
	{
		return A_ColourCloak.instance != null && A_ColourCloak.instance.enabled;
	}

	public void OnEnable()
	{
		base.StopAllCoroutines();
		A_Master.instance.WriteToOutputLog("Rainbow Clothing Mod has been enabled!");
    //Set the colour
		base.StartCoroutine(this.SetColour());
	}

	///<summary> Coroutine used to Set the colour, runs until it can do so successfully.</summary>
	private IEnumerator SetColour()
	{
    //Get the player manifest to access the materials.
		if (this.manifest == null)
		{
			this.manifest = (GlobalPlayerManifest)GameObject.Find("GlobalPlayerManifest").GetComponent(typeof(GlobalPlayerManifest));
		}
    //Loop until we have a main player object, if we don't we can't apply a colour.
		while (A_Master.instance.getMainPlayer() == null)
		{
			yield return null;
		}
    //Loop until we can find the correct renderer.
		while (this.meshRenderer == null)
		{
			if (A_Master.instance.getMainPlayer() != null)
			{
				foreach (Renderer renderer in A_Master.instance.getMainPlayer().GetComponentsInChildren<Renderer>())
				{
					if (renderer.name.ToLower().Contains("cloak") || renderer.name.ToLower().Contains("coat") || renderer.name.ToLower().Contains("jacket"))
					{
						this.meshRenderer = renderer.renderer;
						this.originalCoatMaterial = this.meshRenderer.renderer.material;
						this.originalCoatColor = this.meshRenderer.renderer.material.color;
					}
				}
				yield return null;
			}
		}
		try
		{
      //Set the colour.
			string text = this.meshRenderer.name.ToLower();
			if (text.Contains("cloak"))
			{
				this.meshRenderer.material = this.manifest.Cloak_White;
				this.meshRenderer.material.SetColor("_Color", new Color32(26, 0, 160, 200));
			}
			else if (text.Contains("longjacket"))
			{
				this.meshRenderer.renderer.material = this.manifest.LongJacket_White;
				this.meshRenderer.material.shader = Shader.Find("CtrlJ/UnlitTrans");
				this.meshRenderer.material.SetColor("_Color", new Color32(18, 4, 189, byte.MaxValue));
			}
			else if (text.Contains("shortjacket"))
			{
				this.meshRenderer.renderer.material = this.manifest.ShortJacket_White;
				this.meshRenderer.material.shader = Shader.Find("CtrlJ/UnlitTrans");
				this.meshRenderer.material.SetColor("_Color", new Color32(18, 4, 189, byte.MaxValue));
			}
		}
		catch (Exception ex)
		{
			this.SendError("SetColour() - " + ex.StackTrace);
		}
		yield return null;
		yield break;
	}

  ///<summary> Method used to set the original material back onto the player.</summary>
	private void SetOriginal()
	{
		try
		{
			if (this.meshRenderer != null)
			{
				this.meshRenderer.renderer.material = this.originalCoatMaterial;
				this.meshRenderer.renderer.material.color = new Color(this.meshRenderer.renderer.material.color.r, this.meshRenderer.renderer.material.color.g, this.meshRenderer.renderer.material.color.b, 1f);
			}
		}
		catch (Exception ex)
		{
			this.SendError("SetOriginal() - " + ex.StackTrace);
		}
	}
  
	private void SendError(string message)
	{
		A_Master.instance.WriteToOutputLog("ERROR in A_ColourCloak: " + message);
	}

	//Mesh Renderer
	private Renderer meshRenderer;
	private GlobalPlayerManifest manifest;

	// Token: 0x040035C3 RID: 13763
	public static A_ColourCloak instance;

  //Original Materials
	private Material originalCoatMaterial;
	private Color originalCoatColor;
}
