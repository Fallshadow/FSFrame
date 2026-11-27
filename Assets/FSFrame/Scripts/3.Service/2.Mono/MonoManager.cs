using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoManager : ManagerBase<MonoManager>
{
    private Action updateEvent;
    private Action lateUpdateEvent;
    private Action fixedUpdateEvent;

    public void AddUpdateListener(Action action)
    {
        updateEvent += action;
    }

    public void RemoveUpdateListener(Action action)
    {
        updateEvent -= action;
    }

    public void AddLateUpdateListener(Action action)
    {
        lateUpdateEvent += action;
    }

    public void RemoveLateUpdateListener(Action action)
    {
        lateUpdateEvent -= action;
    }

    public void AddFixedUpdateListener(Action action)
    {
        fixedUpdateEvent += action;
    }

    public void RemoveFixedUpdateListener(Action action)
    {
        fixedUpdateEvent -= action;
    }

    private void Update()
    {
        updateEvent?.Invoke();
    }
    private void LateUpdate()
    {
        lateUpdateEvent?.Invoke();
    }
    private void FixedUpdate()
    {
        fixedUpdateEvent?.Invoke();
    }
}