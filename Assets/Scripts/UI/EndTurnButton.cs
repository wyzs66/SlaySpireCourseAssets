using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 结束玩家回合
/// </summary>
public class EndTurnButton : MonoBehaviour
{
    public Button EndButton;

    private CardDisplayManager cardDisplayManager;
    private CardSelectionHasArrow cardSelectionHasArrow;
    private CardSelectionNoArrow cardSelectionNoArrow;
    private TurnManager turnManager;

    private void Start()
    {
        cardDisplayManager = FindFirstObjectByType<CardDisplayManager>();
        cardSelectionHasArrow = FindFirstObjectByType<CardSelectionHasArrow>();
        cardSelectionNoArrow = FindFirstObjectByType<CardSelectionNoArrow>();
    }

    public void OnCilckEndTurn()
    {
        if (cardDisplayManager.isCardMove()) return;

        if (cardSelectionHasArrow.isSelectedCard() || cardSelectionNoArrow.isSelectedCard()) return;

        turnManager = FindFirstObjectByType<TurnManager>();
        turnManager.EndPlayerTurn();

        EndButton.interactable = false;
    }

    public void OnPlayerTurn()
    {
        EndButton.interactable = true;
    }
}
