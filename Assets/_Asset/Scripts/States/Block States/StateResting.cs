using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateResting : State
{
    public StateResting(StateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        Debug.Log("Enter Resting");
    }

    public override void TryStateTransition(IState state)
    {
        ExecuteStateTransition(state);
    }

    public override void Exit()
    {
        Debug.Log("Exit Resting");
    }

}
