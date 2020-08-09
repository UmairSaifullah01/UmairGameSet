using System.IO;
using TMPro;
using UnityEditor;
using UnityEngine;

namespace UMUI
{


	public class UIEditorWindow : EditorWindow
	{

		static string path = "";

		[MenuItem("Window/UI Window %u")]
		static void Init()
		{
			var window = GetWindow<UIEditorWindow>();
			window.titleContent      =  new GUIContent("UI Window");
			path                     =  EditorPrefs.GetString("path");
			SceneView.duringSceneGui += OnSceneGUI;
		}

		void OnGUI()
		{
			EditorGUILayout.LabelField("", path);
			if (GUILayout.Button("Browse"))
			{
				path = EditorUtility.SaveFolderPanel("Save textures to folder", string.IsNullOrEmpty(path) ? "" : path, "");
				EditorPrefs.SetString("path", path);
			}

			if (string.IsNullOrEmpty(path))
			{
				return;
			}

			CreatePanel();
			CreateButton();
			CreateText();
			CreateScript();
			AddMeshCollider();
			ChangeFont();
			NameOrganizer();
		}

		TMP_FontAsset t;

		TextMeshProUGUI[] texts;

		void ChangeFont()
		{
			EditorGUILayout.BeginVertical("box");
			EditorGUILayout.LabelField("Panel");
			t = (TMP_FontAsset) EditorGUILayout.ObjectField("prefab", t, typeof(TMP_FontAsset), true);
			if (GUILayout.Button("Find"))
			{
				texts = FindObjectsOfType<TextMeshProUGUI>();
			}

			if (texts != null && texts.Length > 0)
			{
				if (GUILayout.Button("Add"))
				{
					foreach (TextMeshProUGUI text in texts)
					{
						text.font = t;
					}
				}
			}

			EditorGUILayout.EndVertical();
		}

		string before, after;

		void NameOrganizer()
		{
			EditorGUILayout.BeginVertical("box");
			EditorGUILayout.LabelField("Name Organizer");
			before = EditorGUILayout.TextField("", before);
			after  = EditorGUILayout.TextField("", after);
			if (GUILayout.Button("Organize"))
			{
				var parent = Selection.activeGameObject.transform;
				for (int i = 0; i < parent.childCount; i++)
				{
					parent.GetChild(i).name = $"{before} {i + 1} {after}";
				}
			}

			EditorGUILayout.EndVertical();
		}

		Renderer[] renderers;

		void AddMeshCollider()
		{
			EditorGUILayout.BeginVertical("box");
			EditorGUILayout.LabelField("Panel");
			if (GUILayout.Button("Find"))
			{
				renderers = FindObjectsOfType<Renderer>();
			}

			if (renderers != null && renderers.Length > 0)
			{
				if (GUILayout.Button("Add"))
				{
					foreach (Renderer renderer in renderers)
					{
						Collider collider = renderer.gameObject.GetComponent<Collider>();
						if (collider) continue;
						renderer.gameObject.AddComponent<MeshCollider>();
					}
				}

				if (GUILayout.Button("Remove"))
				{
					foreach (Renderer renderer in renderers)
					{
						Collider collider = renderer.gameObject.GetComponent<Collider>();
						if (collider) DestroyImmediate(collider);
					}
				}
			}

			EditorGUILayout.EndVertical();
		}

		string panelName = "";

		void CreatePanel()
		{
			EditorGUILayout.BeginVertical("box");
			EditorGUILayout.LabelField("Panel");
			panelName = EditorGUILayout.TextField("", panelName);
			if (GUILayout.Button("Create Panel"))
			{
				CreateFile(panelName, "UMUI", "PanelBehaviour", path);
			}

			EditorGUILayout.EndVertical();
		}

		static void OnSceneGUI(SceneView sceneView)
		{
			if (Event.current.type != EventType.KeyDown) return;
			if (Event.current.keyCode == KeyCode.G)
			{
				Ray        worldRay = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
				RaycastHit hitInfo;
				if (Physics.Raycast(worldRay, out hitInfo))
				{
					Event.current.Use();
					Selection.activeGameObject.transform.position = hitInfo.point;
				}
			}
		}

		GameObject obj        = null;
		string     buttonName = "";

		void CreateButton()
		{
			EditorGUILayout.BeginVertical("box");
			EditorGUILayout.LabelField("Button");
			obj        = (GameObject) EditorGUILayout.ObjectField("prefab", obj, typeof(GameObject), true);
			buttonName = EditorGUILayout.TextField("", buttonName);
			if (GUILayout.Button("Create Button"))
			{
				var parent = Selection.activeGameObject;
				var v      = MonoBehaviour.Instantiate(obj, parent.transform);
				v.name = buttonName + "Button";
				var text            = v.GetComponentInChildren<TextMeshProUGUI>();
				if (text) text.text = buttonName;
			}

			EditorGUILayout.EndVertical();
		}

		void CreateFile(string Name, string nameSpace, string parent, string copyPath)
		{
			copyPath += "/" + Name + ".cs";
			if (File.Exists(copyPath))
				File.Delete(copyPath);
			Debug.Log(copyPath);
			using (StreamWriter outfile = new StreamWriter(copyPath))
			{
				char invertedComma = '"';
				outfile.WriteLine("namespace " + nameSpace + " {");
				if (!string.IsNullOrEmpty(parent))
				{
					if (parent == "UBehaviour")
						outfile.WriteLine("[UClassHeader(" + invertedComma + Name + invertedComma + ")]");
					outfile.WriteLine("     public class " + Name + " : " + parent + " {");
				}
				else
					outfile.WriteLine("     public class " + Name + " {");

				outfile.WriteLine("     }");
				outfile.WriteLine("}");
			}

			AssetDatabase.Refresh();
		}


		string text  = "";
		string Title = "";

		void CreateText()
		{
			EditorGUILayout.BeginVertical("box");
			EditorGUILayout.LabelField("Text");
			Title = EditorGUILayout.TextField("", Title);
			text  = EditorGUILayout.TextField("", text);
			if (GUILayout.Button("Create Text"))
			{
				var        parent = Selection.activeGameObject;
				GameObject g      = new GameObject(Title, typeof(TextMeshProUGUI));
				g.transform.SetParent(parent.transform);
				g.GetComponent<TextMeshProUGUI>().text = text;
			}

			EditorGUILayout.EndVertical();
		}

		string classname = "";
		string parent    = "UBehaviour";
		string nameSpace = "UMGS";

		void CreateScript()
		{
			EditorGUILayout.BeginVertical("box");
			EditorGUILayout.LabelField("ClassName");
			classname = EditorGUILayout.TextField("", classname);
			parent    = EditorGUILayout.TextField("", parent);
			if (GUILayout.Button("Create Class"))
			{
				CreateFile(classname, nameSpace, parent, path);
			}

			EditorGUILayout.EndVertical();
		}

	}


}