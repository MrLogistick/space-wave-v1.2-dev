using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using System.IO;

public class ScenesView : EditorWindow
{
    Texture2D trashIcon;
    Texture2D pencilIcon;

    bool simple = false;

    void OnEnable()
    {
        trashIcon = EditorGUIUtility.IconContent("TreeEditor.Trash").image as Texture2D;
        pencilIcon = EditorGUIUtility.IconContent("editicon.sml").image as Texture2D;
    }
    
    [MenuItem("Window/Scenes View")]
    public static void ShowWindow()
    {
        GetWindow<ScenesView>("Scenes");
    }

    Vector2 scrollPos;

    void OnGUI()
    {
        // Right Click Context Menu
        Event ev = Event.current;
        if (ev.type == EventType.ContextClick)
        {
            GenericMenu menu = new GenericMenu();
            
            menu.AddItem(new GUIContent("Simple Layout"), simple, () => { simple = !simple; } );
            menu.AddItem(new GUIContent("Create New Scene"), false, CreateScene);
            menu.AddItem(new GUIContent("Ping Scenes Folder"), false, PingScenesFolder);

            menu.ShowAsContext();
            ev.Use();
        }

        // Scrolling & Scene IDs
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
        string[] sceneGUIDs = AssetDatabase.FindAssets("t:Scene", new[] { "Assets/Scenes" });

        foreach (string guid in sceneGUIDs)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            string name = Path.GetFileNameWithoutExtension(path);           

            if (simple)
            {
                if (GUILayout.Button(name, GUILayout.Width(position.width * 0.98f), GUILayout.Height(18)))
                {
                    if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                    {
                        EditorSceneManager.OpenScene(path);
                    }
                }
            }
            else
            {
                EditorGUILayout.Space(2);

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.Space(0);

                // Scene Name
                GUILayout.Label(name, GUILayout.Width(position.width * 0.45f), GUILayout.Height(18));

                // Open Scene Button
                if (GUILayout.Button("Open", GUILayout.Width(position.width * 0.25f), GUILayout.Height(18)))
                {
                    if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                    {
                        EditorSceneManager.OpenScene(path);
                    }
                }

                // Rename Scene Button
                if (GUILayout.Button(new GUIContent(pencilIcon), GUILayout.Width(position.width * 0.1f), GUILayout.Height(18)))
                {
                    RenameScene(path);
                }

                // Delete Scene Button
                if (GUILayout.Button(new GUIContent(trashIcon), GUILayout.Width(position.width * 0.1f), GUILayout.Height(18)))
                {
                    DeleteScene(path);
                }

                EditorGUILayout.EndHorizontal();
            }
        }

        EditorGUILayout.EndScrollView();
    }

    void CreateScene()
    {
        string name = EditorUtility.SaveFilePanel(
            "Create New Scene",
            Application.dataPath + "/Scenes",
            "NewScene",
            "unity"
        );

        if (!string.IsNullOrEmpty(name))
        {
            var newScene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects);
            EditorSceneManager.SaveScene(newScene, name);
            EditorSceneManager.OpenScene(name);            
        }

        AssetDatabase.Refresh();
    }

    void RenameScene(string path)
    {
        string currentName = Path.GetFileNameWithoutExtension(path);

        string newName = EditorUtility.SaveFilePanel(
            "Rename Scene",
            Path.GetDirectoryName(path),
            currentName,
            "unity"
        );

        if (string.IsNullOrEmpty(newName)) return;

        if (newName.StartsWith(Application.dataPath)) {
            newName = "Assets" + newName.Substring(Application.dataPath.Length);
        }

        AssetDatabase.MoveAsset(path, newName);
        AssetDatabase.SaveAssets();
        Repaint();
    }

    void DeleteScene(string path)
    {
        if (EditorUtility.DisplayDialog(
            "Delete Scene",
            $"Are you sure you want to delete '{path}'?",
            "Delete",
            "Cancel" ))
        {
            if (AssetDatabase.DeleteAsset(path)) {
                AssetDatabase.Refresh();
            } else {
                EditorUtility.DisplayDialog("Error", "Failed to delete scene.", "OK");
            }
        }
    }

    void PingScenesFolder()
    {
        EditorUtility.FocusProjectWindow();
        var folder = AssetDatabase.LoadAssetAtPath<Object>("Assets/Scenes");
        Selection.activeObject = folder;
        EditorGUIUtility.PingObject(folder);        
    }
}