using UnityEngine;
using System.Collections;
public class SSM_State_HomeScene : State
{
    public SSM_State_HomeScene(StateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        Debug.Log("Enter Home Scene");
    }

    public override void TryStateTransition(IState state)
    {
        ExecuteStateTransition(state);
    }

    public override void Exit()
    {
        Debug.Log("Exit Home Scene");
    }
}