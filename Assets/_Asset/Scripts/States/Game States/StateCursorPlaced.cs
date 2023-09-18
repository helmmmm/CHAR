using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateCursorPlaced : State
{
    public StateCursorPlaced(StateMachine stateMachine) : base(stateMachine) { }

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
