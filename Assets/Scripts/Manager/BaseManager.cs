using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseManager : MonoBehaviour
{
    protected CharacterObject Player;
    protected List<CharacterObject> Enemies;

    public void Initialize(CharacterObject player, List<CharacterObject> enemies)
    {
        Player = player;
        Enemies = enemies;
    }
}
