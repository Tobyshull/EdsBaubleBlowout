using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseState
{
    public string StateName;
    protected StateMachine stateMachine;

    public BaseState(string _stateName, StateMachine _machine)
    {
        StateName = _stateName;
        stateMachine = _machine;
    }

    public virtual void Enter() { }
    public virtual void UpdateLogic() { }
    public virtual void UpdatePhysics() { }
    public virtual void Exit() { }
}

public class StateMachine : MonoBehaviour
{

    BaseState currentState;

    void Start()
    {
        currentState = GetInitialState();
        if (currentState != null)
            currentState.Enter();
    }

    void Update()
    {
        if (currentState != null)
            currentState.UpdateLogic();
    }

    void LateUpdate()
    {
        if (currentState != null)
            currentState.UpdatePhysics();
    }

    public void ChangeState(BaseState newState)
    {
        currentState.Exit();

        currentState = newState;
        Debug.Log("Changed State To: " + currentState.StateName);
        currentState.Enter();
    }

    protected virtual BaseState GetInitialState()
    {
        return null;
    }
}
