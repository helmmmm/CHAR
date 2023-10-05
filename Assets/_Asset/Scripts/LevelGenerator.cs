using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public List<GameObject> _burnablePrefabs = new List<GameObject>();
    // private List<Vector3> _usedPositions = new List<Vector3>();
    public int _burnableCount = 10; // Use density?
    private SM_Game _smGame;
    private Renderer _levelBaseRenderer;
    private Vector3 _levelBaseMin;
    private Vector3 _levelBaseMax;
    private bool _generationInProgress = false;


    // Start is called before the first frame update
    void Start()
    {
        _smGame = new SM_Game();
        _smGame.Initialize();
        _smGame.GSM_State_CursorPlaced.OnEnter += GenerateLevel;
        _levelBaseRenderer = GetComponent<Renderer>();
        _levelBaseMin = _levelBaseRenderer.bounds.min;
        _levelBaseMax = _levelBaseRenderer.bounds.max;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            if (!_generationInProgress)
            {
                GenerateLevel();
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
                // _usedPositions.Clear();
            }
        }
    }

    private void GenerateLevel()
    {
        StartCoroutine(co_GenerateLevel());
    }

    IEnumerator co_GenerateLevel()
    {
        _generationInProgress = true;
        List<Bounds> createdBoundsList = new List<Bounds>();
        int positionSearchCount = 0;
        int positionSearchMax = 100;

        for (int i = 0; i < _burnableCount; i++)
        {
            Vector3 randomSpawnPoint = GetRandomSpawnPoint();
            GameObject randomPrefab = GetRandomPrefab();

            GameObject spawnedObj = Instantiate(randomPrefab, randomSpawnPoint, Quaternion.identity);
            ShowMesh(spawnedObj.transform, false);

            if (createdBoundsList.Count > 0)
            {
                while (!IsPositionAvailable(createdBoundsList, spawnedObj.GetComponent<Collider>().bounds))
                {
                    randomSpawnPoint = GetRandomSpawnPoint();
                    spawnedObj.transform.position = randomSpawnPoint;
                    var temp = spawnedObj.GetComponentsInChildren<Renderer>();

                    if (positionSearchCount++ >= positionSearchMax)
                    {
                        Destroy(spawnedObj);
                        break;
                    }

                    yield return null;
                }
            }
            // Debug.Log("Spawned at: " + randomSpawnPoint + " with prefab: " + randomPrefab.name + " at index: " + i + " of " + _burnableCount + "\n");
            spawnedObj.transform.parent = transform;
            ShowMesh(spawnedObj.transform, true);
            createdBoundsList.Add(spawnedObj.GetComponent<Collider>().bounds);
        }
        _generationInProgress = false;
    }

    private GameObject GetRandomPrefab()
    {
        int randomIndex = Random.Range(0, _burnablePrefabs.Count);
        GameObject randomPrefab = _burnablePrefabs[randomIndex];
        return randomPrefab;
    }

    private Vector3 GetRandomSpawnPoint()
    {
        float x = Random.Range(_levelBaseMin.x, _levelBaseMax.x);
        float y = Random.Range(_levelBaseMin.y, _levelBaseMax.y);
        float z = Random.Range(_levelBaseMin.z, _levelBaseMax.z);

        Vector3 randomSpawnPoint = new Vector3(x, y, z);

        return randomSpawnPoint;
    }

    private bool IsPositionAvailable(List<Bounds> createdBoundsList, Bounds newBounds)
    {
        foreach (Bounds bound in createdBoundsList)
        {
            if (!IsBoundsApart(bound, newBounds))
            {
                return false;
            }
        }
        return true;
    }
    
    private bool IsBoundsApart(Bounds b1, Bounds b2)
    {
        if (b1.min.x > b2.max.x || b1.max.x < b2.min.x) return true;
        if (b1.min.z > b2.max.z || b1.max.z < b2.min.z) return true;
        return false;
    }

    // private void ShowMesh(GameObject obj, bool show = true)
    // {
    //     MeshRenderer[] meshRenderers = obj.GetComponentsInChildren<MeshRenderer>();
    //     foreach (MeshRenderer meshRenderer in meshRenderers)
    //     {
    //         meshRenderer.enabled = show;
    //     }
    // }

    private void ShowMesh(Transform parent, bool show)
    {
        foreach (Transform child in parent)
        {
            child.gameObject.SetActive(show);

            ShowMesh(child, show);
        }
    }
}

