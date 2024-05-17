using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "IntegerEffect", menuName = "CardGame/Templates/IntegerEffect", order = 4)]
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
        var damage = Value;
        Debug.Log( "Deal Damage:" + damage);

        var newHp = hp - damage;
        if (newHp < 0)
        {
            newHp = 0;
        }

        targetHp.SetValue(newHp);
    }
}
