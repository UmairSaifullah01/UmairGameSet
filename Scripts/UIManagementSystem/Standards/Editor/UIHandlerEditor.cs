using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UIHandler), true)]
public class UIHandlerEditor : Editor
{

	private UIHandler handler;

	private void OnEnable()
	{
		handler = (UIHandler) target;
	}

	public override void OnInspectorGUI()
	{
		EditorGUILayout.Space();
		if (GUILayout.Button("Find Panels"))
		{
			handler.panels = FindObjectsOfType<PanelBehaviour>();
			foreach (PanelBehaviour behaviour in handler.panels)
			{
				behaviour.handler = handler;
			}
		}

		EditorGUILayout.HelpBox(new GUIContent("Panels Count:" + (handler.panels == null ? "" : handler.panels.Length.ToString())));
		DrawDefaultInspector();
	}

}