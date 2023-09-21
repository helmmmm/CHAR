using System.Collections.Generic;
using Niantic.LightshipHub;
using UnityEditor;
using UnityEngine;

public class AssetMaker : EditorWindow
{
    private static bool _axisEnabled = false;
    private string assetName = "";
    private string savePath = "";
    private List<GameObject> gameObjects = new List<GameObject>();
    // private List<bool> toggles = new List<bool>();
    private int selectedObjectIndex = -1; 
    // GameObject prefabToLoad;

    // int fillWidth = 1;
    // int fillDepth = 1;
    // int fillHeight = 1;

    private GameObject assetHolder;
    private GameObject selectedBlock;  // You will set this based on which radio button is checked
    private const float blockUnitSize = 0.1f;
    private enum GridSize
    {
        Size_3x3 = 3,
        Size_5x5 = 5,
        Size_7x7 = 7,
        Size_9x9 = 9,
        Size_11x11 = 11
    }
    private GridSize currentGridSize = GridSize.Size_5x5;

    [MenuItem("Tools/Voxel Asset Maker")]
    public static void ShowWindow()
    {
        GetWindow<AssetMaker>("Voxel Asset maker");
    }

    private void OnGUI()
    {
        GUILayout.Space(40);
        GUILayout.Label("Voxel Asset Maker", EditorStyles.boldLabel);
        currentGridSize = (GridSize)EditorGUILayout.EnumPopup("Grid Size:", currentGridSize);
    
        if (GUILayout.Button("Enable 3D Axis"))
        {
            EnableAxis();
        }

        if (GUILayout.Button("Disable 3D Axis"))
        {
            DisableAxis();
        }

        // GameObject Slots
        GUILayout.Space(20);
        GUILayout.Label("Blocks", EditorStyles.boldLabel);

        for (int i = 0; i < gameObjects.Count; i++)
        {
            GUILayout.Space(2);
            EditorGUILayout.BeginHorizontal();
            gameObjects[i] = (GameObject)EditorGUILayout.ObjectField(gameObjects[i], typeof(GameObject), true);
            // bool current = EditorGUILayout.Toggle(i == selectedObjectIndex);
            bool isSelected = GUILayout.Toggle((selectedBlock == gameObjects[i]), "Select");
            // string objectName = gameObjects[i] == null ? "No name" : gameObjects[i].name;

            if (isSelected)
            {
                selectedBlock = gameObjects[i];
            }

            if (GUILayout.Button("Remove"))
            {
                if (selectedBlock == gameObjects[i])
                {
                    selectedBlock = null; // Deselect if this block was selected
                }
                gameObjects.RemoveAt(i);
            }

            EditorGUILayout.EndHorizontal();
        }

        if (GUILayout.Button("Add Block"))
        {
            gameObjects.Add(null);
        }

        // Fill Grid
        // GUILayout.Space(20);
        // EditorGUILayout.LabelField("Fill Dimensions", EditorStyles.boldLabel);
        // fillWidth = EditorGUILayout.IntField("Width:", fillWidth);
        // fillDepth = EditorGUILayout.IntField("Depth:", fillDepth);
        // fillHeight = EditorGUILayout.IntField("Height:", fillHeight);

        // if (GUILayout.Button("Fill"))
        // {
        //     FillGrid();
        // }

        GUILayout.Space(20);
        GUILayout.Label("Asset Creation", EditorStyles.boldLabel);
        assetName = EditorGUILayout.TextField("Asset Name:", assetName);
        savePath = EditorGUILayout.TextField("Save Path:", savePath);

        // GUILayout.Space(60);
        // GUILayout.Label("Load Prefab", EditorStyles.boldLabel);
        // prefabToLoad = (GameObject)EditorGUILayout.ObjectField("Prefab:", prefabToLoad, typeof(GameObject), false);
        // if (GUILayout.Button("Load Prefab"))
        // {
        //     LoadPrefabIntoGrid();
        // }

        GUILayout.Space(60);
        if (GUILayout.Button("Save As Prefab"))
        {
            SaveAsset();
        }
    }

