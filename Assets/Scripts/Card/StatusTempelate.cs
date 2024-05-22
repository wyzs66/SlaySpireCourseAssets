using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName ="StatusTemplate", menuName = "CardGame/Templates/Status", order = 3)]
public class StatusTempelate : ScriptableObject
{
    public string Name;
    public Sprite Sprite;
}
