using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{

    public enum States
    {
        Idle,
        Moving,
        Attack,
        Dead,
        Blocking
    }

    public GameObject owner;
    public State globalState;
    public State currentState;
    public State previousState;

    public void SetValues (State currentState, State globalState, GameObject owner)
    {
        previousState = this.currentState = currentState;
        this.globalState = globalState;
        this.owner = owner;
        currentState.OnStatetEnter(owner);
    }

    public void Execute()
    {
        if (globalState != null)
            globalState.Execute(owner);
        if (currentState != null)
            currentState.Execute(owner);
    }

    public void SetState(State state)
    {
        currentState.OnStateExit(owner);
        previousState = currentState;
        currentState = state;
        currentState.OnStatetEnter(owner);
    }

    public GameObject GetOwner()
    {
        return this.owner;
    }

    public State GetGlobalState()
    {
        return this.globalState;
    }

    public State GetCurrentState()
    {
        return this.currentState;
    }

    public State GetPreviousState()
    {
        return this.previousState;
    }
}
