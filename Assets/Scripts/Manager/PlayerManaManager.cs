using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 角色魔法值管理器
/// </summary>
public class PlayerManaManager : MonoBehaviour
{
    public IntVariable playerManaVariable;
    private int defaultMana = 3;

    public void SetDefaultMana(int mana)
    {
        defaultMana = mana;
    }

    public void OnPlayerTurnBegan()
    {
        playerManaVariable.SetValue(defaultMana);
    }

}
