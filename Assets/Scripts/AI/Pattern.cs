using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// AI行为模式基类
/// </summary>
public abstract class Pattern : ScriptableObject
{
    public List<Effect> Effects = new List<Effect>();
    public abstract string GetName();
}
