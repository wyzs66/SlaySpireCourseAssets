using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 对目标释放BUUF的效果产生器
/// </summary>
[CreateAssetMenu(fileName = "ApplayStatusEffect", menuName = "CardGame/Templates/IntegerEffect/Applay Status Effect", order = 7)]
[Serializable]
public class ApplayStatusEffect : IntegerEffect, IEntityEffect
{
    public StatusTempelate Status;
    public override string GetName()
    {
        if (Status != null)
        {
            return $"{Value.ToString()}{Status.Name}";
        }
        return "Applay Status";
    }

    public override void Resolve(RuntimeCharacter source, RuntimeCharacter target)
    {
        var currentValue = target.Status.GetValue(Status.Name);
        target.Status.SetValue(Status, currentValue + Value);
    }
}
