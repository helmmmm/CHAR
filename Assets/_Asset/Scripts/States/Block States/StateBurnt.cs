using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateBurnt : State
{
    public StateBurnt(StateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        Debug.Log("Enter Burnt");
    }

    public override void TryStateTransition(IState state)
    {
        ExecuteStateTransition(state);
    }

    public override void Exit()
    {
        Debug.Log("Exit Burnt");
    }

}
