using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneStateMachine : StateMachine
{
    private static SceneStateMachine _instance;
    public static SceneStateMachine Instance => _instance ??= new SceneStateMachine();


    // States
    private StateHomeScene _stateHomeScene;
    private StateGameScene _stateGameScene;


    // Create the states
    public StateHomeScene StateHomeScene => _stateHomeScene ??= new StateHomeScene(this);
    public StateGameScene StateGameScene => _stateGameScene ??= new StateGameScene(this);

    
    public bool IsHomeSceneState => GetCurrentState() is StateHomeScene;
    public bool IsGameSceneState => GetCurrentState() is StateGameScene;
        
    public void Initialize()
    {
        // set the initial state
        SetInitialState(StateHomeScene);
        
    }
}
