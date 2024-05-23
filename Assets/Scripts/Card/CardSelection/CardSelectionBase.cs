using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 选择卡牌基类
/// </summary>
public abstract class CardSelectionBase : BaseManager
{
    protected Camera mainCamera;
    public LayerMask cardLayer;
    public LayerMask enemyLayer;

    public CardDisplayManager cardDisplayManager;
    public EffectResolutionManager effectResolutionManager;
    public CardDeckManager deckManager;

    protected GameObject selecteCard;
    public IntVariable playerMana;
    

    protected virtual void Start()
    {
        mainCamera = Camera.main;
    }

    /// <summary>
    /// 对使用掉的卡牌做处理
    /// </summary>
    protected virtual void PlaySelectedCard()
    {
        var card = selecteCard.GetComponent<CardObject>();
        var cardTemplate = card.template;
        playerMana.SetValue(playerMana.Value - cardTemplate.Cost);
        cardDisplayManager.ReOrganizeHandCards(selecteCard);
        cardDisplayManager.MoveCardToDisCardPile(selecteCard);
        deckManager.MoveCardToDisCardPile(card.runtimeCard);
    }
}
