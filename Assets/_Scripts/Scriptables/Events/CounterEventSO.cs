using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CounterEventSO", menuName = "Scriptable Objects/Events/Counter")]
public class CounterEventSO : ScriptableObject {
    public event Action<BaseCounter> OnCounterRequested;

    private void OnDisable()
    {
        OnCounterRequested = null;
    }

    public void RaiseEvent(BaseCounter counter)
    {
        if (OnCounterRequested != null)
        {
            OnCounterRequested.Invoke(counter);
                }
        else
        {
            Debug.LogWarning("A CounterEvent was requested, but nobody picked it up.");
        }
    }
}
