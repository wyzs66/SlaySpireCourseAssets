using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 播放粒子效果的动作类
/// </summary>
[CreateAssetMenu(fileName = "PlayParticlesAction", menuName = "CardGame/Actions/Play Particles Action", order = 8)]
public class PlayParticlesAction : EffectAction
{
    public Vector3 Offset;
    public ParticleSystem Particles;
    
    public override string GetName()
    {
        return "Play Particles";
    }

    public override void Execute(GameObject gameObject)
    {
        var Position = gameObject.transform.position;
        var particles = Instantiate(Particles);
        particles.transform.position = Position + Offset;
        particles.Play();

        var autoDestroy = particles.gameObject.AddComponent<AutoDestroy>();
        autoDestroy.Duration = 2.0f;

    }

}
