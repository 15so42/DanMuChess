using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using UnityEngine.Events;

public enum GameStatus
{
    Init,//程序初始化状态
    WaitingJoin,//等待玩家加入
    CountDownToFight,//玩家加入完毕后倒计时
    Playing,//对局中
}

public class FightingManager : MonoBehaviour
{
    public List<PlayerTeam> teams = new List<PlayerTeam>();


    public PlayerTeam redTeam;
    public PlayerTeam blackTeam;
    
    public List<Player> players=new List<Player>();
    public int maxPlayerCount = 2;
    
    //对局状态
    public GameStatus gameStatus = GameStatus.Init;
    //正在下棋的选手
    [HideInInspector]
    public PlayerTeam nowTeam ;
    
    public void Init()
    {
        PlaceInitChess();
        EventCenter.AddListener<string,int,string,string>(EnumEventType.OnDanMuReceived,OnDanMuReceived);
        gameStatus = GameStatus.WaitingJoin;

        nowTeam = redTeam;
    }

    public void JoinGame(Player player)
    {
        if (players.Count < maxPlayerCount && !players.Contains(player))
        {
            players.Add(player);
            Debug.Log("玩家"+player.userName+"加入了游戏");
            //TipsDialog.ShowDialog("玩家"+player.userName+"加入了游戏");
            if (players.Count == 2)
            {
                TipsDialog.ShowDialog("准备完毕，对局开始");
                gameStatus = GameStatus.CountDownToFight;
                
                //CountDownDialog.ShowDialog(15);

            }
        }
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

    private void OnDanMuReceived(string userName,int uid,string time,string text )
    {
        //找到队伍
        if (text.Split(' ')[0] == "!加入")
        {
            if (gameStatus == GameStatus.WaitingJoin)
            { 
                JoinGame(new Player(uid,userName,teams[players.Count]));
            }
        }
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public class Player
{
    public int uid;
    public string userName;
    public PlayerTeam playerTeam;
    public Player(int uid, string userName, PlayerTeam playerTeam)
    {
        this.uid = uid;
        this.userName = userName;
        this.playerTeam = playerTeam;
    }
}