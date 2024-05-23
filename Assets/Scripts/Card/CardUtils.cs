using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class CardUtils
{
    public static bool CardCanToBePlayed(CardTemplate template, IntVariable mana)
    {
        return(mana.Value - template.Cost) >= 0;
    }
    /// <summary>
    /// ���ݿ���Ŀ�����ͣ��жϿ�������
    /// </summary>
    /// <param name="card"></param>
    /// <returns></returns>
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
