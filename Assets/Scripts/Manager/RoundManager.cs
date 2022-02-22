using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityTimer;

public class RoundManager
{
    public const string MoveErrorMsg = "移动格式错误，应为: 移动 xx xx";
    
    public List<Player> players=new List<Player>();

    [SerializeField]
    private Player nowPlayer;
    
    //记录回合数
    public int roundCount = 0;

    private GameManager gameManager;
    private FightingManager fightingManager;
    private ChessMoveManager chessMoveManager;
    private UIManager uiManager;

    public UnityTimer.Timer timer;//回合计时器，在下棋后引起的回合结束时需要手动关闭相应timer,也要注意UI的timer
    
    public void Init(GameManager gameManager ,List<Player> players)
    {
        this.players = players;
        nowPlayer = players[0];
        this.gameManager = gameManager;
        this.fightingManager = gameManager.fightingManager;
        this.chessMoveManager = gameManager.chessMoveManager;
        this.uiManager = gameManager.uiManager;
        EventCenter.AddListener<string,int,string,string>(EnumEventType.OnDanMuReceived,OnDanMuReceived);
        StartFight();
    }

    Player GetPlayerByRound()//根据回合数决定当前活动队伍
    {
        return players[roundCount % players.Count];
    }

    public void StartFight()
    {
        
        StartNewRound();
        
    }

    void StartNewRound()
    {
        nowPlayer = GetPlayerByRound();
        uiManager.StartNewRound(nowPlayer.uid);
        timer=Timer.Register(fightingManager.roundDuration, () =>
        {
            RoundOver(nowPlayer.uid);//结束自己回合
            
        });
        
    }

    public void Stop()
    {
        timer.Cancel();
        foreach (var player in players)
        {
            uiManager.RoundOver(player.uid);
        }
        EventCenter.RemoveListener<string,int,string,string>(EnumEventType.OnDanMuReceived,OnDanMuReceived);
        
    }

    private void OnDanMuReceived(string userName,int uid,string time,string text)
    {
        ParseCommand(uid,text);
            
    }

    private void RoundOver(int uid)
    {
        roundCount++;
        uiManager.RoundOver(uid);
        timer.Cancel();
        StartNewRound();
    }
    
    

    //解析命令
    private void ParseCommand(int uid,string text)
    {
        if (nowPlayer.uid == uid)
        {
            if (text.StartsWith("移动 "))
            {
                var trim=Regex.Replace(text.Trim(), "\\s+", " ");
                var strArr = trim.Split(' ');
                if (strArr.Length == 3)
                {
                    var startPosStr = strArr[1];
                    var endPosStr = strArr[2];

                    if (String.IsNullOrEmpty(startPosStr) || String.IsNullOrEmpty(endPosStr))
                    {
                        uiManager.ShowMessage(uid, MoveErrorMsg);
                        return;
                    }

                    if (startPosStr.Length < 2 || endPosStr.Length < 2)
                    {
                        uiManager.ShowMessage(uid, MoveErrorMsg);
                        return;
                    }

                    var startX = Int32.Parse(startPosStr[0].ToString());
                    var startY = Int32.Parse(startPosStr[1].ToString());

                    var endX = Int32.Parse(endPosStr[0].ToString());
                    var endY = Int32.Parse(endPosStr[1].ToString());

                    if (startX < 0 || startX > 8 || startY < 0 || startY > 9 || endX < 0 || endX > 8 || endY < 0 ||
                        endY > 9)
                    {
                        uiManager.ShowMessage(uid, "超出棋盘范围，请重下");
                        return;
                    }

                    if (startX == endX && startY == endY)
                    {
                        uiManager.ShowMessage(uid, "目标位置不能和起始位置相同，请重下");
                        return;
                    }
                    
                    bool moveRet=chessMoveManager.MoveChessToPos(nowPlayer.playerTeam,new Vector2Int(startX, startY), new Vector2Int(endX, endY));
                    if(moveRet){
                        RoundOver(uid); //回合结束
                    }  
                    uiManager.ShowMessage(uid, text);
                   
                }
                else
                {
                    uiManager.ShowMessage(uid, text);
                }
                
            } else
            {
                uiManager.ShowMessage(uid, text);
            }
        }
        else
        {
            if(fightingManager.players.Find(x=>x.uid==uid)==null)//无视对局外弹幕
                return;
            uiManager.ShowMessage(uid, "[非己方回合]"+text);
        }
    }
}
