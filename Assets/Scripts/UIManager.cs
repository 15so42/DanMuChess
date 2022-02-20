using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [HideInInspector]
    public GameManager gameManager;

    public List<AccountUI> accountUis=new List<AccountUI>();
    
    public void Init(GameManager gameManager)
    {
        this.gameManager = gameManager;
    }

    public void OnPlayerJoined(Player player,FightingManager fightingManager)
    {
        accountUis[fightingManager.players.Count-1].OnPlayerJoined(player);
    }

    public void ShowMessage(int uid, string message)
    {
        accountUis.Find(x => x.player.uid == uid).ShowMessage(message);
    }

    public void StartNewRound(int uid)
    {
        accountUis.Find(x => x.player.uid == uid).StartNewRound();
    }

    public void RoundOver(int uid)
    {
        accountUis.Find(x => x.player.uid == uid).RoundOver();
    }
    
    
}
