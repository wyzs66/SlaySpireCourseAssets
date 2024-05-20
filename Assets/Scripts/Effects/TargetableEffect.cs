using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 对目标实现卡牌的效果
/// </summary>
public abstract class TargetableEffect : Effect
{
    public EffectTarfetType TargetType;
    public abstract void Resolve(RuntimeCharacter source, RuntimeCharacter target);

}
