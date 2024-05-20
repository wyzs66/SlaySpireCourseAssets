using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 回合制管理器
/// </summary>
public class TurnManager : MonoBehaviour
{
    public GameEvent PlayerTurnBegin;
    public GameEvent PlayerTurnEnded;
    public GameEvent EnemyTurnBegin;
    public GameEvent EnemyTurnEnded;

    private bool isEnemyTurn;
    private float timer;
    private bool isEndOfGame;
    private const float enemyTurnDuration = 3.0f;

    private void Update()
    {
        if (isEnemyTurn)
        {
            timer += Time.deltaTime;
            if(timer > enemyTurnDuration)
            {
                timer = 0.0f;
                EndEnemyTurn();
            }
        }
    }

    public void BeginGame()
    {
        BeginPlayerTurn();
    }

    public void BeginPlayerTurn()
    {
        PlayerTurnBegin.Raise();
        Debug.Log("Began Player Turn");

    }

    public void EndPlayerTurn()
    {
        PlayerTurnEnded.Raise();
        BeginEnemyTurn();
        Debug.Log("End Player Turn");

    }

    public void BeginEnemyTurn()
    {
        EnemyTurnBegin.Raise();
        isEnemyTurn = true;
        Debug.Log("Began Enemy Turn");
    }

    public void EndEnemyTurn()
    {
        EnemyTurnEnded.Raise();
        BeginPlayerTurn();
        isEnemyTurn = false;
        Debug.Log("End Enemy Turn");
    }

    public void SetEndOfGame(bool value)
    {
        isEndOfGame = value;
    }
    public bool IsEndOfGame()
    {
        return isEndOfGame;
    }
}
