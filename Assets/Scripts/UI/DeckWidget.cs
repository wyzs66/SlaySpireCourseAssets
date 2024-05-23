using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DeckWidget : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;

    private int deckSize;

    public void SetAmount(int amount)
    {
        deckSize = amount;
        text.text = deckSize.ToString();
    }

    public void RemoveCard()
    {
        SetAmount(deckSize - 1);
    }
}
