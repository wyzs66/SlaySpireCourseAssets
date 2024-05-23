using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ManaWidget : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private TextMeshProUGUI textBorder;

    private int maxMana;

    public void Initialize(IntVariable mana)
    {
        maxMana = mana.Value;
        SetValue(mana.Value);
    }
    public void SetValue(int value)
    {
        text.text = $"{value.ToString()}/{maxMana.ToString()}";
        textBorder.text = text.text;
    }

    public void OnManaChanged(int value)
    {
        SetValue(value);
    }
}
