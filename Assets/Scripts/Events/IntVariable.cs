using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 整型数值变化触发器
/// </summary>
[CreateAssetMenu(fileName = "InttegerVariable", menuName = "CardGame/Variable/Integer", order = 1)]
public class IntVariable : ScriptableObject
{
    public int Value;
    public GameEventInt ValueChangedEvent;

    public void SetValue(int value)
    {
        Value = value;
        //Debug.Log("hp:" + value);
        ValueChangedEvent?.Raise(value);
    }
}
