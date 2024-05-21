using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 随机行为逻辑类
/// </summary>
[CreateAssetMenu(fileName = "RandomPattern", menuName = "CardGame/Patterns/Random Pattern", order =0)]
[Serializable]
public class RandomPattern : Pattern
{
    public List<Probability> Probabilities = new List<Probability>(4);
    public override string GetName()
    {
        return "Random Pattern";
    }
}
