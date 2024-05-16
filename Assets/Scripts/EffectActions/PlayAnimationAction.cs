using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 播放效果动画
/// </summary>
[CreateAssetMenu(fileName = "PlayAnimationAction", menuName = "CardGame/Actions/Play Animation Action", order = 9)]
public class PlayAnimationAction : EffectAction
{
    public Animator Animator;
    public Vector3 Offset;
    public override string GetName()
    {
        return "Play Animation";
    }

    public override void Execute(GameObject gameObject)
    {
        var position = gameObject.transform.position;
        var animator = Instantiate(Animator);
        animator.transform.position = position + Offset;

        var autoDestroy = animator.gameObject.AddComponent<AutoDestroy>();
        autoDestroy.Duration = 2.0f;
    }
}
