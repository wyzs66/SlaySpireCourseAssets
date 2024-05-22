using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 对目标照成伤害的效果类
/// </summary>
[CreateAssetMenu(fileName = "DealDamageEffect", menuName = "CardGame/Templates/IntegerEffect/Deal Damage Effect", order = 4)]
[Serializable]
public class DealDamageEffect : IntegerEffect, IEntityEffect
{
    public override string GetName()
    {
        return $"Deal {Value.ToString()} damage ";
    }

    public override void Resolve(RuntimeCharacter source, RuntimeCharacter target)
    {
        var targetHp = target.Hp;
        var hp = targetHp.Value;

        var targetShield = target.Shield;
        var shield = targetShield.Value;

        var damage = Value;
        Debug.Log( "Deal Damage:" + damage);

        if(source.Status != null)
        {
            var weak = source.Status.GetValue("Weak");
            if(weak > 0)
            {
                damage = (int)Mathf.Floor(damage * 0.75f);
            }
        }

        if(damage > shield)
        {
            var newHp = hp - (damage - shield);
            if(newHp < 0) newHp = 0;
            targetHp.SetValue(newHp);
            targetShield.SetValue(0);

        }
        else
        {
            target.Shield.SetValue(shield -  damage);
        }
       

        
    }
}
