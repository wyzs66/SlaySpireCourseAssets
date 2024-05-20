using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CardSelectionHasArrow : CardSelectionBase
{
    [SerializeField] private Vector3 previousClickPosition; //���������ʱλ��
    private const float CardDetectionOffset = 2.2f; //��ѡ���ƺ�����ƫ���������ﵽ���ֵ���ƲŽ��붯��
    private const float CardAnimationTime = 0.2f; //����ִ�ж�����ʱ��

    private const float SelectedCardOffset = -1.0f; //��ѡ���Ƶ�ƫ����
    private const float AttackCardInMiddlePositionX = -15.0f; //ѡ��Ŀ���λ�Ƶ����ĵ�λ��

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
    ///  ͨ���������ѡ�п��ƣ���������㣬����ԭ��λ��
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
    /// ȡ��ѡ��Ŀ��ƣ��ÿ��Ʒ���ԭ��λ��
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
    /// �������ͷź�Ķ������������Ƶ�Ч��ʵ��
    /// </summary>
    protected override void PlaySelectedCard()
    {
        base.PlaySelectedCard();
        var car = selecteCard.GetComponent<CardObject>().runtimeCard;
        effectResolutionManager.ResolveCardEffects(car, _selectedEnemy.GetComponent<CharacterObject>());
    }

    /// <summary>
    /// ѡ�񹥻����ƺ󣬽�����λ�Ƶ�����λ��
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
