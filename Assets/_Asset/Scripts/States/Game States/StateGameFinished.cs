using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateGameFinished : State
{
    public StateGameFinished(StateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        Debug.Log("Enter Game Finished State");
    }

    public override void TryStateTransition(IState state)
    {
        ExecuteStateTransition(state);
    }

    public override void Exit()
    {
        Debug.Log("Exit Game Finished State");
    }
}
