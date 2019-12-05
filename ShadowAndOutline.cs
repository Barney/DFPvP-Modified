//Written by Clayton
using System;
using UnityEngine;

public static class ShadowAndOutline
{
	public static void DrawOutline(Rect rect, string text, GUIStyle style, Color outColor, Color inColor, float size)
	{
		float num = size * 0.5f;
		GUIStyle guistyle = new GUIStyle(style);
		Color color = GUI.color;
		style.normal.textColor = outColor;
		GUI.color = outColor;
		rect.x -= num;
		GUI.Label(rect, text, style);
		rect.x += size;
		GUI.Label(rect, text, style);
		rect.x -= num;
		rect.y -= num;
		GUI.Label(rect, text, style);
		rect.y += size;
		GUI.Label(rect, text, style);
		rect.y -= num;
		style.normal.textColor = inColor;
		GUI.color = color;
		GUI.Label(rect, text, style);
		style = guistyle;
	}

	public static void DrawShadow(Rect rect, GUIContent content, GUIStyle style, Color txtColor, Color shadowColor, Vector2 direction)
	{
		GUIStyle guistyle = style;
		style.normal.textColor = shadowColor;
		rect.x += direction.x;
		rect.y += direction.y;
		GUI.Label(rect, content, style);
		style.normal.textColor = txtColor;
		rect.x -= direction.x;
		rect.y -= direction.y;
		GUI.Label(rect, content, style);
		style = guistyle;
	}
  
	public static void DrawLayoutShadow(GUIContent content, GUIStyle style, Color txtColor, Color shadowColor, Vector2 direction, params GUILayoutOption[] options)
	{
		ShadowAndOutline.DrawShadow(GUILayoutUtility.GetRect(content, style, options), content, style, txtColor, shadowColor, direction);
	}
	public static bool DrawButtonWithShadow(Rect r, GUIContent content, GUIStyle style, float shadowAlpha, Vector2 direction)
	{
		GUIStyle guistyle = new GUIStyle(style);
		guistyle.normal.background = null;
		guistyle.hover.background = null;
		guistyle.active.background = null;
		bool result = GUI.Button(r, content, style);
		Color txtColor = r.Contains(Event.current.mousePosition) ? guistyle.hover.textColor : guistyle.normal.textColor;
		ShadowAndOutline.DrawShadow(r, content, guistyle, txtColor, new Color(0f, 0f, 0f, shadowAlpha), direction);
		return result;
	}

	public static bool DrawLayoutButtonWithShadow(GUIContent content, GUIStyle style, float shadowAlpha, Vector2 direction, params GUILayoutOption[] options)
	{
		return ShadowAndOutline.DrawButtonWithShadow(GUILayoutUtility.GetRect(content, style, options), content, style, shadowAlpha, direction);
	}
}
