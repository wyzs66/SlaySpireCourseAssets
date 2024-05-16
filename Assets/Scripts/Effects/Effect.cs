using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����Ч���Ļ���
/// </summary>
public abstract class Effect : ScriptableObject
{
    public List<EffectActionGroupManager> SourceActions = new List<EffectActionGroupManager>();//�����ߵĶ���Ч��
    public List<EffectActionGroupManager> TargetActions = new List<EffectActionGroupManager>();//Ŀ��Ķ���Ч��
    public abstract string GetName();
}
