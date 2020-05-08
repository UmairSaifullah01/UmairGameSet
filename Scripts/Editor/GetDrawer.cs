using UnityEditor;
using UnityEngine;


namespace UMGS
{


	[CustomPropertyDrawer(typeof(GetAttribute))]
	public class GetDrawer : PropertyDrawer
	{

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			
			base.OnGUI(position, property, label);
			GetAttribute get=attribute as GetAttribute;
			
		}

	}


}