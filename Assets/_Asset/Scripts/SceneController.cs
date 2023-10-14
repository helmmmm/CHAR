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

    // private void OnEnable()
    // {
    //     UnitySceneManager.sceneLoaded += OnSceneLoaded;
    // }

    private void Start()
    {
        _smScene.Initialize();

        // _smScene.SSM_State_GameScene.OnEnter += OnGameSceneLoad;
        // _smScene.SSM_State_HomeScene.OnExit += ;
    }

    public void OnStartButtonClick()
    {
        UnitySceneManager.LoadScene("Game");
        _smScene.TryChangeState(_smScene.SSM_State_GameScene);
        // _smScene
    }

    public void OnRestartButtonClick()
    {
        // Regenerate level
    }

    public void OnHomeButtonClick()
    {
        UnitySceneManager.LoadScene("Home");
        _smScene.TryChangeState(_smScene.SSM_State_HomeScene);
    }

    // private void OnSceneloaded()
    // {

    // }
}
