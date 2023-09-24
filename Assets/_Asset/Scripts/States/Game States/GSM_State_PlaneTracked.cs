using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GSM_State_PlaneTracked : State
{
    public GSM_State_PlaneTracked(StateMachine stateMachine) : base(stateMachine) { }

    public override string GetName()
    {
        return "GSM_State_PlaneTracked";
    }

    // public override void Enter()
    // {
    //     Debug.Log("Enter Plane Tracked State");
    // }

    public override void TryStateTransition(IState state)
    {
        ExecuteStateTransition(state);
    }

    // public override void Exit()
    // {
    //     Debug.Log("Exit Plan Tracked State");
    // }
}
