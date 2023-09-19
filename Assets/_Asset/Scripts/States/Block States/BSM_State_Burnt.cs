using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BSM_State_Burnt : State
{
    public BSM_State_Burnt(StateMachine stateMachine) : base(stateMachine) { }

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
