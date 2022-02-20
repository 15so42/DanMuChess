﻿using System;
using System.Collections;
using System.Collections.Generic;
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
        Timer.Register(fightingManager.roundDuration, () =>
        {
            RoundOver(nowPlayer.uid);//结束自己回合
            StartNewRound();
        });
        
    }

    private void OnDanMuReceived(string userName,int uid,string time,string text)
    {
        ParseCommand(uid,text);
            
    }

    private void RoundOver(int uid)
    {
        roundCount++;
        uiManager.RoundOver(uid);
    }

    //解析命令
    private void ParseCommand(int uid,string text)
    {
        if (nowPlayer.uid == uid)
        {
            if (text.StartsWith("移动 "))
            {
                var strArr = text.Split(' ');
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
                        uiManager.ShowMessage(uid, MoveErrorMsg);
                        return;
                    }

                    try
                    {
                        chessMoveManager.MoveChessToPos(new Vector2Int(startX, startY), new Vector2Int(endX, endY));
                        RoundOver(uid); //回合结束
                    }
                    catch (Exception e)
                    {
                        uiManager.ShowMessage(uid, MoveErrorMsg);
                        
                    }
                   
                }
                
            } else
            {
                uiManager.ShowMessage(uid, text);
            }
        }
        else
        {
            uiManager.ShowMessage(uid, "[非己方回合]"+text);
        }
    }
}
