using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ¹¥»÷Ä¿±ê
/// </summary>
public abstract class TargetableEffect : Effect
{
    public EffectTarfetType TargetType;
    public abstract void Resolve(RuntimeCharacter source, RuntimeCharacter target);

}
