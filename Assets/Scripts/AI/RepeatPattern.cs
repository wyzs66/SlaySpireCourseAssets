using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 重复行为逻辑类
/// </summary>
[CreateAssetMenu(fileName = "RepeatPattern", menuName = "CardGame/Patterns/Repeat Pattern", order = 1)]
[Serializable]
public class RepeatPattern : Pattern
{
    public int Times;//重复次数
    public Sprite Sprite;
    public override string GetName()
    {
        return $"Repeat x {Times.ToString()}";
    }
}
