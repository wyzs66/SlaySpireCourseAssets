using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameEventInt", menuName ="CardGame/Event/Game event (int)", order = 1)]
public class GameEventInt : ScriptableObject
{
    private List<GameEventIntListener> listeners = new List<GameEventIntListener>();

    public void Raise( int value)
    {
        for(int i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i].OnEventRaised(value);
        }
    }

    public void RegisterListener(GameEventIntListener listener)
    {
        if(!listeners.Contains(listener))
        {
            listeners.Add(listener);
        }
    }

    public void UnRegisterListener(GameEventIntListener listener)
    {
        if (listeners.Contains(listener))
        {
            listeners.Remove(listener);
        }
    }
}