    private void SaveAsset()
    {
        // Check if assetHolder is empty or not
        if (assetHolder == null || assetHolder.transform.childCount == 0)
        {
            Debug.LogError("AssetHolder is empty. Nothing to save.");
            return;
        }

        // Check if the save path and asset name are valid
        if (string.IsNullOrEmpty(savePath) || string.IsNullOrEmpty(assetName))
        {
            Debug.LogError("Invalid save path or asset name.");
            return;
        }

        // Generate the prefab path
        string prefabPath = savePath + "/" + assetName + ".prefab";

        // Check if a prefab already exists at the specified path
        if (AssetDatabase.LoadAssetAtPath(prefabPath, typeof(GameObject)))
        {
            // Ask if you want to overwrite the existing prefab
            if (EditorUtility.DisplayDialog("Are you sure?", "The prefab already exists. Do you want to overwrite it?", "Yes", "No"))
            {
                // Replace the existing prefab
                PrefabUtility.SaveAsPrefabAsset(assetHolder, prefabPath);
                Debug.Log("Prefab overwritten and saved at " + prefabPath);
            }
        }
        else
        {
            // Create a new prefab
            PrefabUtility.SaveAsPrefabAsset(assetHolder, prefabPath);
            Debug.Log("New prefab saved at " + prefabPath);
        }
    }

    void EnableAxis()
    {
        if (!_axisEnabled)
        {
            assetHolder = new GameObject("AssetHolder");
            SceneView.duringSceneGui += OnSceneGUI;  // Subscribe to the duringSceneGui event
            _axisEnabled = true;
        }

        if (SceneView.lastActiveSceneView != null)
        {
            SceneView.lastActiveSceneView.LookAt(assetHolder.transform.position);
            SceneView.lastActiveSceneView.orthographic = false; // Set to perspective view
            SceneView.lastActiveSceneView.size = 0.6f; // Zoom size
            SceneView.lastActiveSceneView.rotation = Quaternion.Euler(30, 210, 0); // Rotation for a better view
        }
    }

    void DisableAxis()
    {
        if (_axisEnabled)
        {
            if (assetHolder != null)
            {
                DestroyImmediate(assetHolder);
            }
            SceneView.duringSceneGui -= OnSceneGUI;  // Unsubscribe from the duringSceneGui event
            _axisEnabled = false;
        }
    }

    // void FillGrid()
    // {
    //     for (int x = 0; x < fillWidth; x++)
    //     {
    //         for (int y = 0; y < fillDepth; y++)
    //         {
    //             for (int z = 0; z < fillHeight; z++)
    //             {
    //                 InstantiatePrefabAtGridPosition(x, y, z);
    //             }
    //         }
    //     }
    // }

    // void LoadPrefabIntoGrid()
    // {
    //     if (prefabToLoad == null)
    //     {
    //         Debug.LogWarning("No prefab selected.");
    //         return;
    //     }

    //     // Calculate the middle position of the grid.
    //     // Assuming gridWidth and gridHeight represent the dimensions of the grid.
    //     float middleX = (float)currentGridSize * 0.1f;
    //     float middleY = (float)currentGridSize * 0.1f;

    //     // Calculate the world position for the middle of the grid.
    //     // Assuming each cell has a size of cellSize and you have a method to convert grid coordinates to world position.
    //     Vector3 middlePosition = GridToWorldPosition(middleX, middleY);

    //     // Instantiate prefab at the calculated position.
    //     GameObject instance = PrefabUtility.InstantiatePrefab(prefabToLoad) as GameObject;
    //     instance.transform.position = middlePosition;

    //     GameObject assetHolder = GameObject.Find("AssetHolder");

    //     // Set the parent of the instance to be the AssetHolder.
    //     if (assetHolder != null)
    //     {
    //         instance.transform.SetParent(assetHolder.transform);
    //     }
    //     else
    //     {
    //         Debug.LogWarning("AssetHolder not found.");
    //     }
    // }

    // Vector3 GridToWorldPosition(float x, float y)
    // {
    //     // Assuming the bottom-left corner of the grid starts at (0, 0, 0)
    //     // and cellSize is the size of each cell.
    //     float worldX = x * 0.1f;
    //     float worldY = y * 0.1f;

