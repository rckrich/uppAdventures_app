using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Manager : MonoBehaviour
{
    protected void AddEventListener<T>(EventManager.EventDelegate<T> listener) where T : AppEvent
    {
        EventManager.instance.AddListener<T>(listener);
    }

    protected void InvokeEvent<T>(T e) where T : AppEvent
    {
        EventManager.instance.InvokeEvent<T>(e);
    }

    protected void RemoveEventListener<T>(EventManager.EventDelegate<T> listener) where T : AppEvent
    {
        EventManager.instance.RemoveListener<T>(listener);
    }
}
