using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class CardUtils
{
    public static bool CarHasTargetableEffect(CardTemplate card)
    {
        foreach (var effect in card.Effects)
        {
            if (effect is TargetableEffect targetableEffect)
            {
                if(targetableEffect.TargetType == EffectTarfetType.TargetEnemy)
                {
                    return true;
                }
            }
        }
        return false;
    }
}
