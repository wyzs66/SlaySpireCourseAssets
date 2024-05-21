using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 卡牌效果解析管理器
/// </summary>
public class EffectResolutionManager : BaseManager
{
    private CharacterObject _currentEnemy;
    public CardSelectionHasArrow cardSelectionHasArrow;

    /// <summary>
    /// 解析玩家攻击型卡牌效果
    /// </summary>
    /// <param name="card">选择的卡牌</param>
    /// <param name="playerSelectedTarget">卡牌目标</param>
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
    /// 解析玩家非攻击型卡牌效果
    /// </summary>
    /// <param name="card">选择的卡牌</param>
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
    /// 敌人发起的行为效果解析
    /// </summary>
    /// <param name="enemy">发起行为的敌人</param>
    /// <param name="effects">行为的效果列表</param>
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
    /// 设置当前敌人
    /// </summary>
    /// <param name="enemy"></param>
    public void SetCurrentEnemy(CharacterObject enemy)
    {
        _currentEnemy = enemy;
    }

    /// <summary>
    /// 获取对象发起动作的目标对象列表
    /// </summary>
    /// <param name="effect">发起行为的效果</param>
    /// <param name="playerSelectedTarget">玩家选择的目标</param>
    /// <param name="playerSource">是否是玩家发起</param>
    /// <returns></returns>
    private List<CharacterObject> GetTargets(TargetableEffect effect, CharacterObject playerSelectedTarget, bool playerSource)
    {
        var targets = new List<CharacterObject>();

        // 判断是否是玩家发起
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
