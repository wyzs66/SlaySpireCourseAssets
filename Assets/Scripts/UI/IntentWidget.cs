using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 显示敌人下一步行动的UI
/// </summary>
public class IntentWidget : MonoBehaviour
{
    [SerializeField] private Image intentImage;
    [SerializeField] private TextMeshProUGUI amountText;

    private const float InitialDelay = 1.25f;
    private const float FadeInDuration = 0.8f;
    private const float FadeOutDuration = 0.5f;

    public void OnEnemyTurnBegan()
    {
        var sequence = DOTween.Sequence();
        sequence.AppendInterval(InitialDelay);
        sequence.Append(intentImage.DOFade(0.0f, FadeOutDuration));

        sequence = DOTween.Sequence();
        sequence.AppendInterval(InitialDelay);
        sequence.Append(amountText.DOFade(0.0f, FadeOutDuration));
    }


    public void OnIntentChange(Sprite sprite, int value)
    {
        intentImage.sprite = sprite;
        amountText.text = value.ToString();
        intentImage.SetNativeSize();

        intentImage.DOFade(1.0f, FadeInDuration);
        amountText.DOFade(1.0f, FadeInDuration);

    }

    public void OnHpChange(int hp)
    {
        if(hp <= 0)
        {
            intentImage.DOFade(0.0f, FadeOutDuration);
            amountText.DOFade(0.0f, FadeOutDuration);
        }
    }
}
