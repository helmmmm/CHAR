using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GSM_State_LevelGenerated : State
{
    public GSM_State_LevelGenerated(StateMachine stateMachine) : base(stateMachine) { }

    public override string GetName()
    {
        return "GSM_State_LevelGenerated";
    }

    // public override void Enter()
    // {
    //     Debug.Log("Enter Cursor Placed State");
    // }

    public override void TryStateTransition(IState state)
    {
        ExecuteStateTransition(state);
    }

    // public override void Exit()
    // {
    //     Debug.Log("Exit Cursor Placed State");
    // }
}
