using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    public StateFSM currentState;

    public void Initialize(StateFSM startingState)
    {
        currentState = startingState;
        startingState.Enter();
    }

    public void ChangeState(StateFSM newState)
    {
        currentState.Exit();
        currentState = newState;
        newState.Enter();
    }
}
