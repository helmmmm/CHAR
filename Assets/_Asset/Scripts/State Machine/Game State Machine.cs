using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateMachine : StateMachine
{
    private static GameStateMachine _instance;
    public static GameStateMachine Instance => _instance ??= new GameStateMachine();


    // States
    private StatePlaneTracked _statePlaneTracked;
    private StateCursorPlaced _stateCursorPlaced;
    private StateFirefighting _stateFirefighting;
    private StatePaused _statePaused;
    private StateGameFinished _stateGameFinished;

    // Create the states
    public StatePlaneTracked StatePlaneTracked => _statePlaneTracked ??= new StatePlaneTracked(this);
    public StateCursorPlaced StateCursorPlaced => _stateCursorPlaced ??= new StateCursorPlaced(this);
    public StateFirefighting StateFirefighting => _stateFirefighting ??= new StateFirefighting(this);
    public StatePaused StatePaused => _statePaused ??= new StatePaused(this);
    public StateGameFinished StateGameFinished => _stateGameFinished ??= new StateGameFinished(this);

    
    public bool IsPlaneTracked => GetCurrentState() is StatePlaneTracked;
    public bool IsCursorPlaced => GetCurrentState() is StateCursorPlaced;
    public bool IsFirefighting => GetCurrentState() is StateFirefighting;
    public bool IsPaused => GetCurrentState() is StatePaused;
    public bool IsGameFinished => GetCurrentState() is StateGameFinished;

    public void Initialize()
    {
        // set the initial state
        SetInitialState(StatePlaneTracked);
        
    }
}
