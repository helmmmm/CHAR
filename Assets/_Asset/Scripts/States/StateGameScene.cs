using UnityEngine;
using System.Collections;
public class StateGameScene : State
{
    public StateGameScene(StateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        Debug.Log("Enter Game Scene");
    }

    public override void TryStateTransition(IState state)
    {
        ExecuteStateTransition(state);
    }

    public override void Exit()
    {
        Debug.Log("Exit Game Scene");
    }
    
}