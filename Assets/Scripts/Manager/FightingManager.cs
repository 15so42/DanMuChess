using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Timers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.UI;

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
    
    //UIManager
    public GameManager gameManager;
    public UIManager uiManager;

    public void Init(GameManager gameManager)
    {
        PlaceInitChess();
        EventCenter.AddListener<string,int,string,string>(EnumEventType.OnDanMuReceived,OnDanMuReceived);
        gameStatus = GameStatus.WaitingJoin;

        nowTeam = redTeam;
        this.gameManager = gameManager;
        uiManager = gameManager.uiManager;
    }

    public void JoinGame(Player player)
    {
        if (players.Count < maxPlayerCount && !players.Contains(player))
        {
            players.Add(player);
            
            Debug.Log("玩家"+player.userName+"加入了游戏");
            TipsDialog.ShowDialog("玩家"+player.userName+"加入了游戏",null);
            //同步UI
            uiManager.OnPlayerJoined(player,this);
            
            //游戏开始判断
            if (players.Count == 2)
            {
                TipsDialog.ShowDialog("准备完毕，对局开始", () =>
                {
                    gameStatus = GameStatus.CountDownToFight;
                    CountDownDialog.ShowDialog(15);
                });
                
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
        if (text.Split(' ')[0] == "!加入"||text.Split(' ')[0] == "!加入游戏")
        {
            if (gameStatus == GameStatus.WaitingJoin)
            {
                var playerAccount = BiliUserInfoQuerier.Query(uid);
                if (playerAccount.code == 0)
                {
                    JoinGame(new Player(uid, userName, teams[players.Count], playerAccount.data.face,playerAccount.data.top_photo));
                }
                else
                {
                    TipsDialog.ShowDialog("读取用户信息ErrorCode:"+playerAccount.code,null);
                }
            }
        }
        
    }  
}

public class Player
{
    public int uid;
    public string userName;
    public PlayerTeam playerTeam;
    public string faceUrl;
    public string top_photo;
    public Player(int uid, string userName, PlayerTeam playerTeam,string faceUrl,string top_photo)
    {
        this.uid = uid;
        this.userName = userName;
        this.playerTeam = playerTeam;
        this.faceUrl = faceUrl;
        this.top_photo = top_photo;
    }
}