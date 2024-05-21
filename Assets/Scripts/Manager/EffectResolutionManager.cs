using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����Ч������������
/// </summary>
public class EffectResolutionManager : BaseManager
{
    private CharacterObject _currentEnemy;
    public CardSelectionHasArrow cardSelectionHasArrow;

    /// <summary>
    /// ������ҹ����Ϳ���Ч��
    /// </summary>
    /// <param name="card">ѡ��Ŀ���</param>
    /// <param name="playerSelectedTarget">����Ŀ��</param>
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

                    foreach(var groupManager in targetableEffect.SourceActions)
                    {
                        foreach(var group in groupManager.Group.Actions)
                        {
                            group.Execute(Player.gameObject);
                        }
                    }

                    foreach (var groupManager in targetableEffect.TargetActions)
                    {
                        foreach (var group in groupManager.Group.Actions)
                        {
                            var enemy = cardSelectionHasArrow.GetSelectedEnemy();
                            group.Execute(enemy.gameObject);
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// ������ҷǹ����Ϳ���Ч��
    /// </summary>
    /// <param name="card">ѡ��Ŀ���</param>
    public void ResolveCardEffects(RuntimeCard card)
    {
        foreach (var effect in card.Template.Effects)
        {
            var targetableEffect = effect as TargetableEffect;

            if (targetableEffect != null)
            {
                var targets = GetTargets(targetableEffect, null, true);
                foreach (var target in targets)
                {
                    targetableEffect.Resolve(Player.Character, target.Character);
                }
            }
        }
    }

    /// <summary>
    /// ���˷������ΪЧ������
    /// </summary>
    /// <param name="enemy">������Ϊ�ĵ���</param>
    /// <param name="effects">��Ϊ��Ч���б�</param>
    public void ResolveEnemyEffect(CharacterObject enemy, List<Effect> effects)
    {
        foreach(var  effect in effects)
        {
            var targetableEffect = effect as TargetableEffect;
            if(targetableEffect != null)
            {
                var targets = GetTargets(targetableEffect, null, false);
                foreach (var target in targets)
                {
                    targetableEffect.Resolve(enemy.Character, target.Character);
                }
            }
        }
    }
    /// <summary>
    /// ���õ�ǰ����
    /// </summary>
    /// <param name="enemy"></param>
    public void SetCurrentEnemy(CharacterObject enemy)
    {
        _currentEnemy = enemy;
    }

    /// <summary>
    /// ��ȡ����������Ŀ������б�
    /// </summary>
    /// <param name="effect">������Ϊ��Ч��</param>
    /// <param name="playerSelectedTarget">���ѡ���Ŀ��</param>
    /// <param name="playerSource">�Ƿ�����ҷ���</param>
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
