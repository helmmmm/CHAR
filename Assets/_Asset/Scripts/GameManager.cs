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
    public int _currentFireCount = 0; // To determine early game end state
    // public int _burntCount = 0;

    // UI
    private GameObject _fightFireButton;

    void Start()
    {
        _smGame.Initialize();
        Instance = this;
    }

    private void OnEnable() 
    {
        _smGame.GSM_State_LevelGenerated.OnEnter += EnableStartLevelButton;
        _smGame.GSM_State_Firefighting.OnEnter += () => _isFireFighting = true;
        _smGame.GSM_State_Firefighting.OnExit += () => _isFireFighting = false;
        _smGame.GSM_State_Paused.OnEnter += Pause;
        _smGame.GSM_State_Paused.OnExit += Resume;
    }

    private void OnDisable() 
    {
        _smGame.GSM_State_LevelGenerated.OnEnter -= EnableStartLevelButton;
        _smGame.GSM_State_Firefighting.OnEnter -= () => _isFireFighting = true;
        _smGame.GSM_State_Firefighting.OnExit -= () => _isFireFighting = false;
        _smGame.GSM_State_Paused.OnEnter -= Pause;
        _smGame.GSM_State_Paused.OnExit -= Resume;
    }

    private void EnableStartLevelButton()
    {
        GameSceneUIManager.Instance.EnableStartLevelButton();
    }

    public void StartLevel()
    {
        CountDownAndAction("IgniteRandom");
        StartCoroutine(co_CountTotalBlocks());
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
            GameSceneUIManager.Instance.ChangeText(GameSceneUIManager.Instance._countDownText, i.ToString());
            yield return new WaitForSeconds(1);
        }
        GameSceneUIManager.Instance.DisableCountDownUI();
        Invoke(method, 0);
    }

    IEnumerator co_CountTotalBlocks()
    {
        ScoreManager.Instance._totalBlockCount = GameObject.FindGameObjectsWithTag("Burnable Block").Length;
        yield return null;
    }

    private void IgniteRandom()
    {
        LevelGenerator.Instance.IgniteRandoms();
        SM_Game.Instance.TryChangeState(SM_Game.Instance.GSM_State_Firefighting);
    }

    public bool IsFireGone()
    {
        return _currentFireCount == 0;
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
        // Debug.Log(_smGame.GetCurrentState()+"\n");
    }

    private void OnDestroy() 
    {
        StopCoroutine("co_CountDownAndIgnite");
    }
}
