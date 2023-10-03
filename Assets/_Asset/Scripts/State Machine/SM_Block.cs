using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SM_Block : StateMachine
{
    private static SM_Block _instance;
    public static SM_Block Instance => _instance ??= new SM_Block();

    // States
    private BSM_State_Resting _stateResting;
    // private BSM_State_Ignited _stateIgnited;
    private BSM_State_Burning _stateBurning;
    private BSM_State_Burnt _stateBurnt;


    // Create the states
    public BSM_State_Resting BSM_State_Resting => _stateResting ??= new BSM_State_Resting(this);
    public BSM_State_Burning BSM_State_Burning => _stateBurning ??= new BSM_State_Burning(this);
    public BSM_State_Burnt BSM_State_Burnt => _stateBurnt ??= new BSM_State_Burnt(this);


    public bool IsRestingState => GetCurrentState() is BSM_State_Resting;
    public bool IsBurningState => GetCurrentState() is BSM_State_Burning;
    public bool IsBurnt => GetCurrentState() is BSM_State_Burnt;

    public void Initialize()
    {
        // set the initial state
        SetInitialState(BSM_State_Resting);
    }

    public void CheckComponent()
    {
        // Debug.Log("I'm here");
    }
}
