using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using System.IO;

public class ScenesView : EditorWindow
{
    Texture2D trashIcon;
    Texture2D pencilIcon;

    void OnEnable()
    {
        trashIcon = EditorGUIUtility.IconContent("TreeEditor.Trash").image as Texture2D;
        pencilIcon = EditorGUIUtility.IconContent("EditCollider").image as Texture2D;
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
            menu.AddItem(new GUIContent("Create New Scene"), false, CreateScene);
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

            EditorGUILayout.BeginHorizontal();

            // Scene Name
            GUILayout.Label(name, GUILayout.Width(position.width / 2));

            // Open Scene Button
            if (GUILayout.Button("Open", GUILayout.Width(60)))
            {
                if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                {
                    EditorSceneManager.OpenScene(path);
                }
            }

            GUILayoutOption[] dimensions = { GUILayout.Width(22), GUILayout.Height(18) };

            // Rename Scene Button
            if (GUILayout.Button(new GUIContent(pencilIcon), dimensions))
            {
                RenameScene(path);
            }

            // Delete Scene Button
            if (GUILayout.Button(new GUIContent(trashIcon), dimensions))
            {
                DeleteScene(path);
            }

            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.EndScrollView();
    }

    void CreateScene()
    {
        var path = "Assets/Scenes/MyScene.unity";
        var newScene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects);
        EditorSceneManager.SaveScene(newScene, path);
        EditorSceneManager.OpenScene(path);
    }

    void RenameScene(string path)
    {
        
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
}