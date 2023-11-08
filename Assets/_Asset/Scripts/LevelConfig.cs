using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelConfig : MonoBehaviour
{
    public static LevelConfig Instance;

    public int _generateCount = 10;
    public List<GameObject> _burnableList = new List<GameObject>();
    public float _startingFireCount = 7;
    public float _waterCapacity = 80;
    public float _waterReloadRate = 6f;
    public float levelScale = 0.2f;
    public string levelType;

    void Awake() 
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void SetLevelType(string levelType)
    {
        switch (levelType)
        {
            case "Village":
                _generateCount = 15;
                levelType = "Village";
                break;
            case "Forest":
                _generateCount = 18;
                levelType = "Forest";
                break;
            case "Building":
                _generateCount = 1;
                levelType = "Building";
                break;
            default:
                break;
        }
    }

    public void SetBurnableList(string levelType)
    {
        _burnableList.Clear();
        
        switch (levelType)
        {
            case "Village":
                _burnableList.Add(Resources.Load<GameObject>("Prefabs/Burnables/WalnutTree_L"));
                _burnableList.Add(Resources.Load<GameObject>("Prefabs/Burnables/WalnutTree_M"));
                _burnableList.Add(Resources.Load<GameObject>("Prefabs/Burnables/WalnutTree_S"));
                _burnableList.Add(Resources.Load<GameObject>("Prefabs/Burnables/PineTree_L"));
                _burnableList.Add(Resources.Load<GameObject>("Prefabs/Burnables/PineTree_M"));
                _burnableList.Add(Resources.Load<GameObject>("Prefabs/Burnables/House 1"));
                _burnableList.Add(Resources.Load<GameObject>("Prefabs/Burnables/House 2"));
                break;
            case "Forest":
                _burnableList.Add(Resources.Load<GameObject>("Prefabs/Burnables/WalnutTree_L"));
                _burnableList.Add(Resources.Load<GameObject>("Prefabs/Burnables/WalnutTree_M"));
                _burnableList.Add(Resources.Load<GameObject>("Prefabs/Burnables/WalnutTree_S"));
                _burnableList.Add(Resources.Load<GameObject>("Prefabs/Burnables/PineTree_L"));
                _burnableList.Add(Resources.Load<GameObject>("Prefabs/Burnables/PineTree_M"));
                break;
            case "Building":
                _burnableList.Add(Resources.Load<GameObject>("Prefabs/Burnables/Building1"));
                break;
            default:
                break;
        }
    }
}
