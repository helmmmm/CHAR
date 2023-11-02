using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GSM_State_CameraActivated : State
{
    public GSM_State_CameraActivated(StateMachine stateMachine) : base(stateMachine) { }

    public override string GetName()
    {
        return "GSM_State_CameraActivated";
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
