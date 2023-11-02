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

        _homeCanvas = Resources.Load<GameObject>("Prefabs/Canvas/Home Canvas");
        _gameCanvas = Resources.Load<GameObject>("Prefabs/Canvas/Game Canvas");
    }

    private void OnEnable() 
    {
        _smScene.SSM_State_HomeScene.OnEnter += SetTimeScale;
        _smScene.SSM_State_GameScene.OnEnter += SetTimeScale;
    }

    private void OnDisable() 
    {
        _smScene.SSM_State_HomeScene.OnEnter -= SetTimeScale;
        _smScene.SSM_State_GameScene.OnEnter -= SetTimeScale;
    }

    private void SetTimeScale()
    {
        if (Time.timeScale != 1f)
        {
            Time.timeScale = 1f;
        }
    }

    private void SetHomeCanvas()
    {
        Time.timeScale = 1f;
        Instantiate(_homeCanvas);
    }

    private void SetGameCanvas()
    {
        Time.timeScale = 1f;
        Instantiate(_gameCanvas);
    }

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
}
