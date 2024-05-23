using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterObject : MonoBehaviour
{
    public CharacterTemplate Template;
    public RuntimeCharacter Character;

    public void OnCharacterDeid()
    {
        if(Character.Hp.Value <= 0)
        {
            GetComponent<BoxCollider2D>().enabled = false;
            var childObjects = transform.childCount;
            for(int i = childObjects - 1; i >= 0; i--)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
}
