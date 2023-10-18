using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance;
    SM_Scene _smScene => SM_Scene.Instance;
    SM_Game _smGame => SM_Game.Instance;
    public GameObject _homeCanvas;
    public GameObject _gameCanvas;


    private void Awake() 
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

    private void Start() 
    {
        _smScene.Initialize();

        // if (_smScene.IsHomeSceneState)
        //     _homeCanvas = HomeSceneUIManager.Instance.gameObject;
        
        // if (_smScene.IsGameSceneState)
        //     _gameCanvas = GameSceneUIManager.Instance.gameObject;

        // _smScene.SSM_State_HomeScene.OnEnter += SetHomeCanvas;
        // _smScene.SSM_State_GameScene.OnEnter += SetGameCanvas;
        // _smScene.SSM_State_HomeScene.OnExit += DeactivateHomeSceneUI;
        // _smScene.SSM_State_GameScene.OnEnter += ActivateGameSceneUI;
        // _smScene.SSM_State_GameScene.OnExit += DeactivateGameSceneUI;
    }

    // public void SetHomeCanvas()
    // {
    //     _homeCanvas = HomeSceneUIManager.Instance.gameObject;
    // }

    // public void SetGameCanvas()
    // {
    //     _gameCanvas = GameSceneUIManager.Instance.gameObject;
    // }

    public void ActivateHomeSceneUI()
    {
        _homeCanvas.SetActive(true);
    }

    public void DeactivateHomeSceneUI()
    {
        _homeCanvas.SetActive(false);
    }

    public void ActivateGameSceneUI()
    {
        _gameCanvas.SetActive(true);
    }

    public void DeactivateGameSceneUI()
    {
        _gameCanvas.SetActive(false);
    }

    private void OnDestroy() 
    {
        // _smScene.SSM_State_HomeScene.OnEnter -= ActivateHomeSceneUI;
        // _smScene.SSM_State_HomeScene.OnExit -= DeactivateHomeSceneUI;
        // _smScene.SSM_State_GameScene.OnEnter -= ActivateGameSceneUI;
        // _smScene.SSM_State_GameScene.OnExit -= DeactivateGameSceneUI;
        // _smScene.SSM_State_HomeScene.OnEnter -= () => _homeCanvas = HomeSceneUIManager.Instance.gameObject;
        // _smScene.SSM_State_GameScene.OnEnter -= () => _gameCanvas = GameSceneUIManager.Instance.gameObject;
    }
}
