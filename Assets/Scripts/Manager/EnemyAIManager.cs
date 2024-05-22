using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敌人AI逻辑管理，处理敌人随机释放，或者重复释放等效果
/// </summary>
public class EnemyAIManager : BaseManager
{
    [SerializeField] private EffectResolutionManager effectResolutionManager;
    [SerializeField] private List<IntentChangeEvent> intentEvents;

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
        //遍历所有敌人
        foreach(var enemy in Enemies)
        {
            //获取敌人模版，取得身上属性
            var template = enemy.Template as EnemyTemplate;
            //当前敌人的AI逻辑
            var brain = brains[enemyIndex];

            if (template != null)
            {
                //判断当前行为层是否大于总行为层
                if(brain.PatternIndex >= template.patterns.Count)
                {
                    brain.PatternIndex = 0;
                }
                Sprite sprite = null;
                //获取当前层的行为
                var pattern = template.patterns[brain.PatternIndex];
                //判断行为属于随机行为还是重复行为
                if(pattern is RepeatPattern repeatPattern)
                {
                    currentRepeatCount += 1;
                    //如果当前重复行为的次数等于最大重复次数，则进入下一个行为，重置重复次数
                    if(currentRepeatCount == repeatPattern.Times)
                    {
                        currentRepeatCount = 0;
                        brain.PatternIndex += 1;
                    }
                    //获取行为的效果列表
                    brain.Effects = pattern.Effects;
                    sprite = repeatPattern.Sprite;
                }
                else if (pattern is RandomPattern randomPattern)
                {
                    var effcts = new List<int>();
                    var index = 0;
                    //根据随机行为的效果列表的随机几率值probability.Value，
                    //将当前效果在Probabilities的下标index放入随机列表effcts，
                    //通过随机值，获取effcts的值取得将要使用的效果，实现随机
                    foreach (var probability in randomPattern.Probabilities)
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

                    //获取当前行为的图标
                    for(int i = 0;i < randomPattern.Effects.Count;i++) 
                    {
                        var effect = randomPattern.Effects[i];
                        if(effect == selectedEffct)
                        {
                            sprite = randomPattern.Probabilities[i].Sprite;
                            break;
                        }
                    }
                    brain.PatternIndex += 1;
                }
                var currentEffct = brain.Effects[0];
                if (currentEffct != null)
                {
                    //通过事件将图标显示
                    intentEvents[enemyIndex].Raise(sprite, (currentEffct as IntegerEffect).Value);
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
