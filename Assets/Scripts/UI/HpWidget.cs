using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Ѫ��UI��ʾ
/// </summary>
public class HpWidget : MonoBehaviour
{
    [SerializeField] private Image hpBar;
    [SerializeField] private Image hpBarBackGround;
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private TextMeshProUGUI hpBorderText;
    [SerializeField] private GameObject shiledGroup;
    [SerializeField] private TextMeshProUGUI shiledText;
    [SerializeField] private TextMeshProUGUI shiledBorderText;
    

    private int maxValue;

    public void Initialize(IntVariable hp, int max, IntVariable shiled)
    {
        maxValue = max;
        SetHp(hp.Value);
        SetShield(shiled.Value);
    }

    /// <summary>
    /// ���ö�����ֵ
    /// </summary>
    /// <param name="value">��ֵ��С</param>
    private void SetShield(int value)
    {
        shiledText.text = $"{value.ToString()}";
        shiledBorderText.text = $"{value.ToString()}";
        SetShiledActive(value > 0);
    }

    private void SetShiledActive(bool v)
    {
        shiledGroup.SetActive(v);
    }

    /// <summary>
    /// ��������ֵ
    /// </summary>
    /// <param name="hp"></param>
    private void SetHp(int hp)
    {
        var newHp = hp / (float)maxValue;
        hpBar.DOFillAmount(newHp, 0.2F).SetEase(Ease.InSine);

        var seq = DOTween.Sequence();
        seq.AppendInterval(0.5f);
        seq.Append(hpBarBackGround.DOFillAmount(newHp, 0.2F));
        seq.SetEase(Ease.InSine);

        hpText.text = $"{hp.ToString()} / {maxValue.ToString()}";
        hpBorderText.text = $"{hp.ToString()} / {maxValue.ToString()}";
    }

    public void OnChangedHp(int value)
    {
        SetHp(value);
        if(value <= 0)
        {
            gameObject.SetActive(false);
        }
    }
    public void OnChangedShield(int value)
    {
        SetShield(value);
    }
}
