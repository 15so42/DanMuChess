using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ChessType
{
    将,
    士,
    相,
    马,
    车,
    炮,
    兵

}

public class ChessUnit : MonoBehaviour
{
    public PlayerTeam playerTeam;

    public ChessType chessType;
    public Vector2 logicGridPos;//逻辑坐标

    public ChessMoveRule moveRule;

    public void Init(PlayerTeam playerTeam,  Vector2 logicGridPos)
    {
        this.playerTeam = playerTeam;
        this.logicGridPos = logicGridPos;
        
    }

    public virtual void Move(Vector2 startPos, Vector2 targetPos)
    {
        
    }

    public virtual void OnMoveEnd(Vector2Int endPos)
    {
        this.logicGridPos = endPos;
    }
}
