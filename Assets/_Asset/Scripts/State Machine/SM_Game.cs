using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SM_Game : StateMachine
{
    private static SM_Game _instance;
    public static SM_Game Instance => _instance ??= new SM_Game();


    // States
    private GSM_State_PlaneTracked _statePlaneTracked;
    private GSM_State_CursorPlaced _stateCursorPlaced;
    private GSM_State_Firefighting _stateFirefighting;
    private GSM_State_Paused _statePaused;
    private GSM_State_GameFinished _stateGameFinished;

    // Create the states
    public GSM_State_PlaneTracked GSM_State_PlaneTracked => _statePlaneTracked ??= new GSM_State_PlaneTracked(this);
    public GSM_State_CursorPlaced GSM_State_CursorPlaced => _stateCursorPlaced ??= new GSM_State_CursorPlaced(this);
    public GSM_State_Firefighting GSM_State_Firefighting => _stateFirefighting ??= new GSM_State_Firefighting(this);
    public GSM_State_Paused GSM_State_Paused => _statePaused ??= new GSM_State_Paused(this);
    public GSM_State_GameFinished GSM_State_GameFinished => _stateGameFinished ??= new GSM_State_GameFinished(this);

    
    public bool IsPlaneTracked => GetCurrentState() is GSM_State_PlaneTracked;
    public bool IsCursorPlaced => GetCurrentState() is GSM_State_CursorPlaced;
    public bool IsFirefighting => GetCurrentState() is GSM_State_Firefighting;
    public bool IsPaused => GetCurrentState() is GSM_State_Paused;
    public bool IsGameFinished => GetCurrentState() is GSM_State_GameFinished;

    public void Initialize()
    {
        // set the initial state
        SetInitialState(GSM_State_PlaneTracked);
        
    }
}