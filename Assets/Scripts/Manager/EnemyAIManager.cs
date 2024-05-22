using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����AI�߼����������������ͷţ������ظ��ͷŵ�Ч��
/// </summary>
public class EnemyAIManager : BaseManager
{
    [SerializeField] private EffectResolutionManager effectResolutionManager;
    [SerializeField] private List<IntentChangeEvent> intentEvents;

    private int currentRepeatCount;//��ǰ�ظ���Ϊ��λ��
    private List<EnemyAI> brains;//���е��˵�AI�б�
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
    /// ����һغϿ�ʼʱ��������һ�غϵ��˵Ķ���
    /// </summary>
    public void OnPlayerTurnBegin()
    {
        const int enemyIndex = 0;
        //�������е���
        foreach(var enemy in Enemies)
        {
            //��ȡ����ģ�棬ȡ����������
            var template = enemy.Template as EnemyTemplate;
            //��ǰ���˵�AI�߼�
            var brain = brains[enemyIndex];

            if (template != null)
            {
                //�жϵ�ǰ��Ϊ���Ƿ��������Ϊ��
                if(brain.PatternIndex >= template.patterns.Count)
                {
                    brain.PatternIndex = 0;
                }
                Sprite sprite = null;
                //��ȡ��ǰ�����Ϊ
                var pattern = template.patterns[brain.PatternIndex];
                //�ж���Ϊ���������Ϊ�����ظ���Ϊ
                if(pattern is RepeatPattern repeatPattern)
                {
                    currentRepeatCount += 1;
                    //�����ǰ�ظ���Ϊ�Ĵ�����������ظ��������������һ����Ϊ�������ظ�����
                    if(currentRepeatCount == repeatPattern.Times)
                    {
                        currentRepeatCount = 0;
                        brain.PatternIndex += 1;
                    }
                    //��ȡ��Ϊ��Ч���б�
                    brain.Effects = pattern.Effects;
                    sprite = repeatPattern.Sprite;
                }
                else if (pattern is RandomPattern randomPattern)
                {
                    var effcts = new List<int>();
                    var index = 0;
                    //���������Ϊ��Ч���б���������ֵprobability.Value��
                    //����ǰЧ����Probabilities���±�index��������б�effcts��
                    //ͨ�����ֵ����ȡeffcts��ֵȡ�ý�Ҫʹ�õ�Ч����ʵ�����
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

                    //��ȡ��ǰ��Ϊ��ͼ��
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
                    //ͨ���¼���ͼ����ʾ
                    intentEvents[enemyIndex].Raise(sprite, (currentEffct as IntegerEffect).Value);
                }
            }
        }
    }

    /// <summary>
    /// ���˻غ�ʱ���ͷ���Ϊ
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
