using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateBurning : State
{
    public StateBurning(StateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        Debug.Log("Enter Burning");
    }

    public override void TryStateTransition(IState state)
    {
        ExecuteStateTransition(state);
    }

    public override void Exit()
    {
        Debug.Log("Exit Burning");
    }

}
