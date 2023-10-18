using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;
using UnityEngine.UI;

public class GameSceneUIManager : MonoBehaviour
{
    public static GameSceneUIManager Instance;
    SM_Scene _smScene => SM_Scene.Instance;
    private GameObject _gameCanvas;
    [SerializeField] private GameObject _homeButton;
    [SerializeField] private GameObject _startLevelButton;
    // Start is called before the first frame update
    
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
        // _gameCanvas = Instance.gameObject;
        SceneController.Instance._gameCanvas = gameObject;
        _homeButton.SetActive(true);
        _startLevelButton.SetActive(false);
        SM_Game.Instance.GSM_State_LevelGenerated.OnEnter += ActivateStartLevelButton;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // private void ActivateGameSceneUI()
    // {
    //     _gameCanvas.SetActive(true);
    // }

    // private void DeactivateGameSceneUI()
    // {
    //     _gameCanvas.SetActive(false);
    // }

    public void OnHomeButtonClick()
    {
        _smScene.TryChangeState(_smScene.SSM_State_HomeScene);
        // SceneController.Instance.DeactivateGameSceneUI();
        UnitySceneManager.LoadScene("Home");
        // SceneController.Instance.ActivateHomeSceneUI();
    }

    public void OnStartLevelButtonClick()
    {
        if (SM_Game.Instance.IsLevelGenerated)
        {
            _startLevelButton.SetActive(false);
            SM_Game.Instance.TryChangeState(SM_Game.Instance.GSM_State_Firefighting);
            GameManager.Instance.StartLevel();
        }
    }

    private void ActivateStartLevelButton()
    {
        _startLevelButton.SetActive(true);
    }

    private void OnDestroy() 
    {
        // _smScene.SSM_State_GameScene.OnEnter -= ActivateGameSceneUI;
        // _smScene.SSM_State_GameScene.OnExit -= DeactivateGameSceneUI;
        SM_Game.Instance.GSM_State_LevelGenerated.OnEnter -= ActivateStartLevelButton;
    }
}
