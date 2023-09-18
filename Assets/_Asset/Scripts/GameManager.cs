using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    SceneStateMachine sceneStateMachine = new SceneStateMachine();

    // Start is called before the first frame update
    void Start()
    {
        sceneStateMachine.Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("c"))
        {
            if (sceneStateMachine.IsHomeSceneState)
            {
                sceneStateMachine.TryChangeState(sceneStateMachine.StateGameScene);
            }
            else if (sceneStateMachine.IsGameSceneState)
            {
                sceneStateMachine.TryChangeState(sceneStateMachine.StateHomeScene);
            }
        }
    }
}
