using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    * "c" for changing scene
    * "f" for progressing fire on block
    * "t" for tracking plane
    * "p" for pausing and resuming
*/

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    SM_Scene sceneStateMachine = new SM_Scene();
    SM_Block blockStateMachine = new SM_Block();
    SM_Game gameStateMachine = new SM_Game();
    private float burnTimer = 0;

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

    // Start is called before the first frame update
    void Start()
    {
        sceneStateMachine.Initialize();
        blockStateMachine.Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        // Change between home and game scene
        if (Input.GetKeyDown("c"))
        {
            if (sceneStateMachine.IsHomeSceneState)
            {
                sceneStateMachine.TryChangeState(sceneStateMachine.SSM_State_GameScene);
                gameStateMachine.Initialize();
            }
            else if (sceneStateMachine.IsGameSceneState)
            {
                sceneStateMachine.TryChangeState(sceneStateMachine.SSM_State_HomeScene);
                blockStateMachine.TryChangeState(blockStateMachine.BSM_State_Resting);

            }
        }

        // Track plane
        if (Input.GetKeyDown("t"))
        {
            if (sceneStateMachine.IsGameSceneState)
            {
                if (gameStateMachine.IsPlaneTracked)
                {
                    gameStateMachine.TryChangeState(gameStateMachine.GSM_State_CursorPlaced);
                }
                else if (gameStateMachine.IsCursorPlaced)
                {
                    gameStateMachine.TryChangeState(gameStateMachine.GSM_State_Firefighting);
                }
                else if (gameStateMachine.IsFirefighting)
                {
                    gameStateMachine.TryChangeState(gameStateMachine.GSM_State_GameFinished);
                }
            }
        }

        // Pause and resume
        if (Input.GetKeyDown("p"))
        {
            if (sceneStateMachine.IsGameSceneState)
            {
                if (gameStateMachine.IsFirefighting)
                {
                    gameStateMachine.TryChangeState(gameStateMachine.GSM_State_Paused);
                }
                else if (gameStateMachine.IsPaused)
                {
                    gameStateMachine.TryChangeState(gameStateMachine.GSM_State_Firefighting);
                }
            }
        }

        // Progress fire on block
        if (Input.GetKeyDown("f"))
        {
            if (sceneStateMachine.IsGameSceneState)
            {
                if (blockStateMachine.IsRestingState)
                {
                    blockStateMachine.TryChangeState(blockStateMachine.BSM_State_Burning);
                }
            }
        }

        // Burn out after 2 second 
        if (blockStateMachine.IsBurningState)
        {
            burnTimer += Time.deltaTime;
            if (burnTimer >= 2)
            {
                blockStateMachine.TryChangeState(blockStateMachine.BSM_State_Burnt);
            }
        }
    }

    // private IEnumerator BurntAfterSeconds()
    // {
    //     yield return new WaitForSeconds(1);
    //     blockStateMachine.TryChangeState(blockStateMachine.BSM_State_Burnt);
    // }
}
