
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

/// <summary>
/// 非攻击类卡牌被选中和取消的动作行为
/// </summary>
public class CardSelectionNoArrow : CardSelectionBase
{
    private Vector3 originalCardPosition;
    private Quaternion originalCardRotation;
    private int originalCardSortingOder;

    private const float cardCancelAnimationTime = 0.2f;
    private const Ease cardAnimationEase = Ease.OutBack;

    private const float cardAboutToBePlayedOffsetY = 1.5f;//选择要打出的牌时，鼠标Y轴偏移量，达到时将牌打出
    private const float cardAnimationTime = 0.2f;//将要打出的牌动作时间
    [SerializeField] private BoxCollider2D cardArea;//打出的效果牌，在效果产生时，卡牌存放区域

    private bool isCardAboutToBePlayed;

    private void Update()
    {
        if (cardDisplayManager.isCardMove() || isCardAboutToBePlayed)
            return;

        if(Input.GetMouseButtonDown(0) && selecteCard == null)
        {
            DetectCardSelection();
        }
        else if (Input.GetMouseButtonDown(1))
        {
            CancelCardSelection();
        }
        if(selecteCard !=  null)
        {
            UpdateSelecteCard();
        }
    }

    /// <summary>
    /// 通过鼠标射线选中卡牌，并保存卡牌原来数据
    /// </summary>
    private void DetectCardSelection()
    {
        var mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);

        var hitInfo = Physics2D.Raycast(mousePosition, Vector3.forward, Mathf.Infinity, cardLayer);

        if(hitInfo.collider != null)
        {
            var card = hitInfo.collider.GetComponent<CardObject>();
            var cardTemplate = card.template;
            if (!CardUtils.CarHasTargetableEffect(cardTemplate))
            {
                selecteCard = hitInfo.collider.gameObject;
                originalCardPosition = selecteCard.transform.position;
                originalCardRotation = selecteCard.transform.rotation;
                originalCardSortingOder = selecteCard.GetComponent<SortingGroup>().sortingOrder;
            }
        }
    }

    /// <summary>
    /// 更新选中卡牌位置，跟随鼠标移动
    /// </summary>
    private void UpdateSelecteCard()
    {
        var mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;

        selecteCard.transform.position = mousePosition;
    }

    /// <summary>
    /// 取消选择的卡牌，让卡牌返回原来位置
    /// </summary>
    private void CancelCardSelection()
    {
       if(selecteCard != null)
        {
            var cardSeq = DOTween.Sequence();

            cardSeq.AppendCallback(() =>
            {
                selecteCard.transform.DOMove(originalCardPosition, cardCancelAnimationTime).SetEase(cardAnimationEase);
                selecteCard.transform.DORotate(originalCardRotation.eulerAngles, cardCancelAnimationTime);
            });

            cardSeq.OnComplete(() =>
            {
                selecteCard.GetComponent<SortingGroup>().sortingOrder = originalCardSortingOder;
                selecteCard = null;
            });
            
        }
    }
}
