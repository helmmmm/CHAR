using UnityEngine;
using System.Collections;
public class SSM_State_GameScene : State
{
    public SSM_State_GameScene(StateMachine stateMachine) : base(stateMachine) { }

    public override string GetName()
    {
        return "SSM_State_GameScene";
    }

    // public override void Enter()
    // {
    //     Debug.Log("Enter Game Scene");
    // }

    public override void TryStateTransition(IState state)
    {
        ExecuteStateTransition(state);
    }

    // public override void Exit()
    // {
    //     Debug.Log("Exit Game Scene");
    // }
    
}