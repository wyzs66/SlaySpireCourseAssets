using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// �ظ���Ϊ�߼���
/// </summary>
[CreateAssetMenu(fileName = "RepeatPattern", menuName = "CardGame/Patterns/Repeat Pattern", order = 1)]
[Serializable]
public class RepeatPattern : Pattern
{
    public int Times;//�ظ�����
    public Sprite Sprite;
    public override string GetName()
    {
        return $"Repeat x {Times.ToString()}";
    }
}
