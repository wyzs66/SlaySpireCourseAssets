using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 卡牌库：可以定义库中有哪些类型的卡牌，每种卡牌数量
/// </summary>
[Serializable]
[CreateAssetMenu(fileName = "CardBank", menuName = "CardGame/Templates/CaedBank", order = 3)]
public class CardBank : ScriptableObject
{
    public string Name;
    public List<CardBankItem> Items = new List<CardBankItem>();
}
