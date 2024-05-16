using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MoveCharacterAction", menuName = "CardGame/Actions/Move Character Action", order = 7)]
public class MoveCharacterAction : EffectAction
{
    public Vector3 Offset;
    public float Duration;

    public override string GetName()
    {
        return "Move Character";
    }

    public override void Execute(GameObject gameObject)
    {
        var originalPosition = gameObject.transform.position;
        var Seq = DOTween.Sequence();
        Seq.Append(gameObject.transform.DOMove(originalPosition + Offset, Duration));
        Seq.Append(gameObject.transform.DOMove(originalPosition , Duration * 2));
    }

    
}
