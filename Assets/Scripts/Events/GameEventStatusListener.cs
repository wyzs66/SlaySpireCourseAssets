using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameEventStatusListener : MonoBehaviour
{
    public GameEventStatus Event;
    public UnityEvent<StatusTempelate, int> Response;

    private void OnEnable()
    {
        Event.RegisterListener(this);
    }

    private void OnDisable()
    {
        Event.UnRegisterListener(this);
    }

    public void OnEventRaised(StatusTempelate status, int value)
    {
        Response.Invoke(status, value);
    }
}
