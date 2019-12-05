//Written by Clayton
using System;
using UnityEngine;

public class A_Tracer : MonoBehaviour
{
	private void Awake()
	{
		this.lineRenderer = base.gameObject.AddComponent<LineRenderer>();
		this.lineRenderer.receiveShadows = false;
		this.lineRenderer.useWorldSpace = true;
		this.lineRenderer.material = new Material(Shader.Find("Particles/Alpha Blended"));
	}

	private void Update()
	{
		this.lineRenderer.SetWidth(0.03f, 0.055f);
		this.lineRenderer.SetColors(new Color(0.5f, 0.5f, 0.5f, 0f), new Color(0.45f, 0.45f, 0.4f, this.alpha));
		if (this.alpha > 0f)
		{
			this.alpha -= Time.deltaTime * this.fadeSpeed;
			return;
		}
		UnityEngine.Object.Destroy(base.gameObject);
	}

	public void CreateProjectileLine(Vector3 origin, Vector3 direction, float distance)
	{
		this.lineRenderer.SetPosition(0, origin);
		this.lineRenderer.SetPosition(1, origin + distance * direction);
		this.alpha = 0.7f;
	}

	private LineRenderer lineRenderer;
	public Color startingColor = Color.gray;
	public Color endingColor = Color.gray;
	public float alpha = 1f;
	private float fadeSpeed = 10f;
}
