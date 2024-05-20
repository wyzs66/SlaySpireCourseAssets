using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CardSelectionHasArrow : CardSelectionBase
{
    [SerializeField] private Vector3 previousClickPosition; //鼠标点击卡牌时位置
    private const float CardDetectionOffset = 2.2f; //当选择卡牌后，鼠标的偏移量，当达到这个值卡牌才进入动画
    private const float CardAnimationTime = 0.2f; //卡牌执行动画的时间

    private const float SelectedCardOffset = -1.0f; //被选择卡牌的偏移量
    private const float AttackCardInMiddlePositionX = -15.0f; //选择的卡牌位移到中心的位置

    private AttackArrow _attackArrow;
    private bool _isArrowCreated;

    private GameObject _selectedEnemy;

    protected override void Start()
    {
        base.Start();
        _attackArrow = FindAnyObjectByType<AttackArrow>();
    }

    private void Update()
    {
        if (cardDisplayManager.isCardMove())
            return;

        if (Input.GetMouseButtonDown(0) && selecteCard == null)
        {
            DetectCardSelection();
            DetectEnemySelection();
        }
        else if(Input.GetMouseButtonUp(0))
        {
            DetectEnemySelection();
        }
        else if (Input.GetMouseButtonDown(1))
        {
            CancelCardSelection();
        }

        if (selecteCard != null)
        {
            UpdateCardAndTargetingArrow();

        }
    }


    private void DetectEnemySelection()
    {
        if (selecteCard == null) return;
        var mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        var hitInfo = Physics2D.Raycast(mousePosition, Vector3.forward, Mathf.Infinity, enemyLayer);

        if(hitInfo.collider != null)
        {
            _selectedEnemy = hitInfo.collider.gameObject;
            PlaySelectedCard();
            selecteCard = null;
            _isArrowCreated = false;
            _attackArrow.ArrowEnable(false);
        }
    }

    /// <summary>
    ///  通过鼠标射线选中卡牌，更新排序层，保存原来位置
    /// </summary>
    private void DetectCardSelection()
    {
        var mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);

        var hitInfo = Physics2D.Raycast(mousePosition, Vector3.forward, Mathf.Infinity, cardLayer);

        if (hitInfo.collider != null)
        {
            var card = hitInfo.collider.GetComponent<CardObject>();
            var cardTemplate = card.template;
            if (CardUtils.CarHasTargetableEffect(cardTemplate))
            {
                selecteCard = hitInfo.collider.gameObject;
                selecteCard.GetComponent<SortingGroup>().sortingOrder += 10;
                previousClickPosition = mousePosition;
            }  
        }
    }

    /// <summary>
    /// 取消选择的卡牌，让卡牌返回原来位置
    /// </summary>
    private void CancelCardSelection()
    {
        if(selecteCard != null)
        {
            var cardObject = selecteCard.GetComponent<CardObject>();
            selecteCard.transform.DOKill();
            cardObject.Reset(() =>
            {
                _isArrowCreated = false;
                selecteCard = null;
            });

            _attackArrow.ArrowEnable(false);
        }
    }

    /// <summary>
    /// 处理卡牌释放后的动画，并将卡牌的效果实现
    /// </summary>
    protected override void PlaySelectedCard()
    {
        base.PlaySelectedCard();
        var car = selecteCard.GetComponent<CardObject>().runtimeCard;
        effectResolutionManager.ResolveCardEffects(car, _selectedEnemy.GetComponent<CharacterObject>());
    }

    /// <summary>
    /// 选择攻击卡牌后，将卡牌位移到中心位置
    /// </summary>
    private void UpdateCardAndTargetingArrow()
    {
        var mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);

        var diffY = mousePosition.y - previousClickPosition.y;

        if (!_isArrowCreated && diffY < CardDetectionOffset)
        {
            _isArrowCreated = true;
            var position = selecteCard.transform.position;

            selecteCard.transform.DOKill();

            var suq = DOTween.Sequence();
            suq.AppendCallback(() =>
            {
                selecteCard.transform.DOMove(new Vector3(AttackCardInMiddlePositionX, SelectedCardOffset, position.z), CardAnimationTime);
                selecteCard.transform.DORotate(Vector3.zero, CardAnimationTime);
            }).OnComplete(() =>
            {
                _attackArrow.ArrowEnable(true);
            });
        }
    }

    public GameObject GetSelectedEnemy()
    {
        return _selectedEnemy;
    }

    public bool isSelectedCard()
    {
        return selecteCard != null;
    }
}
