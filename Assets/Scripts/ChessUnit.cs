using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
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
    public Vector2Int logicGridPos;//逻辑坐标

    public ChessMoveRule moveRule;
    //死亡动画参数
    float jumpPower = 15f;

    public void Init(PlayerTeam playerTeam,  Vector2Int logicGridPos)
    {
        this.playerTeam = playerTeam;
        this.logicGridPos = logicGridPos;
        moveRule.Init(GameManager.Instance.chessMoveManager);
        
    }

    public Player FindWinnerPlayer(PlayerTeam loseTeam)//通过输家来找到赢家
    {
        FightingManager fightingManager = GameManager.Instance.fightingManager;
        PlayerTeam winnerTeam= fightingManager.teams.Find(x => x != loseTeam);
        Player winnerPlayer = fightingManager.players.Find(x => x.playerTeam == winnerTeam);
        return winnerPlayer;
    }
    
    public void OnAttacked()
    {
        //从逻辑棋盘中删除
        GameManager.Instance.chessMoveManager.SetChess(logicGridPos,null);
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOJump(transform.position + Vector3.up*jumpPower, jumpPower, 1, 1f));
        sequence.Insert(0, transform.DOPunchRotation(new Vector3(-180, 0, 0), 1f));
        //sequence.Join(transform.DOScale(Vector3.zero, 0.5f));
        sequence.OnComplete(() =>
        {
            if (chessType == ChessType.将)//游戏结束
            {
                GameManager.Instance.fightingManager.BattleOver(FindWinnerPlayer(playerTeam));
            }
            Destroy(gameObject);
        });

    }
    public virtual void Move(Vector2 startPos, Vector2 targetPos)
    {
        
    }

    public void Kill()
    {
        OnAttacked();
    }

    public virtual void OnMoveEnd(Vector2Int endPos)
    {
        this.logicGridPos = endPos;
    }
}
