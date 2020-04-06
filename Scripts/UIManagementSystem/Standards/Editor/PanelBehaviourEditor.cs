using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[CustomEditor(typeof(PanelBehaviour), true)]
public class PanelBehaviourEditor : Editor
{
    private PanelBehaviour handler;
    
    private void OnEnable()
    {
        handler = (PanelBehaviour) target;
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.Space();
        if (GUILayout.Button("Find Buttons"))
        {
            handler.Buttons = handler.gameObject.GetComponentsInChildren<Button>();
        }
        
        DrawDefaultInspector();
    }
}