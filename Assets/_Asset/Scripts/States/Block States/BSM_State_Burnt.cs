using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BSM_State_Burnt : State
{
    public BSM_State_Burnt(StateMachine stateMachine) : base(stateMachine) { }

    public override string GetName()
    {
        return "BSM_State_Burnt";
    }

    // public override void Enter()
    // {
    //     // Block.Burnt();
    //     // Debug.Log("Enter Burnt");
    //     // Burnt Shader
    // }

    public override void TryStateTransition(IState state)
    {
        ExecuteStateTransition(state);
    }

    // public override void Exit()
    // {
    //     // Debug.Log("Exit Burnt");
    // }

}
