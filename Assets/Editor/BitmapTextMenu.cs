using UnityEditor;
using UnityEngine;

public class BitmapTextMenu
{
    [MenuItem("GameObject/UI/Bitmap Text", false, 2000)]
    static void CreateBitmapText(MenuCommand menuCommand)
    {
        var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(
            "Assets/Objects/Prefabs/UI/BitmapText.prefab"
        );

        if (prefab == null)
        {
            Debug.LogError("BitmapText prefab not found");
            return;
        }

        GameObject obj = (GameObject)PrefabUtility.InstantiatePrefab(prefab);

        GameObjectUtility.SetParentAndAlign(obj, menuCommand.context as GameObject);
        Undo.RegisterCreatedObjectUndo(obj, "Create Bitmap Text");
        Selection.activeObject = obj;
    }
}
