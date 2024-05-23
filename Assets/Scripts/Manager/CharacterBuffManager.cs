using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBuffManager : BaseManager
{
    public void OnPlayerTurnEnd()
    {
        var playerStatus = Player.Character.Status;
        if(playerStatus.Value.Count > 0)
        {
            var statusNames = new List<string>();
            foreach(var status  in playerStatus.Value)
            {
                statusNames.Add(status.Key);
            }
            for(int i = 0; i < statusNames.Count; i++)
            {
                var value = playerStatus.GetValue(statusNames[i]);
                if (value > 0)
                    playerStatus.SetValue(playerStatus.Template[statusNames[i]], value - 1);
            }
        }
    }
}
