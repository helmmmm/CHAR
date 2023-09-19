using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GSM_State_CursorPlaced : State
{
    public GSM_State_CursorPlaced(StateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        Debug.Log("Enter Cursor Placed State");
    }

    public override void TryStateTransition(IState state)
    {
        ExecuteStateTransition(state);
    }

    public override void Exit()
    {
        Debug.Log("Exit Cursor Placed State");
    }
}
