using System;
using UnityEditor;
using UnityEngine;


namespace UMGS.DoTween
{


	// [CustomPropertyDrawer(typeof(Anim))]
	// public class AnimEditor : PropertyDrawer
	// {
	//
	// 	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	// 	{
	// 		EditorGUI.DefaultPropertyField(position, property, label);
	// 		EditorGUI.BeginProperty(position, label, property);
	// 		var target = property.FindPropertyRelative("target");
	// 		EditorGUILayout.ObjectField(target, new GUIContent("abc"));
	// 		EditorGUILayout.BeginVertical("box");
	// 		EditorGUILayout.LabelField("Movement");
	// 		EditorGUILayout.BeginHorizontal("box");
	// 		var toPosition = property.FindPropertyRelative("toPosition");
	// 		toPosition.vector3Value = EditorGUILayout.Vector3Field("To", toPosition.vector3Value);
	// 		if (GUILayout.Button("Set"))
	// 		{
	// 			var t = (Transform) target.objectReferenceValue;
	// 			toPosition.vector3Value = t.position;
	// 		}
	//
	// 		EditorGUILayout.EndHorizontal();
	// 		EditorGUILayout.EndVertical();
	// 		EditorGUI.EndProperty();
	// 	}

	// }


}