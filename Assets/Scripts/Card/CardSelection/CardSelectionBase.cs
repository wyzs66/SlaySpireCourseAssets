using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ѡ���ƻ���
/// </summary>
public abstract class CardSelectionBase : BaseManager
{
    protected Camera mainCamera;
    public LayerMask cardLayer;
    public LayerMask enemyLayer;

    public CardDisplayManager cardDisplayManager;
    public EffectResolutionManager effectResolutionManager;

    protected GameObject selecteCard;
    

    protected virtual void Start()
    {
        mainCamera = Camera.main;
    }

    /// <summary>
    /// ��ʹ�õ��Ŀ���������
    /// </summary>
    protected virtual void PlaySelectedCard()
    {
        cardDisplayManager.ReOrganizeHandCards(selecteCard);
        cardDisplayManager.MoveCardToDisCardPile(selecteCard);
    }
}
