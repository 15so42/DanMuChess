﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CheckMoveResultCode
{
    InvalidRow,//非法行数
    InvalidColumn,//非法列数
    InvalidShape,//走法不符合规则(如果使用距离进行判断就表示非法距离
    CantCrossRiver,//不允许过河
    Success,//可移动
    
}

public struct ChessMoveResultStruct
{
    private CheckMoveResultCode code;

    private string message;

    public ChessMoveResultStruct(CheckMoveResultCode code,string message)
    {
        this.code = code;
        this.message = message;
    }
}

public abstract class ChessMoveRule : ScriptableObject
{
    public ChessMoveManager chessMoveManager;
    public void Init(ChessMoveManager chessMoveManager)
    {
        this.chessMoveManager = chessMoveManager;
    }

    public abstract ChessMoveResultStruct Check(PlayerTeam playerTeam, Vector2Int startPos, Vector2Int endPos);
}
