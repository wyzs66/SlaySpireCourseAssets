using System.Collections;
using System.Collections.Generic;
using TMPro;
using System.Text;
using UnityEngine.Rendering;
using DG.Tweening;
using UnityEngine;
using System;

/// <summary>
/// 单个卡牌，为卡牌元素导入数据
/// </summary>
public class CardObject : MonoBehaviour
{
    [SerializeField] private TextMeshPro coatText;
    [SerializeField] private TextMeshPro nameText;
    [SerializeField] private TextMeshPro typeText;
    [SerializeField] private TextMeshPro descriptionText;
    [SerializeField] private SpriteRenderer picture;

    public CardTemplate template;
    public RuntimeCard runtimeCard;

    private Vector3 _savePosition;
    private Quaternion _saveRotation;
    private int _sortingOder;
    private SortingGroup _sortingGroup;

    public enum CardState
    {
        InHand, //手牌中
        AboutToBePlayed //将要使用的卡牌
    }

    public CardState _curentState;
    public CardState State => _curentState;

    public void SetState(CardState state)
    {
        _curentState = state;
    }

    private void OnEnable()
    {
        SetState(CardState.InHand);
    }

    private void Awake()
    {
        _sortingGroup = gameObject.GetComponent<SortingGroup>();
    }

    private void Start()
    {
        var card = new RuntimeCard
        {
            Template = template,
        };
        SetInfo(card);
    }

    /// <summary>
    /// 写入卡牌元素数据
    /// </summary>
    /// <param name="card"></param>
    public void SetInfo(RuntimeCard card)
    {
        this.runtimeCard = card;
        template = card.Template;
        coatText.text = template.Cost.ToString();
        nameText.text = template.Name;
        typeText.text = template.Type.TypeName;
        var builder = new StringBuilder();
        descriptionText.text = builder.ToString();
        picture.sprite = template.Picture;
        
    }

    /// <summary>
    /// 保存初始位置和层级信息
    /// </summary>
    /// <param name="position"></param>
    /// <param name="rotation"></param>
    public void SaveTranform(Vector3 position, Quaternion rotation)
    {
        _savePosition = position;
        _saveRotation = rotation;
        _sortingOder = _sortingGroup.sortingOrder;
    }

    /// <summary>
    /// 回到初始位置
    /// </summary>
    /// <param name="onComplete"></param>
    public void Reset(Action onComplete)
    {
        transform.DOMove(_savePosition, 0.2f);
        transform.DORotateQuaternion(_saveRotation, 0.2f);
        _sortingGroup.sortingOrder = _sortingOder;
        onComplete();
    }
}
