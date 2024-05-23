using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��ɫħ��ֵ������
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
