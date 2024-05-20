using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 对目标产生护盾值的效果类
/// </summary>
[CreateAssetMenu(fileName = "ShieldEffect", menuName = "CardGame/Templates/IntegerEffect/Shield Effect", order = 6)]
[Serializable]
public class ShieldEffect : IntegerEffect, IEntityEffect
{
    public override string GetName()
    {
        return $"Shield:{Value.ToString()}";
    }

    public override void Resolve(RuntimeCharacter source, RuntimeCharacter target)
    {
        var shield = target.Shield;
        shield.SetValue(shield.Value + Value);
        Debug.Log("Shield:" + shield);
    }
}
