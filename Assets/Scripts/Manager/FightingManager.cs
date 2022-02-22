using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
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
    WaitingNewFighting,//对局结束，有玩家胜利，开始倒计时，倒计时结束后进入WaitingJoin状态
}

public class FightingManager : MonoBehaviour
{
    public List<PlayerTeam> teams = new List<PlayerTeam>();

    public List<Player> players=new List<Player>();
    public int maxPlayerCount = 2;
    
    //对局状态
    public GameStatus gameStatus = GameStatus.Init;
    
    
    //UIManager
    public GameManager gameManager;
    public UIManager uiManager;
    //Round Manager
    public RoundManager roundManager;

    [Header("回合时间")] public int roundDuration=45;
    [Header("最大允许挂机时间")] public int kickOutTime = 120;

    public void Init(GameManager gameManager)
    {
        PlaceInitChess();
        EventCenter.AddListener<string,int,string,string>(EnumEventType.OnDanMuReceived,OnDanMuReceived);
        gameStatus = GameStatus.WaitingJoin;

        
        this.gameManager = gameManager;
        uiManager = gameManager.uiManager;
        
    }
    
    public Dictionary<int,float> activeTimeTable=new Dictionary<int, float>(){};

    public void JoinGame(Player player)
    {
        if (players.Count < maxPlayerCount && !players.Contains(player) && players.Find(x=>x.uid==player.uid)==null )
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
                    CountDownDialog.ShowDialog(3, () =>
                    {
                        TipsDialog.ShowDialog("红方先手",null);
                        gameStatus = GameStatus.Playing;
                        roundManager=new RoundManager();
                        roundManager.Init(gameManager,players);
                        
                        //两个玩家更新活跃表
                        foreach (var p in players)
                        {
                            activeTimeTable.Add(player.uid,Time.time);
                        }
                    });
                });
                
            }
        }
    }

    Player GetPlayerByUid(int uid)
    {
        return players.Find(x => x.uid == uid);
    }
    
    public Player FindWinnerPlayer(PlayerTeam loseTeam)//通过输家来找到赢家
    {
        PlayerTeam winnerTeam= teams.Find(x => x != loseTeam);
        Player winnerPlayer = players.Find(x => x.playerTeam == winnerTeam);
        return winnerPlayer;
    }

    private void Update()
    {
        for (int i=0;i<activeTimeTable.Count;i++)
        {
            var kv = activeTimeTable.ElementAt(i);
            if (Time.time - kv.Value > kickOutTime)
            {
                var player = GetPlayerByUid(kv.Key);
                if (player != null)
                {
                    Debug.Log(player.userName+"长时间未操作，踢出");
                    TipsDialog.ShowDialog(player.userName+"长时间未操作，踢出", () =>
                    {
                        var winner = FindWinnerPlayer(player.playerTeam);
                        BattleOver(winner);
                    });
                   
                }
               
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
    /// <summary>
    /// 对局结束
    /// </summary>
    /// <param name="winner"></param>
    public void BattleOver(Player winner)
    {
        roundManager.Stop();
        activeTimeTable.Clear();
        
        gameStatus =  GameStatus.WaitingNewFighting;
        BattleOverDialog.ShowDialog(15,winner, () =>
        {
            StartNewBattle();
        });
    }

    public void StartNewBattle()
    {
        players.Clear();
        uiManager.ResetUi();
        roundManager = null;
        gameManager.chessMoveManager.ReInit();
        PlaceInitChess();

        gameStatus = GameStatus.WaitingJoin;
        TipsDialog.ShowDialog("✪ ω ✪棋盘初始化完成,输入加入游戏即可游玩",null);

    }
    public void OnChessBoardClick(PlayerTeam playerTeam,Vector2 gridPos)
    {
        
    }

    public void UpdateLastActiveTime(int uid, float time)
    {
        activeTimeTable[uid] = time;
    }

    private void OnDanMuReceived(string userName,int uid,string time,string text )
    {
        //找到队伍
        if (text.Split(' ')[0] == "加入"||text.Split(' ')[0] == "加入游戏")
        {
            if (gameStatus == GameStatus.WaitingJoin)
            {
                var playerAccount = BiliUserInfoQuerier.Query(uid);
                if (playerAccount.code == 0 && players.Count<maxPlayerCount)
                {
                    JoinGame(new Player(uid, userName, teams[players.Count], playerAccount.data.face,playerAccount.data.top_photo));
                }
                else
                {
                    TipsDialog.ShowDialog("Error:"+playerAccount.code+","+playerAccount.message,null);
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