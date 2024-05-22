using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameEventStatus", menuName = "CardGame/Event/Game Event Status", order = 4)]
public class GameEventStatus : ScriptableObject
{
    private List<GameEventStatusListener> listeners = new List<GameEventStatusListener>();

    public void Raise(StatusTempelate status, int value)
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i].OnEventRaised(status, value);
        }
    }

    public void RegisterListener(GameEventStatusListener listener)
    {
        if (!listeners.Contains(listener))
        {
            listeners.Add(listener);
        }
    }

    public void UnRegisterListener(GameEventStatusListener listener)
    {
        if (listeners.Contains(listener))
        {
            listeners.Remove(listener);
        }
    }
}
