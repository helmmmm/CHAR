using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;
using UnityEngine.UI;
using TMPro;

public class HomeSceneUIManager : MonoBehaviour
{
    public static HomeSceneUIManager Instance;
    SM_Scene _smScene => SM_Scene.Instance;
    private GameObject _homeCanvas;
    public LevelTypeDB _levelTypeDB;
    private int _levelTypeOption = 0;
    public DifficultyDB _difficultyDB;
    private int _difficultyOption = 0;
    public LevelSizeDB _levelSizeDB;
    private int _levelSizeOption = 0;

    [SerializeField] private GameObject _startButton;
    // Start is called before the first frame update

    [SerializeField] private GameObject _levelTypeLeftButton;
    [SerializeField] private GameObject _levelTypeRightButton;
    [SerializeField] private GameObject _difficultyLeftButton;
    [SerializeField] private GameObject _difficultyRightButton;
    [SerializeField] private GameObject _levelSizeLeftButton;
    [SerializeField] private GameObject _levelSizeRightButton;

    [SerializeField] private TMP_Text _levelTypeText;
    [SerializeField] private TMP_Text _difficultyText;
    [SerializeField] private TMP_Text _levelSizeText;
    
    void Awake() 
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        _smScene.Initialize();
        SM_Game.Instance.Initialize();
        SceneController.Instance._homeCanvas = gameObject;
        // _homeCanvas = Instance.gameObject;
        // _smScene.SSM_State_HomeScene.OnEnter += ActivateHomeSceneUI;
        // _smScene.SSM_State_HomeScene.OnExit += DeactivateHomeSceneUI;
        
        _startButton.SetActive(true);
    }

    private void OnEnable() 
    {
        _smScene.SSM_State_HomeScene.OnEnter += ConfigRoutine;    
    }

    private void OnDisable() 
    {
        _smScene.SSM_State_HomeScene.OnEnter -= ConfigRoutine;
    }

    private void ConfigRoutine()
    {
        UpdateLevelType(_levelTypeOption);
        UpdateDifficulty(_difficultyOption);
        UpdateLevelSize(_levelSizeOption);
    }

    public void OnLevelTypeRightClick()
    {
        _levelTypeOption++;
        if (_levelTypeOption >= _levelTypeDB.levelTypeCount)
        {
            _levelTypeOption = 0;
        }
        UpdateLevelType(_levelTypeOption);
    }

    public void OnLevelTypeLeftClick()
    {
        _levelTypeOption--;
        if (_levelTypeOption < _levelTypeDB.levelTypeCount)
        {
            _levelTypeOption = _levelTypeDB.levelTypeCount - 1;
        }
        UpdateLevelType(_levelTypeOption);
    }

    private void UpdateLevelType(int levelTypeOption)
    {
        LevelType levelType = _levelTypeDB.GetLevelType(levelTypeOption);
        _levelTypeText.text = levelType._levelTypeName;
        LevelConfig.Instance.SetLevelType(levelType._levelTypeName);
        LevelConfig.Instance.levelType = levelType._levelTypeName;
        LevelConfig.Instance.SetBurnableList(levelType._levelTypeName);
    }

    public void OnDifficultyRightClick()
    {
        _difficultyOption++;
        if (_difficultyOption >= _difficultyDB.difficultyTypesCount)
        {
            _difficultyOption = 0;
        }
        UpdateDifficulty(_difficultyOption);
    }

    public void OnDifficultyLeftClick()
    {
        _difficultyOption--;
        if (_difficultyOption < _difficultyDB.difficultyTypesCount)
        {
            _difficultyOption = _difficultyDB.difficultyTypesCount - 1;
        }
        UpdateDifficulty(_difficultyOption);
    }

    public void UpdateDifficulty(int difficultyOption)
    {
        Difficulty difficulty = _difficultyDB.GetDifficulty(difficultyOption);
        _difficultyText.text = difficulty._difficulty;
        LevelConfig.Instance._startingFireCount = _difficultyDB.GetDifficulty(difficultyOption)._startingFireCount;
        LevelConfig.Instance._waterCapacity = _difficultyDB.GetDifficulty(difficultyOption)._waterCapacity;
        LevelConfig.Instance._waterReloadRate = _difficultyDB.GetDifficulty(difficultyOption)._waterReloadRate;
    }

    public void OnLevelSizeRightClick()
    {
        _levelSizeOption++;
        if (_levelSizeOption >= _levelSizeDB.levelSizesCount)
        {
            _levelSizeOption = 0;
        }
        UpdateLevelSize(_levelSizeOption);
    }

    public void OnLevelSizeLeftClick()
    {
        _levelSizeOption--;
        if (_levelSizeOption < _levelSizeDB.levelSizesCount)
        {
            _levelSizeOption = _levelSizeDB.levelSizesCount - 1;
        }
        UpdateLevelSize(_levelSizeOption);
    }

    private void UpdateLevelSize(int levelSizeOption)
    {
        LevelSize levelSize = _levelSizeDB.GetLevelSize(levelSizeOption);
        _levelSizeText.text = levelSize._size;
        LevelConfig.Instance.levelScale = _levelSizeDB.GetLevelSize(levelSizeOption)._multiplier;
    }

    public void OnStartButtonClick()
    {
        _smScene.TryChangeState(_smScene.SSM_State_GameScene);
        // SceneController.Instance.DeactivateHomeSceneUI();
        UnitySceneManager.LoadScene("Game");
        // SceneController.Instance.ActivateGameSceneUI();
    }
}