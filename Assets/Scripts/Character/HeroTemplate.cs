using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
[CreateAssetMenu(fileName = "Hero", menuName = "CardGame/Templates/Hero", order = 1)]
public class HeroTemplate : CharacterTemplate
{
    public CardBank StartDeck;
}
