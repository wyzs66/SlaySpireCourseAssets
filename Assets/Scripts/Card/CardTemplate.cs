using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 卡牌模版
/// </summary>
[CreateAssetMenu(fileName = "Card", menuName = "CardGame/Templates/Card", order = 0)]
public class CardTemplate : ScriptableObject
{
    public int Id;
    public string Name;
    public int Cost;
    public Sprite Picture;
    public CardType Type;//卡牌类型
    public List<Effect> Effects = new List<Effect>();//卡牌效果
}
