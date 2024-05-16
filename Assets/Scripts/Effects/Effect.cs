using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 攻击效果的基类
/// </summary>
public abstract class Effect : ScriptableObject
{
    public List<EffectActionGroupManager> SourceActions = new List<EffectActionGroupManager>();//发起者的动作效果
    public List<EffectActionGroupManager> TargetActions = new List<EffectActionGroupManager>();//目标的动作效果
    public abstract string GetName();
}
