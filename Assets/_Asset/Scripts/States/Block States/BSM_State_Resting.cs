using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BSM_State_Resting : State
{
    public BSM_State_Resting(StateMachine stateMachine) : base(stateMachine) { }

    public override string GetName()
    {
        return "BSM_State_Resting";
    }

    // public override void Enter()
    // {
    //     // Debug.Log("Enter Resting\n");
        

    //     // Detect nearby blocks
    //     // Original Shader
    // }

    public override void TryStateTransition(IState state)
    {
        ExecuteStateTransition(state);
    }

    // public override void Exit()
    // {
    //     // Debug.Log("Exit Resting\n");
    //     // Ignition methods & animations
    // }

}