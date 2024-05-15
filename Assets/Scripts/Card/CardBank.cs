using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���ƿ⣺���Զ����������Щ���͵Ŀ��ƣ�ÿ�ֿ�������
/// </summary>
[Serializable]
[CreateAssetMenu(fileName = "CardBank", menuName = "CardGame/Templates/CaedBank", order = 3)]
public class CardBank : ScriptableObject
{
    public string Name;
    public List<CardBankItem> Items = new List<CardBankItem>();
}
