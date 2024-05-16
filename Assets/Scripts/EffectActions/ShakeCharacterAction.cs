using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����������������Ч��
/// </summary>
[CreateAssetMenu(fileName = "ShakeCharacterAction", menuName = "CardGame/Actions/Shake Character Action", order = 10)]
public class ShakeCharacterAction : EffectAction
{
    public float Duration;
    public Vector3 Strength;

    public override string GetName()
    {
        return "Shake Character";
    }

    public override void Execute(GameObject gameObject)
    {
        gameObject.transform.DOShakePosition(Duration, Strength);
    }

    
}