    //     return new Vector3(worldX, worldY, 0);
    // }

    // void SavePrefabChanges()
    // {
    //     // Replace 'instance' with the actual GameObject instance you are working on.
    //     if (instance != null)
    //     {
    //         PrefabUtility.SaveAsPrefabAsset(instance, AssetDatabase.GetAssetPath(prefab));
    //     }
    // }

    void OnSceneGUI(SceneView sceneView)
    {
        float gridSize = (float)currentGridSize / 10;
        float step = 0.1f;
        Color gridColor = new Color(0.5f, 0.5f, 0.5f, 0.3f);

        Handles.color = gridColor;
        for (float i = 0; i <= gridSize; i += step)
        {
            Handles.DrawLine(new Vector3(i, 0, 0), new Vector3(i, gridSize, 0));
            Handles.DrawLine(new Vector3(0, i, 0), new Vector3(gridSize, i, 0));
        }

        // Draw grid for XZ Plane
        for (float i = 0; i <= gridSize; i += step)
        {
            Handles.DrawLine(new Vector3(i, 0, 0), new Vector3(i, 0, gridSize));
            Handles.DrawLine(new Vector3(0, 0, i), new Vector3(gridSize, 0, i));
        }

        // Draw grid for YZ Plane
        for (float i = 0; i <= gridSize; i += step)
        {
            Handles.DrawLine(new Vector3(0, i, 0), new Vector3(0, i, gridSize));
            Handles.DrawLine(new Vector3(0, 0, i), new Vector3(0, gridSize, i));
        }

        // Draw X-axis (Red)
        Handles.color = Color.red;
        Handles.DrawLine(Vector3.zero, Vector3.right * gridSize);

        // Draw Y-axis (Green)
        Handles.color = Color.green;
        Handles.DrawLine(Vector3.zero, Vector3.up * gridSize);

        // Draw Z-axis (Blue)
        Handles.color = Color.blue;
        Handles.DrawLine(Vector3.zero, Vector3.forward * gridSize);

        // Draw XY Plane (Red-Green)
        Handles.color = new Color(1, 0, 0, 0.2f);
        Vector3[] xy = { new Vector3(0, 0, 0), new Vector3(gridSize, 0, 0), new Vector3(gridSize, gridSize, 0), new Vector3(0, gridSize, 0) };
        Handles.DrawSolidRectangleWithOutline(xy, new Color(1, 0, 0, 0.2f), Color.clear);

        // Draw XZ Plane (Red-Blue)
        Handles.color = new Color(0, 0, 1, 0.2f);
        Vector3[] xz = { new Vector3(0, 0, 0), new Vector3(gridSize, 0, 0), new Vector3(gridSize, 0, gridSize), new Vector3(0, 0, gridSize) };
        Handles.DrawSolidRectangleWithOutline(xz, new Color(0, 0, 1, 0.2f), Color.clear);

        // Draw YZ Plane (Green-Blue)
        Handles.color = new Color(0, 1, 0, 0.2f);
        Vector3[] yz = { new Vector3(0, 0, 0), new Vector3(0, gridSize, 0), new Vector3(0, gridSize, gridSize), new Vector3(0, 0, gridSize) };
        Handles.DrawSolidRectangleWithOutline(yz, new Color(0, 1, 0, 0.2f), Color.clear);

        
        Vector3 gridVolume = new Vector3(gridSize, gridSize, gridSize);

        // Initialize assetHolder if null
        if (assetHolder == null)
        {
            assetHolder = new GameObject("AssetHolder");
        }

        Plane xyPlane = new Plane(Vector3.forward, Vector3.zero);
        Plane xzPlane = new Plane(Vector3.up, Vector3.zero);
        Plane yzPlane = new Plane(Vector3.right, Vector3.zero);

        if (Event.current.type == EventType.MouseDown && Event.current.button == 0) // Left Mouse Button
        {
            Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
            RaycastHit hitInfo;
            float enter;

            if (Event.current.alt) // Check if Option/Alt key is pressed
            {
                // Raycast to find block
                if (Physics.Raycast(ray, out hitInfo))
                {
                    // Delete block
                    if (hitInfo.collider.gameObject.transform.parent == assetHolder.transform)
                    {
                        DestroyImmediate(hitInfo.collider.gameObject);
                        Event.current.Use();  // Consume the event so it doesn't trigger other behaviors
                        return;
                    }
                }
            }

            Vector3 hitPoint = Vector3.zero;
            // Debug.DrawRay(rayG.origin, rayG.direction * 100, Color.yellow, 2f);

            // Check for cube hit
            if (Physics.Raycast(ray, out hitInfo))
            {
                Debug.Log("Hit object: " + hitInfo.collider.gameObject.name);
                Debug.Log("Ray hitInfo point: " + hitInfo.point);
                if (WithinBounds(hitInfo.point, gridSize))
                {
                    PlaceBlock(GetSnapPosition(hitInfo.point, hitInfo.collider.gameObject.transform.position), "Block");
                    return;
                }
            }
            else
            {
                Debug.Log("Ray hit nothing");
            }

            // Try hitting the XY plane
            if (xyPlane.Raycast(ray, out enter))
            {
                hitPoint = ray.GetPoint(enter);
                if (WithinBounds(hitPoint, gridSize))
                {
                    PlaceBlock(hitPoint, "Plane");
                    return; // If block placed, no need to continue
                }
            }

            // Try hitting the XZ plane
            if (xzPlane.Raycast(ray, out enter))
            {
                hitPoint = ray.GetPoint(enter);
                if (WithinBounds(hitPoint, gridSize))
                {
                    PlaceBlock(hitPoint, "Plane");
                    return;
                }
            }

            // Try hitting the YZ plane
            if (yzPlane.Raycast(ray, out enter))
            {
                hitPoint = ray.GetPoint(enter);
                if (WithinBounds(hitPoint, gridSize))
                {
                    PlaceBlock(hitPoint, "Plane");
                    return;
                }
            }

            Debug.Log("Outside grid boundaries");
        }

        // Check if hitPoint is within the bounds
        bool WithinBounds(Vector3 hitPoint, float gridSize)
        {
            return hitPoint.x >= 0 && hitPoint.x <= gridSize &&
                hitPoint.y >= 0 && hitPoint.y <= gridSize &&
                hitPoint.z >= 0 && hitPoint.z <= gridSize;
        }

        // Place the selected block at the hitPoint
        void PlaceBlock(Vector3 hitPoint, string hitType)
        {
            float gridStep = step;
            Vector3 gridPosition = new Vector3(
                Mathf.Round(hitPoint.x / gridStep) * gridStep,
                Mathf.Round(hitPoint.y / gridStep) * gridStep,
                Mathf.Round(hitPoint.z / gridStep) * gridStep
            );

            Debug.Log("Block instantiated at: " + hitPoint);

            if (selectedBlock != null)
            {
                GameObject blockInstance = Instantiate(selectedBlock, gridPosition, Quaternion.identity);
                blockInstance.transform.SetParent(assetHolder.transform);
                // blockInstance.transform.position = SnapBlocks(hitPoint, blockInstance.transform.position);
            }
            else
            {
                Debug.Log("No block selected");
            }
        }

        Vector3 GetSnapPosition(Vector3 hitPoint, Vector3 otherBlock)
        {
            // Direction vector from hitpoint to otherBlock
            Vector3 direction = hitPoint - otherBlock;

            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y) && Mathf.Abs(direction.x) > Mathf.Abs(direction.z))
            {
                direction = new Vector3(Mathf.Sign(direction.x), 0, 0);
            }
            else if (Mathf.Abs(direction.y) > Mathf.Abs(direction.x) && Mathf.Abs(direction.y) > Mathf.Abs(direction.z))
            {
                direction = new Vector3(0, Mathf.Sign(direction.y), 0);
            }
            else
            {
                direction = new Vector3(0, 0, Mathf.Sign(direction.z));
            }

            // Calculate the position to place the new block based on the direction
            Vector3 snapPosition = otherBlock + direction * blockUnitSize;

            return snapPosition;
        }

        // Make the scene view repaint
        SceneView.RepaintAll();
    }
}
