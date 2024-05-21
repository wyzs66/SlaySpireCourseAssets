using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敌人AI逻辑管理，处理敌人随机释放，或者重复释放等效果
/// </summary>
public class EnemyAIManager : BaseManager
{
    [SerializeField] EffectResolutionManager effectResolutionManager;

    private int currentRepeatCount;//当前重复行为的位置
    private List<EnemyAI> brains;//所有敌人的AI列表
    private const float ThinkingTime = 1.5f;

    public override void Initialize(CharacterObject player, List<CharacterObject> enemies)
    {
        base.Initialize(player, enemies);
        brains = new List<EnemyAI>(enemies.Count);

        foreach (var enemy in enemies)
        {
            brains.Add(new EnemyAI(enemy));
        }
    }
    /// <summary>
    /// 当玩家回合开始时，计算下一回合敌人的动作
    /// </summary>
    public void OnPlayerTurnBegin()
    {
        const int enemyIndex = 0;
        foreach(var enemy in Enemies)
        {
            var template = enemy.Template as EnemyTemplate;
            var brain = brains[enemyIndex];

            if (template != null)
            {
                if(brain.PatternIndex >= template.patterns.Count)
                {
                    brain.PatternIndex = 0;
                }

                var pattern = template.patterns[brain.PatternIndex];

                if(pattern is RepeatPattern repeatPattern)
                {
                    currentRepeatCount += 1;
                    if(currentRepeatCount == repeatPattern.Times)
                    {
                        currentRepeatCount = 0;
                        brain.PatternIndex += 1;
                    }
                    brain.Effects = pattern.Effects;
                }
                else if (pattern is RandomPattern randomPattern)
                {
                    var effcts = new List<int>();
                    var index = 0;

                    foreach(var probability in randomPattern.Probabilities)
                    {
                        var amount = probability.Value;
                        for(int i = 0; i < amount; i++)
                        {
                            effcts.Add(index); 
                        }
                        index++;
                    }
                    var randomIndex = Random.Range(0, effcts.Count - 1);
                    var selectedEffct = randomPattern.Effects[effcts[randomIndex]];
                    brain.Effects = new List<Effect> { selectedEffct };
                    brain.PatternIndex += 1;
                }
            }
        }
    }

    /// <summary>
    /// 敌人回合时，释放行为
    /// </summary>
    public void OnEnemyBegan()
    {
        StartCoroutine(ProcessEnemyBrains());
    }

    private IEnumerator ProcessEnemyBrains()
    {
        foreach(var brain in brains)
        {
            effectResolutionManager.SetCurrentEnemy(brain.Enemy);
            effectResolutionManager.ResolveEnemyEffect(brain.Enemy, brain.Effects);
            yield return new WaitForSeconds(ThinkingTime);
        }
    }
}
