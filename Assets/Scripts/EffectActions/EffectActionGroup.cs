using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �����Ӿ�Ч������
/// </summary>
[CreateAssetMenu(fileName ="Action group", menuName = "CardGame/Actions/Action group", order = 6)]
public class EffectActionGroup : ScriptableObject
{
    public List<EffectAction> Actions = new List<EffectAction>();
}
