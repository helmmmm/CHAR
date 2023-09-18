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
    SceneStateMachine sceneStateMachine = new SceneStateMachine();
    BlockStateMachine blockStateMachine = new BlockStateMachine();
    GameStateMachine gameStateMachine = new GameStateMachine();
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
                sceneStateMachine.TryChangeState(sceneStateMachine.StateGameScene);
                gameStateMachine.Initialize();
            }
            else if (sceneStateMachine.IsGameSceneState)
            {
                sceneStateMachine.TryChangeState(sceneStateMachine.StateHomeScene);
                blockStateMachine.TryChangeState(blockStateMachine.StateResting);

            }
        }

        // Track plane
        if (Input.GetKeyDown("t"))
        {
            if (sceneStateMachine.IsGameSceneState)
            {
                if (gameStateMachine.IsPlaneTracked)
                {
                    gameStateMachine.TryChangeState(gameStateMachine.StateCursorPlaced);
                }
                else if (gameStateMachine.IsCursorPlaced)
                {
                    gameStateMachine.TryChangeState(gameStateMachine.StateFirefighting);
                }
                else if (gameStateMachine.IsFirefighting)
                {
                    gameStateMachine.TryChangeState(gameStateMachine.StateGameFinished);
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
                    gameStateMachine.TryChangeState(gameStateMachine.StatePaused);
                }
                else if (gameStateMachine.IsPaused)
                {
                    gameStateMachine.TryChangeState(gameStateMachine.StateFirefighting);
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
                    blockStateMachine.TryChangeState(blockStateMachine.StateBurning);
                }
            }
        }

        // Burn out after 2 second 
        if (blockStateMachine.IsBurningState)
        {
            burnTimer += Time.deltaTime;
            if (burnTimer >= 2)
            {
                blockStateMachine.TryChangeState(blockStateMachine.StateBurnt);
            }
        }
    }

    // private IEnumerator BurntAfterSeconds()
    // {
    //     yield return new WaitForSeconds(1);
    //     blockStateMachine.TryChangeState(blockStateMachine.StateBurnt);
    // }
}
