using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敌人的AI逻辑基类
/// </summary>
public class EnemyAI
{
    public CharacterObject Enemy;

    public int PatternIndex; //用来指明AI要使用哪个行为逻辑

    public List<Effect> Effects;//行为效果列表

    public EnemyAI(CharacterObject enemy)
    {
        Enemy = enemy;
    }
}
