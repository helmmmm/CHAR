using System.Runtime.CompilerServices;
using UnityEngine;

public class StateMachine
{

    private IState _currentState;
    
    
    protected void SetInitialState(IState state)
    {
        _currentState = new State(this);
        _currentState.TryStateTransition(state); 
    }
    
    
    public IState GetCurrentState()
    {
        return _currentState;
    }

    
    public void TryChangeState(State newState)
    {
        Debug.Log($"TryChangeState ({newState.GetName()})\n");
        _currentState.TryStateTransition(newState);
    }
    
    
    public void ExecuteStateTransition(IState newState)
    {
        _currentState = newState;
        newState.Enter();
    }

    
}