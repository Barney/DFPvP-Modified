using System;
using UnityEngine;

public class A_DMGNum : MonoBehaviour
{
	private void OnGUI()
	{
		this.numberStyle.normal.textColor = this.numberColor;
		if (this.damageCritValue >= 1f)
		{
			GUI.skin = DFHUD.hudSkin;
			GUI.depth = 9000;
			ShadowAndOutline.DrawOutline(new Rect(this.numberScreenPosition.x - 50f, (float)Screen.height - this.numberScreenPosition.y - 60f, 100f, 30f), this.damageValue.ToString("F2"), this.numberStyle, new Color(1f, 0.48f, 0f, this.numberColorAlpha), this.numberColor, 2.2f);
			return;
		}
		if (this.damageCritValue < 1f)
		{
			GUI.skin = DFHUD.mainSkin;
			GUI.depth = 9000;
			ShadowAndOutline.DrawOutline(new Rect(this.numberScreenPosition.x - 50f, (float)Screen.height - this.numberScreenPosition.y, 100f, 30f), this.damageValue.ToString("F2"), this.numberStyle, new Color(0.2f, 0.25f, 0.3f, this.numberColorAlpha), this.numberColor, 2.2f);
		}
	}

	private void Update()
	{
		if (this.damageCritValue >= 1f)
		{
			this.critFadeSpeed += Time.deltaTime * 2f;
			this.numberColorAlpha -= Time.deltaTime * this.critFadeSpeed;
			this.numberColor = new Color(1f, 1f, 0f, this.numberColorAlpha);
		}
		else if (this.damageCritValue < 1f)
		{
			this.nonCritFadeSpeed += Time.deltaTime * 2f;
			this.numberColorAlpha -= Time.deltaTime * this.nonCritFadeSpeed;
			this.numberColor = new Color(0.95f, 0.95f, 1f, this.numberColorAlpha);
		}
		if (this.numberColorAlpha <= 0f)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}
  
	public float damageValue;
	public Vector3 numberScreenPosition;
	private GUIStyle numberStyle = new GUIStyle();
	private float numberColorAlpha = 1f;
	private float alphaFadeSpeed = 1f;
	private Color numberColor;
	public float damageCritValue;
	private float nonCritFadeSpeed = 0.35f;
	private float critFadeSpeed = 0.1f;
}
