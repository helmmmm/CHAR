using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockStateMachine : StateMachine
{
    private static BlockStateMachine _instance;
    public static BlockStateMachine Instance => _instance ??= new BlockStateMachine();


    // States
    private StateResting _stateResting;
    private StateBurning _stateBurning;
    private StateBurnt _stateBurnt;


    // Create the states
    public StateResting StateResting => _stateResting ??= new StateResting(this);
    public StateBurning StateBurning => _stateBurning ??= new StateBurning(this);
    public StateBurnt StateBurnt => _stateBurnt ??= new StateBurnt(this);


    public bool IsRestingState => GetCurrentState() is StateResting;
    public bool IsBurningState => GetCurrentState() is StateBurning;
    public bool IsBurnt => GetCurrentState() is StateBurnt;

    public void Initialize()
    {
        // set the initial state
        SetInitialState(StateResting);
        
    }
}
