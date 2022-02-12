using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class FightingManager : MonoBehaviour
{
    public List<PlayerTeam> teams = new List<PlayerTeam>();
    // Start is called before the first frame update
    public void Init()
    {
        PlaceInitChess();
    }

    void PlaceInitChess()
    {
        foreach (var team in teams)
        {
            team.PlaceInitChess(GameManager.Instance);
        }
    }
    public void OnChessBoardClick(PlayerTeam playerTeam,Vector2 gridPos)
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
