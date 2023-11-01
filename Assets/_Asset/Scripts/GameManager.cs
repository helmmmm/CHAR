using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    SM_Scene _smScene => SM_Scene.Instance;
    SM_Game _smGame => SM_Game.Instance;
    private float burnTimer = 0;
    private int _startingFireCount = 5;
    private bool _isFireFighting = false;
    private bool _isPauseClicked = false;

    // UI
    private GameObject _fightFireButton;

    void Start()
    {
        Instance = this;
        _smScene.Initialize();
        _smGame.Initialize();
        
        _smGame.GSM_State_LevelGenerated.OnEnter += EnableStartLevelButton;
        _smGame.GSM_State_Firefighting.OnEnter += EnableWaterShooter;
        _smGame.GSM_State_Firefighting.OnExit += DisableWaterShooter;
        _smGame.GSM_State_Paused.OnEnter += Pause;
        _smGame.GSM_State_Paused.OnExit += Resume;
    }

    private void EnableStartLevelButton()
    {
        GameSceneUIManager.Instance.EnableStartLevelButton();
    }

    public void StartLevel()
    {
        CountDownAndAction("IgniteRandom");
    }

    private void CountDownAndAction(string method)
    {
        StartCoroutine(co_CountDownAndAction(method));
    }

    IEnumerator co_CountDownAndAction(string method)
    {
        GameSceneUIManager.Instance.EnableCountDownUI();
        for (int i = 3; i > 0; i--)
        {
            GameSceneUIManager.Instance.SetCountDownText(i.ToString());
            yield return new WaitForSeconds(1);
        }
        GameSceneUIManager.Instance.DisableCountDownUI();
        Invoke(method, 0);
    }

    private void IgniteRandom()
    {
        LevelGenerator.Instance.IgniteRandoms();
        SM_Game.Instance.TryChangeState(SM_Game.Instance.GSM_State_Firefighting);
    }

    public bool IsFireFighting()
    {
        return _isFireFighting;
    }

    private void EnableWaterShooter()
    {
        _isFireFighting = true;
    }

    private void DisableWaterShooter()
    {
        _isFireFighting = false;
    }

    private void EnableGamePlayUI()
    {
        GameSceneUIManager.Instance.EnableGamePlayUI();
    }

    private void Pause()
    {
        Time.timeScale = 0;
    }

    private void Resume()
    {
        Time.timeScale = 1;
    }

    // private void CountDownAndResume()
    // {
    //     StartCoroutine(co_CountDownAndAction("Resume"));
    // }

    // Update is called once per frame
    void Update()
    {
    
    }

    private void OnDestroy() 
    {
        StopCoroutine("co_CountDownAndIgnite");
        _smGame.GSM_State_LevelGenerated.OnEnter -= EnableStartLevelButton;
        _smGame.GSM_State_Firefighting.OnEnter -= EnableWaterShooter;
        _smGame.GSM_State_Firefighting.OnExit -= DisableWaterShooter;
        _smGame.GSM_State_Paused.OnEnter -= Pause;
        _smGame.GSM_State_Paused.OnExit -= Resume;
    }
}
