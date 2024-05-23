using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �ƶѹ�����
/// </summary>
public class CardDeckManager : MonoBehaviour
{
    private List<RuntimeCard> _deck; //���ƶ�
    private List<RuntimeCard> _disCardPile; //���ƶ�
    private List<RuntimeCard> _hand; //����

    private const int DeckCapacity = 30;//���ƶ�����
    private const int DisCardCapacity = 30;//���ƶ�����
    private const int HandCapacity = 30;//��������

    public CardDisplayManager _displayManager;

    private DeckWidget deckWidget;
    private DisCardWidget disCardWidget;

    private void Awake()
    {
        _deck = new List<RuntimeCard>(DeckCapacity);
        _disCardPile = new List<RuntimeCard>(DisCardCapacity);
        _hand = new List<RuntimeCard>(HandCapacity);
    }

    public void Initialize(DeckWidget deckWidget, DisCardWidget disCardWidget)
    {
        this.deckWidget = deckWidget;
        this.disCardWidget = disCardWidget;
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
        deckWidget.SetAmount(_deck.Count);
        disCardWidget.SetAmount(0);
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
                _hand.Add(card);
                drawCards.Add(card);
            }
            _displayManager.CreateHandCards(drawCards, previousDeckSize);
        }
        else
        {
            for(int i = 0; i < _disCardPile.Count; i++)
            {
                _deck.Add(_disCardPile[i]);
            }

            _disCardPile.Clear();

            _displayManager.UpdateDiscardSize(_disCardPile.Count);

            if(amount > _deck.Count + _disCardPile.Count)
            {
                amount = _deck.Count + _disCardPile.Count;
            }
            DrawCardsFormDeck(amount);
        }
    }

    public void MoveCardToDisCardPile(RuntimeCard card)
    {
        _hand.Remove(card);
        _disCardPile.Add(card);
    }

    public void MoveCardsToDisCardPile()
    {
        foreach(var  card in _hand)
        {
            _disCardPile.Add(card);
        }
        _hand.Clear();
    }
}
