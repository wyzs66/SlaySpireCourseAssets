using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����Ч������������
/// </summary>
public class EffectResolutionManager : BaseManager
{
    private CharacterObject _currentEnemy;

    public void ResolveCardEffects(RuntimeCard card, CharacterObject playerSelectedTarget)
    {
        foreach(var effect in card.Template.Effects)
        {
            var targetableEffect = effect as TargetableEffect; 
            
            if (targetableEffect != null)
            {
                var targets = GetTargets(targetableEffect, playerSelectedTarget, true);
                foreach(var target in targets)
                {
                    targetableEffect.Resolve(Player.Character, target.Character);
                }
            }
        }
    }

    /// <summary>
    /// ��ȡ����������Ŀ������б�
    /// </summary>
    /// <param name="effect"></param>
    /// <param name="playerSelectedTarget"></param>
    /// <param name="playerSource"></param>
    /// <returns></returns>
    private List<CharacterObject> GetTargets(TargetableEffect effect, CharacterObject playerSelectedTarget, bool playerSource)
    {
        var targets = new List<CharacterObject>();

        // �ж��Ƿ�����ҷ���
        if(playerSource)
        {
            switch(effect.TargetType)
            {
                case EffectTarfetType.Self:
                    targets.Add(Player);
                    break;
                case EffectTarfetType.TargetEnemy:
                    targets.Add(playerSelectedTarget);
                    break;
            }
        }
        else
        {
            switch (effect.TargetType)
            {
                case EffectTarfetType.Self:
                    targets.Add(_currentEnemy);
                    break;
                case EffectTarfetType.TargetEnemy:
                    targets.Add(Player);
                    break;
            }
        }
        return targets;
    }
}
