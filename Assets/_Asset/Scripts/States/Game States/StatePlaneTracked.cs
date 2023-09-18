using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatePlaneTracked : State
{
    public StatePlaneTracked(StateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        Debug.Log("Enter Plane Tracked State");
    }

    public override void TryStateTransition(IState state)
    {
        ExecuteStateTransition(state);
    }

    public override void Exit()
    {
        Debug.Log("Exit Plan Tracked State");
    }
}
