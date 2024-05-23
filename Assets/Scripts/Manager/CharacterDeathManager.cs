using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterDeathManager : BaseManager
{
    [SerializeField] private float endBattlePopupDeplay = 1.0f;
    [SerializeField] private EndBattlePopup endBattlePopup;

    public void OnPlayerHpChanged(int hp)
    {
        if(hp <= 0)
        {
            EndGame(true);
            Player.OnCharacterDeid();
        }
    }

    public void OnEnemyHpChanged(int hp)
    {
        if (hp <= 0)
        {
            Enemies[0].OnCharacterDeid();
            EndGame(false);  
        }
    }

    public void EndGame(bool v)
    {
        StartCoroutine(ShowEndBettle(v));
    }

    private IEnumerator ShowEndBettle(bool v)
    {
        //yield return new WaitForSeconds(0.2f);
        //Debug.Log("Game Over");
        yield return new WaitForSeconds(endBattlePopupDeplay);
        if(endBattlePopup != null )
        {
            endBattlePopup.Show();
            if(!v)
            {
                endBattlePopup.SetVictoryText();
            }
            else
            {
                endBattlePopup.SetDefeatText();
            }
            var turnmanager = FindFirstObjectByType<TurnManager>();
            turnmanager.SetEndOfGame(true);
        }
    }
}
