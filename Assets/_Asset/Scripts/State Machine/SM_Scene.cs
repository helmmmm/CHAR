using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SM_Scene : StateMachine
{
    private static SM_Scene _instance;
    public static SM_Scene Instance => _instance ??= new SM_Scene();


    // States
    private SSM_State_HomeScene _stateHomeScene;
    private SSM_State_GameScene _stateGameScene;


    // Create the states
    public SSM_State_HomeScene SSM_State_HomeScene => _stateHomeScene ??= new SSM_State_HomeScene(this);
    public SSM_State_GameScene SSM_State_GameScene => _stateGameScene ??= new SSM_State_GameScene(this);

    
    public bool IsHomeSceneState => GetCurrentState() is SSM_State_HomeScene;
    public bool IsGameSceneState => GetCurrentState() is SSM_State_GameScene;
        
    public void Initialize()
    {
        // set the initial state
        SetInitialState(SSM_State_HomeScene);
        
    }
}
