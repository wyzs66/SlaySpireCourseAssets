using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �ƶѹ�����
/// </summary>
public class CardDeckManager : MonoBehaviour
{
    private List<RuntimeCard> _deck; //���ƶ�
    private const int DeckCapacity = 30;//���ƶ�����

    public CardDisplayManager _displayManager;

    private void Awake()
    {
        _deck = new List<RuntimeCard>(DeckCapacity);
    }

    /// <summary>
    /// �������ƶ�
    /// </summary>
    /// <param name="decks"></param>
    public  void LaodDeck(List<CardTemplate> decks)
    {
        var deckSize = 0;
        foreach (var template in decks)
        {
            if(template ==  null)
                continue;
            var card = new RuntimeCard
            {
                Template = template
            };

            _deck.Add(card);
            deckSize++;
        }
    }

    /// <summary>
    /// ϴ��
    /// </summary>
    public void DeckShuffle()
    {
        _deck.Shuffle();
    }

    /// <summary>
    /// ��ȡ����
    /// </summary>
    /// <param name="amount"></param>
    public void DrawCardsFormDeck(int amount)
    {
        var deckSize = _deck.Count;

        if(deckSize >= amount)
        {
            var previousDeckSize = deckSize;
            var drawCards = new List<RuntimeCard>(amount);
            for (int i = 0; i < amount; i++)
            {
                var card = _deck[0];
                _deck.RemoveAt(0);
                drawCards.Add(card);
            }
            _displayManager.CreateHandCards(drawCards);
        }
    }
}
