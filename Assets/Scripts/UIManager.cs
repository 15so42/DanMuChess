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
        accountUis[fightingManager.players.Count].OnPlayerJoined(player);
    }
}
