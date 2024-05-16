using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 发动攻击的视觉效果基类
/// </summary>
public abstract class EffectAction : ScriptableObject
{
    public abstract string GetName();

    public abstract void Execute(GameObject gameObject);
}
