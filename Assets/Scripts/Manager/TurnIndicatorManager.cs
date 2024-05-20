using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// 显示当前是谁的回合UI
/// </summary>
public class TurnIndicatorManager : MonoBehaviour
{
    [SerializeField] private GameObject turnIndicator;
    [SerializeField] private TextMeshProUGUI text;

    private const float InAnimationTime = 0.5f;
    private const float OutAnimationTime = 0.6f;
    private const float PauseAnimationTime = 2.0f;

    public void OnPlayerIndicator()
    {
        text.text = "Player Turn";
        var seq = DOTween.Sequence();
        seq.Append(turnIndicator.GetComponent<RectTransform>().DOScale(1.0f, InAnimationTime));
        seq.AppendInterval(PauseAnimationTime);
        seq.Append(turnIndicator.GetComponent<RectTransform>().DOScale(0.0f, OutAnimationTime));
    }

    public void OnEnemyIndicator()
    {
        text.text = "Enemy Turn";
        var seq = DOTween.Sequence();
        seq.Append(turnIndicator.GetComponent<RectTransform>().DOScale(1.0f, InAnimationTime));
        seq.AppendInterval(PauseAnimationTime);
        seq.Append(turnIndicator.GetComponent<RectTransform>().DOScale(0.0f, OutAnimationTime));
    }
}
