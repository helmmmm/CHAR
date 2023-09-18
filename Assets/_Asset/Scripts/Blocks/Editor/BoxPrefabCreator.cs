using UnityEngine;
using UnityEditor;

public class CreateBoxPrefabEditor : EditorWindow
{
    private Vector3 cubePosition = Vector3.zero; // default position

    [MenuItem("Tools/Create Box Prefab Editor")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(CreateBoxPrefabEditor));
    }

    void OnGUI()
    {
        GUILayout.Label("Create Box Prefab at Position", EditorStyles.boldLabel);

        // Draw position fields
        cubePosition = EditorGUILayout.Vector3Field("Cube Position", cubePosition);

        // Get desitination path
        

        // Create prefab button
        if (GUILayout.Button("Create Prefab"))
        {
            CreatePrefab();
        }
    }

    public void CreatePrefab()
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.localPosition = cubePosition;
        string path = "Assets/MyBoxPrefab.prefab";

        // Check if the Prefab exists at the path
        if (AssetDatabase.LoadAssetAtPath(path, typeof(GameObject)))
        {
            if (EditorUtility.DisplayDialog("Are you sure?", "The prefab already exists. Do you want to overwrite it?", "Yes", "No"))
            {
                CreateNewPrefab(cube, path);
            }
        }
        else
        {
            CreateNewPrefab(cube, path);
        }
    }

    private static void CreateNewPrefab(GameObject obj, string path)
    {
        Object prefab = PrefabUtility.SaveAsPrefabAsset(obj, path);
        DestroyImmediate(obj);

        if (prefab == null)
        {
            Debug.LogError("Prefab could not be created.");
        }
        else
        {
            Debug.Log("Prefab created at " + path);
        }
    }
}

