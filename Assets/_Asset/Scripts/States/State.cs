using System;
    
public interface IState
{
    void Enter();
    void Exit();

    void TryStateTransition(IState newSate);
}

public class State : IState
{
    private StateMachine MyStateMachine { get; set; }
    
    
    
    public State(StateMachine stateMachine)
    {
        MyStateMachine = stateMachine;
    }


    public virtual void Enter()
    {
        OnEnter?.Invoke();
    }

    public virtual void Exit()
    {
        OnExit?.Invoke();
    }
    
    public event Action OnEnter;
    public event Action OnExit;
    
    
    public virtual void TryStateTransition(IState newSate)
    {
        MyStateMachine.ExecuteStateTransition(newSate);
    }

    
    protected void ExecuteStateTransition(IState newState)
    {
        Exit();
        MyStateMachine.ExecuteStateTransition(newState);
    }

}