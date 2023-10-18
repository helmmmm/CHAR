using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;
using UnityEngine.UI;

public class HomeSceneUIManager : MonoBehaviour
{
    public static HomeSceneUIManager Instance;
    SM_Scene _smScene => SM_Scene.Instance;
    private GameObject _homeCanvas;
    [SerializeField] private GameObject _startButton;
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
        SM_Scene.Instance.Initialize();
        SM_Game.Instance.Initialize();
        SceneController.Instance._homeCanvas = gameObject;
        // _homeCanvas = Instance.gameObject;
        // _smScene.SSM_State_HomeScene.OnEnter += ActivateHomeSceneUI;
        // _smScene.SSM_State_HomeScene.OnExit += DeactivateHomeSceneUI;
        
        _startButton.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // private void ActivateHomeSceneUI()
    // {
    //     _homeCanvas.SetActive(true);
    // }

    // private void DeactivateHomeSceneUI()
    // {
    //     _homeCanvas.SetActive(false);
    // }

    public void OnStartButtonClick()
    {
        _smScene.TryChangeState(_smScene.SSM_State_GameScene);
        // SceneController.Instance.DeactivateHomeSceneUI();
        UnitySceneManager.LoadScene("Game");
        // SceneController.Instance.ActivateGameSceneUI();
    }

    private void OnDisable() 
    {
        // _smScene.SSM_State_HomeScene.OnEnter -= ActivateHomeSceneUI;
        // _smScene.SSM_State_HomeScene.OnExit -= DeactivateHomeSceneUI;    
    }
}
