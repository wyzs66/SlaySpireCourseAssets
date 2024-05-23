using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisCardWidget : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;

    private int disCardSize;

    public void SetAmount(int amount)
    {
        disCardSize = amount;
        text.text = disCardSize.ToString();
    }

    public void Addcard()
    {
        SetAmount(disCardSize + 1);
    }
}
