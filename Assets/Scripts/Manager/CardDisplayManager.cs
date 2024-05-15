using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

/// <summary>
/// 显示卡牌管理器：将抽到的手牌显示出来，并计算显示位置，旋转方向
/// </summary>
public class CardDisplayManager : MonoBehaviour
{
    private const int PositionNumber = 20;
    private const int RotationNumber = 20;
    private const int SortingOdersNumber = 20;

    private CardManager _cardManager;

    private  List<Vector3> _positions;
    private  List<Quaternion> _rotations;
    private  List<int> _sortingOders;

    private readonly List<GameObject> _handCards = new (PositionNumber);//显示手牌列表
    private const float Radius = 16.0f; //旋转半径

    private readonly Vector3 _center = new (-15.0f, -18.5f, 0.0f);//中心点
    private readonly Vector3 _originalCardScale = new (0.5f, 0.5f, 1.0f);//卡牌原始缩放比例

    public bool isCardMoving = false;
    private float cardToDisCardPileAnimationTime = 0.3f;

    public void Initialize(CardManager cardManager)
    {
        _cardManager = cardManager;
    }

    private void Awake()
    {
        _positions = new(PositionNumber);
        _rotations = new(RotationNumber);
        _sortingOders = new(SortingOdersNumber);
    }

    /// <summary>
    /// 创建手牌的实例
    /// </summary>
    /// <param name="cardsInHand"></param>
    public void CreateHandCards(List<RuntimeCard> cardsInHand)
    {
        var drawCards = new List<GameObject>(cardsInHand.Count);
        foreach(var  card in cardsInHand)
        {
            var cardGameObject = CreateCardGameObject(card);
            _handCards.Add(cardGameObject);
            drawCards.Add(cardGameObject);
        }
        PutDeckCardsToHands(drawCards);
    }

    private GameObject CreateCardGameObject(RuntimeCard card)
    {
        var gameObject = _cardManager.GetCardObject();
        var cardObject = gameObject.GetComponent<CardObject>();
        cardObject.SetInfo(card);

        gameObject.transform.position = Vector3.zero;
        gameObject.transform.localScale = Vector3.zero;
       
        return gameObject;
    }

    /// <summary>
    /// 卡牌刚创建时的移动
    /// </summary>
    /// <param name="drawCards"></param>
    private void PutDeckCardsToHands(List<GameObject> drawCards)
    {
        isCardMoving = true;

        OrganizeHandCards();

        var interval = 0.0f;

        for(var i = 0; i < drawCards.Count; i++)
        {
            var j = i;
            const float time = 0.5f;
            var card = drawCards[i];

            if(drawCards.Contains(card))
            {
                var cardObject = card.GetComponent<CardObject>();

                var seq = DOTween.Sequence();
                seq.AppendInterval(interval);
                seq.AppendCallback(() =>
                {
                    var move = card.transform.DOMove(_positions[j], time).OnComplete(() =>
                    {
                        cardObject.SaveTranform(_positions[j], _rotations[j]);
                    });

                    card.transform.DORotateQuaternion(_rotations[j], time);
                    card.transform.DOScale(_originalCardScale, time);

                    if(j == drawCards.Count - 1)
                    {
                        move.OnComplete(() =>
                        {
                            isCardMoving = false;
                            cardObject.SaveTranform(_positions[j], _rotations[j]);
                        });
                    }
                });
            }
            card.GetComponent<SortingGroup>().sortingOrder = _sortingOders[i];
            interval += 0.2f;
        }

    }
    /// <summary>
    /// 计算卡牌的旋转方向，位置，叠放顺序
    /// </summary>
    private void OrganizeHandCards()
    {
        _positions.Clear();
        _rotations.Clear();
        _sortingOders.Clear();

        const float angle = 5.0f;
        var cardAngle = (_handCards.Count - 1) * angle / 2;
        var z = 0.0f;

        for(int i = 0; i < _handCards.Count; i++)
        {
            //rotate
            var rotation = Quaternion.Euler(0, 0, cardAngle - i * angle);
            _rotations.Add(rotation);

            //position
            z -= 0.1f;
            var position = CalculateCardPosition(cardAngle - i * angle);
            position.z = z;
            _positions.Add(position);

            //卡牌叠放顺序，后抽取的在上方
            _sortingOders.Add(i);
        }
    }

    /// <summary>
    /// 通过初始位置计算位移位置
    /// </summary>
    /// <param name="angle"></param>
    /// <returns></returns>
    private Vector3 CalculateCardPosition(float angle)
    {
        return new Vector3(
            _center.x - Radius * Mathf.Sin(Mathf.Deg2Rad * angle),
            _center.y + Radius * Mathf.Cos(Mathf.Deg2Rad * angle),
            0.0f
            );
    }

    public bool isCardMove()
    {
        return isCardMoving;
    }

    /// <summary>
    /// 用掉卡牌后对剩下的牌重新排序
    /// </summary>
    /// <param name="selecteCard"></param>
    public void ReOrganizeHandCards(GameObject selecteCard)
    {
        _handCards.Remove(selecteCard);

        //卡牌位置数据重新调整
        OrganizeHandCards();

        // 卡牌调整位置动画
        for(int i = 0; i < _handCards.Count; i++) 
        {
            var card = _handCards[i];
            const float time = 0.3f;
            card.transform.DOMove(_positions[i], time);
            card.transform.DORotateQuaternion(_rotations[i], time);
            card.GetComponent<SortingGroup>().sortingOrder = _sortingOders[i];
            card.GetComponent<CardObject>().SaveTranform(_positions[i], _rotations[i]);

        }
    }

    public void MoveCardToDisCardPile(GameObject gameObject)
    {
        var seq = DOTween.Sequence();
        seq.AppendCallback(() =>
        {
            gameObject.transform.DOScale(Vector3.zero, cardToDisCardPileAnimationTime).OnComplete(() =>
            {
                gameObject.GetComponent<CardManager.ManagerPooleObject>().cardManager.ReturnObject(gameObject);
            });
        });
        seq.AppendCallback(() =>
        {
            _handCards.Remove(gameObject);
        });
    }
}
