
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

/// <summary>
/// �ǹ����࿨�Ʊ�ѡ�к�ȡ���Ķ�����Ϊ
/// </summary>
public class CardSelectionNoArrow : CardSelectionBase
{
    private Vector3 originalCardPosition;
    private Quaternion originalCardRotation;
    private int originalCardSortingOder;

    private const float cardCancelAnimationTime = 0.2f;
    private const Ease cardAnimationEase = Ease.OutBack;

    private const float cardAboutToBePlayedOffsetY = 1.5f;//ѡ��Ҫ�������ʱ�����Y��ƫ�������ﵽʱ���ƴ��
    private const float cardAnimationTime = 0.2f;//��Ҫ������ƶ���ʱ��
    [SerializeField] private BoxCollider2D cardArea;//�����Ч���ƣ���Ч������ʱ�����ƴ������

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
    /// ͨ���������ѡ�п��ƣ������濨��ԭ������
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
    /// ����ѡ�п���λ�ã���������ƶ�
    /// </summary>
    private void UpdateSelecteCard()
    {
        var mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;

        selecteCard.transform.position = mousePosition;
    }

    /// <summary>
    /// ȡ��ѡ��Ŀ��ƣ��ÿ��Ʒ���ԭ��λ��
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
