using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IntentChangeEvent", menuName = "CardGame/Event/Intent Change Event", order = 3)]
public class IntentChangeEvent : ScriptableObject
{
    private List<IntentChangeListener> listeners = new List<IntentChangeListener>();

    public void Raise(Sprite sprite, int value)
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i].OnEventRaised(sprite, value);
        }
    }

    public void RegisterListener(IntentChangeListener listener)
    {
        if (!listeners.Contains(listener))
        {
            listeners.Add(listener);
        }
    }

    public void UnRegisterListener(IntentChangeListener listener)
    {
        if (listeners.Contains(listener))
        {
            listeners.Remove(listener);
        }
    }
}
