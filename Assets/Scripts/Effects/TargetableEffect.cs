using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����Ŀ��
/// </summary>
public abstract class TargetableEffect : Effect
{
    public EffectTarfetType TargetType;
    public abstract void Resolve(RuntimeCharacter source, RuntimeCharacter target);

}
