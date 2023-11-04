using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public static LevelGenerator Instance;
    private List<GameObject> _burnablePrefabs = new List<GameObject>();
    public List<GameObject> _initialBurnableBlocks = new List<GameObject>();
    // private List<Vector3> _usedPositions = new List<Vector3>();
    private int _burnableCount; // Use density?
    private SM_Game _smGame => SM_Game.Instance;
    private Renderer _levelBaseRenderer;
    private Vector3 _levelBaseMin;
    private Vector3 _levelBaseMax;
    private bool _generationInProgress = false;
    public List<GameObject> _generatedBurnables = new List<GameObject>();

    // Only ignite visible blocks, at least one open face
    // If generate count goes over number of burnables, generate within one

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        _burnableCount = LevelConfig.Instance._generateCount;
        foreach (GameObject burnable in LevelConfig.Instance._burnableList)
        {
            _burnablePrefabs.Add(burnable);
        }

        _levelBaseRenderer = GetComponent<Renderer>();
        _levelBaseMin = _levelBaseRenderer.bounds.min;
        _levelBaseMax = _levelBaseRenderer.bounds.max;
        GenerateLevel();
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
                _generatedBurnables.Clear();
                // _usedPositions.Clear();
            }
        }
    }

    public void GenerateLevel()
    {
        StartCoroutine(co_GenerateLevel());
    }

    IEnumerator co_GenerateLevel()
    {
        yield return new WaitForSeconds(0.5f);
        // Generating level UI message
        _generationInProgress = true;
        // GameSceneUIManager.Instance.EnableGeneratingLevelUI();
        List<Bounds> createdBoundsList = new List<Bounds>();

        for (int i = 0; i < _burnableCount; i++)
        {
            int positionSearchCount = 0;
            int positionSearchMax = 50;

            Vector3 randomSpawnPoint = GetRandomSpawnPoint();
            GameObject randomPrefab = GetRandomPrefab();
            int randomRotationSide = Random.Range(0, 3);

            GameObject spawnedObj = Instantiate(randomPrefab, randomSpawnPoint, Quaternion.Euler(0, 90 * randomRotationSide, 0));
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
            if (spawnedObj != null)
            {
                _generatedBurnables.Add(spawnedObj);
                spawnedObj.transform.parent = transform;
                createdBoundsList.Add(spawnedObj.GetComponent<Collider>().bounds);
            }
            else
            {
                i--;
            }
        }
        
        transform.localScale *= LevelConfig.Instance.levelScale;
        foreach (GameObject burnable in _generatedBurnables)
        {
            ShowMesh(burnable.transform, true);
        }

        _generationInProgress = false;
        _smGame.TryChangeState(_smGame.GSM_State_LevelGenerated);
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
            // Debug.Log($"B1 min X: {bound.min.x} max X: {bound.max.x} min Z: {bound.min.z} max Z: {bound.max.z}\n");
            // Debug.Log($"B2 min X: {newBounds.min.x} max X: {newBounds.max.x} min Z: {newBounds.min.z} max Z: {newBounds.max.z}\n");

            if (!IsBoundsApart(bound, newBounds))
            {
                return false;
            }
            // Debug.Log("+++++++++++++++++++ Bounds apart true +++++++++++++++++++\n");
        }
        return true;
    }
    
    private bool IsBoundsApart(Bounds b1, Bounds b2)
    {
        if (b1.min.x > b2.max.x || b1.max.x < b2.min.x) return true;
        if (b1.min.z > b2.max.z || b1.max.z < b2.min.z) return true;
        return false;
    }

    private void ShowMesh(Transform parent, bool show)
    {
        foreach (Transform child in parent)
        {
            child.gameObject.SetActive(show);

            ShowMesh(child, show);
        }
    }

    private List<GameObject> GetRandomBurnablesFromList()
    {
        List<GameObject> randomBurnables = new List<GameObject>();
        List<GameObject> temp = new List<GameObject>(_generatedBurnables);

        for (int i = 0; i < LevelConfig.Instance._startingFireCount; i++)
        {
            if (temp.Count == 0)
                temp.AddRange(_generatedBurnables);

            int randomIndex = Random.Range(0, temp.Count);
            randomBurnables.Add(temp[randomIndex]);
            temp.RemoveAt(randomIndex);
        }

        return randomBurnables;
    }

    private List<Block> GetRandomBlocksOnBurnable()
    {
        List<Block> randomBlocks = new List<Block>();

        foreach (GameObject burnable in GetRandomBurnablesFromList())
        {
            int randomBlockIndex = Random.Range(0, burnable.transform.childCount);
            Block blockAtIndex = burnable.transform.GetChild(randomBlockIndex).GetComponent<Block>();
            // if the block has at least 1 exposed surface
            if (blockAtIndex._nearbyBlockList.Count < 6)
                randomBlocks.Add(blockAtIndex);
        }

        return randomBlocks;
    }

    public void IgniteRandoms()
    {
        foreach (Block block in GetRandomBlocksOnBurnable())
        {
            block.Ignite();
        }
    }

    // private void OnDisable() 
    // {
    //     _smGame.GSM_State_CursorPlaced.OnEnter -= () => GenerateLevel();    
    // }

    private void OnDestroy() 
    {
        if (Instance == this)
            Instance = null;

        StopCoroutine(co_GenerateLevel());
    }
}

