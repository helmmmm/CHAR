using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatePaused : State
{
    public StatePaused(StateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        Debug.Log("Paused");
    }

    public override void TryStateTransition(IState state)
    {
        ExecuteStateTransition(state);
    }

    public override void Exit()
    {
        Debug.Log("Resumed");
    }
}
