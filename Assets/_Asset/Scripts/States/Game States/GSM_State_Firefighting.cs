using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GSM_State_Firefighting : State
{
    public GSM_State_Firefighting(StateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        Debug.Log("Enter Firefighting State");
    }

    public override void TryStateTransition(IState state)
    {
        ExecuteStateTransition(state);
    }

    public override void Exit()
    {
        Debug.Log("Exit Firefighting State");
    }
}
