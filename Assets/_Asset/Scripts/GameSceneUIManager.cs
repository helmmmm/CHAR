using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;
using UnityEngine.UI;
using TMPro;

public class GameSceneUIManager : MonoBehaviour
{
    public static GameSceneUIManager Instance;
    SM_Scene _smScene => SM_Scene.Instance;
    SM_Game _smGame => SM_Game.Instance;
    private GameObject _gameCanvas;
    [SerializeField] private GameObject _startLevelButton;
    [SerializeField] private GameObject _gameTimer;
    [SerializeField] private GameObject _countDownUI;
    [SerializeField] private GameObject _pauseButton;
    [SerializeField] private GameObject _pauseUI;
    [SerializeField] private GameObject _confirmationUI;
    [SerializeField] private GameObject _scanUI;
    [SerializeField] private GameObject _directionsUI;
    [SerializeField] private GameObject _finishScreenUI;

    public GameObject _waterGauge;
    public TMP_Text _gameTimerText;
    public TMP_Text _countDownText;
    public TMP_Text _burntRaw;
    public TMP_Text _burntPenalty;
    public TMP_Text _extinguishedRaw;
    public TMP_Text _extinguishedBonus;
    public TMP_Text _waterAccuracyRaw;
    public TMP_Text _waterAccuracyBonus;
    public TMP_Text _timeBonusFinal;
    public TMP_Text _overallScore;

    // [SerializeField] private TMP_Text _directionsText;
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
        SceneController.Instance._gameCanvas = gameObject;
        _startLevelButton.SetActive(false);
        _countDownUI.SetActive(false);
        _waterGauge.SetActive(false);
        _confirmationUI.SetActive(false);
        _directionsUI.SetActive(false);
        _scanUI.SetActive(false);
        _pauseUI.SetActive(false);
        _gameTimer.SetActive(false);
        _finishScreenUI.SetActive(false);
        // Group into initialize method later

        

        // _waterGauge.SetActive(false);
        // _pauseButton.SetActive(false);
    }

    private void OnEnable() 
    {
        _smGame.GSM_State_CameraActivated.OnEnter += EnableScanUI;
        _smGame.GSM_State_PlaneTracked.OnEnter += DisableScanUI;
        _smGame.GSM_State_PlaneTracked.OnEnter += EnableDirectionsUI;
        _smGame.GSM_State_CursorPlaced.OnEnter += DisableDirectionsUI;

        // _smScene.SSM_State_GameScene.OnExit += () => gameObject.SetActive(false);
        _smGame.GSM_State_Firefighting.OnEnter += EnableGamePlayUI;
        _smGame.GSM_State_GameFinished.OnEnter += DisableGamePlayUI;
    }

    private void OnDisable() 
    {
        _smGame.GSM_State_CameraActivated.OnEnter -= EnableScanUI;
        _smGame.GSM_State_PlaneTracked.OnEnter -= DisableScanUI;
        _smGame.GSM_State_PlaneTracked.OnEnter -= EnableDirectionsUI;
        _smGame.GSM_State_CursorPlaced.OnEnter -= DisableDirectionsUI;

        // _smScene.SSM_State_GameScene.OnExit += () => gameObject.SetActive(false);
        _smGame.GSM_State_Firefighting.OnEnter -= EnableGamePlayUI;
        _smGame.GSM_State_GameFinished.OnEnter -= DisableGamePlayUI;    
    }

    private void EnableScanUI()
    {
        _scanUI.SetActive(true);
    }

    private void DisableScanUI()
    {
        _scanUI.SetActive(false);
    }

    private void EnableDirectionsUI()
    {
        _directionsUI.SetActive(true);
    }

    private void DisableDirectionsUI()
    {
        _directionsUI.SetActive(false);
    }

    public void OnStartLevelButtonClick()
    {
        if (SM_Game.Instance.IsLevelGenerated)
        {
            _startLevelButton.SetActive(false);
            GameManager.Instance.StartLevel();
        }
    }

    public void EnableStartLevelButton()
    {
        _startLevelButton.SetActive(true);
    }

    public void EnableGamePlayUI()
    {
        _waterGauge.SetActive(true);
        _pauseButton.SetActive(true);
        _gameTimer.SetActive(true);
    }

    public void DisableGamePlayUI()
    {
        _waterGauge.SetActive(false);
        _pauseButton.SetActive(false);
        _gameTimer.SetActive(false);
    }

    public void EnableCountDownUI()
    {
        _countDownUI.SetActive(true);
    }

    public void DisableCountDownUI()
    {
        _countDownUI.SetActive(false);
    }

    private void EnableWaterGauge()
    {
        _waterGauge.SetActive(true);
    }

    private void DisableWaterGauge()
    {
        _waterGauge.SetActive(false);
    }

    private void EnableConfirmationUI()
    {
        _confirmationUI.SetActive(true);
    }

    private void DisableConfirmationUI()
    {
        _confirmationUI.SetActive(false);
    }

    public void OnPause()
    {
        _smGame.TryChangeState(_smGame.GSM_State_Paused);
        _pauseButton.SetActive(false);
        _pauseUI.SetActive(true);
    }

    public void OnResume()
    {
        _smGame.TryChangeState(_smGame.GSM_State_Firefighting);
        _pauseButton.SetActive(true);
        _pauseUI.SetActive(false);
    }

    public void EnableFinishScreen()
    {
        _finishScreenUI.SetActive(true);
    }

    public void OnMainMenuButtonClick()
    {
        _confirmationUI.SetActive(true);
        _pauseUI.SetActive(false);
    }

    public void OnMainMenuYes()
    {
        _smScene.TryChangeState(_smScene.SSM_State_HomeScene);
        UnitySceneManager.LoadScene("Home");
    }

    public void OnMainMenuNo()
    {
        _pauseUI.SetActive(true);
        _confirmationUI.SetActive(false);
    }

    public void ChangeText(TMP_Text textObj, string newText)
    {
        textObj.text = newText;
    }

    private void OnDestroy() 
    {
        // _smGame.GSM_State_CameraActivated.OnEnter -= EnableScanUI;
        // _smGame.GSM_State_CameraActivated.OnExit -= DisableScanUI;
        // _smGame.GSM_State_PlaneTracked.OnEnter -= EnableDirectionsUI;
        // _smGame.GSM_State_CursorPlaced.OnEnter -= DisableDirectionsUI;

        // // _smScene.SSM_State_GameScene.OnExit -= () => gameObject.SetActive(false);
        // _smGame.GSM_State_Firefighting.OnEnter -= EnableGamePlayUI;
        // _smGame.GSM_State_GameFinished.OnEnter -= DisableGamePlayUI;
    }
}
