using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���˵�AI�߼�����
/// </summary>
public class EnemyAI
{
    public CharacterObject Enemy;

    public int PatternIndex; //����ָ��AIҪʹ���ĸ���Ϊ�߼�

    public List<Effect> Effects;//��ΪЧ���б�

    public EnemyAI(CharacterObject enemy)
    {
        Enemy = enemy;
    }
}
