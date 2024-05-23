using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StatusVariable", menuName = "CardGame/Variable/Status", order = 2)]
public class StatusVariable : ScriptableObject
{
    public Dictionary<string, int> Value = new Dictionary<string, int>();
    public Dictionary<string, StatusTempelate> Template = new Dictionary<string, StatusTempelate>();

    public GameEventStatus ValueChangedEvent;

    public int GetValue(string status)
    {
        if (Value.ContainsKey(status)) return Value[status];
        return 0;
    }

    public void SetValue(StatusTempelate status, int value)
    {
            Value[status.Name] = value;
            ValueChangedEvent?.Raise(status, value);

            if (!Template.ContainsKey(status.Name))
            {
                Template.Add(status.Name, status);
            }
    }
}
