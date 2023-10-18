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

    // UI
    private GameObject _fightFireButton;

    void Start()
    {
        Instance = this;
        _smScene.Initialize();
        _smGame.Initialize();

        // _smScene.SSM_State_GameScene.OnEnter += GetLevelGenerator;
        // _smGame.GSM_State_CursorPlaced.OnEnter += ActivateLevelGeneration;
        // _smGame.GSM_State_LevelGenerated.OnEnter += FillBurnableList;
        
    }

    // private void GetLevelGenerator()
    // {
    //     _levelGenerator = GameObject.Find("Level(Clone)").GetComponent<LevelGenerator>();
    // }

    public void StartLevel()
    {
        StartCoroutine("co_CountDownAndIgnite");
    }

    IEnumerator co_CountDownAndIgnite() // from UI eventsystem
    {
        Debug.Log("starting fire in 3");
        yield return new WaitForSeconds(3);
        LevelGenerator.Instance.IgniteRandoms();
    }

    // Update is called once per frame
    void Update()
    {
    
    }

    private void OnDestroy() 
    {
        StopCoroutine("co_CountDownAndIgnite");
        // _smGame.GSM_State_LevelGenerated.OnEnter -= FillBurnableList;
    }
}
