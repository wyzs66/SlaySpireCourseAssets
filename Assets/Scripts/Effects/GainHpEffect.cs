using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 对目标产生回复效果的效果类
/// </summary>
[CreateAssetMenu(fileName = "GainHpEffect", menuName = "CardGame/Templates/IntegerEffect/Gain Hp Effect", order = 5)]
[Serializable]
public class GainHpEffect : IntegerEffect, IEntityEffect
{
    public override string GetName()
    {
        return $"{Value.ToString()} HP";
    }

    public override void Resolve(RuntimeCharacter source, RuntimeCharacter target)
    {
        var targetHp = target.Hp;
        var finalHp = targetHp.Value + Value;

        if (finalHp > target.MaxHp)
        {
            finalHp = target.MaxHp;
        }

        targetHp.SetValue(finalHp);
        Debug.Log("Gain Hp");
    }
}
