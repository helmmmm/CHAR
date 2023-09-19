using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GSM_State_Paused : State
{
    public GSM_State_Paused(StateMachine stateMachine) : base(stateMachine) { }

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
