using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mFSM<T> where T : Enum
{

    Dictionary<T, State> States;
    T currentState;

    public mFSM(T initState)
    {

        States = new Dictionary<T, State>();
        foreach (T e in System.Enum.GetValues(typeof(T)))
        {
            States.Add(e, new State());
        }

        currentState = initState;
    }

    public void Update()
    {
        States[currentState].OnStay?.Invoke();
    }


    public void ChangeState(T newState)
    {
        States[currentState].OnExit?.Invoke();

        States[newState].OnEnter?.Invoke();
        currentState = newState;

    }

    public void SetOnStay(T state, Action f)
    {
        States[state].OnStay = f;
    }
    public void SetOnEnter(T state, Action f)
    {
        States[state].OnEnter = f;
    }
    public void SetOnExit(T state, Action f)
    {
        States[state].OnExit = f;
    }
}
