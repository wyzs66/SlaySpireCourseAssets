using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 人物产生随机抖动的效果
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
