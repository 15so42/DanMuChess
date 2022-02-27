using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.CodeEditor;
using UnityEngine;

public class ChessMoveManager : MonoBehaviour
{
    private GameManager gameManager;

    private GridManager gridManager;

    private int xCount;
    private int yCount;
    
    //创建一个二维数组存放实际的棋盘数据
    ChessUnit[,] chessTable;
    // Start is called before the first frame update
    public void Init(int xCount,int yCount)//yCount为行，xCount为列
    {
        this.xCount = xCount;
        this.yCount = yCount;//记录用于ReInit,因为ReInit的时候不需要初始化gridManager
        chessTable = new ChessUnit[xCount, yCount];//与逻辑坐标系统一,获取世界坐标时再转换
        gameManager=GameManager.Instance;
        gridManager = gameManager.gridManager;
    }

    public void ReInit()
    {
        //删除所有已经生成的棋子
        foreach (var chessUnit in chessTable)
        {
            if (chessUnit != null)
            {
                SetChess(chessUnit.logicGridPos, null);
                Destroy(chessUnit.gameObject);
            }
        }
        Init(xCount,yCount);
    }

    public void SetChess(Vector2Int logicGridPos,ChessUnit chessUnit)//摆盘的时候给逻辑表赋予棋子位置，0,0为车,1,0为马
    {
        chessTable[logicGridPos.x, logicGridPos.y] = chessUnit;
    }
    public bool IsExitChess(Vector2Int logicGridPos)
    {
        return chessTable[logicGridPos.x, logicGridPos.y]!=null;
    }

    public ChessUnit GetChessUnit(Vector2Int logicGridPos)
    {
        return chessTable[logicGridPos.x, logicGridPos.y];
    }

    /// <summary>
    /// 根据棋子种类和队伍来寻找棋子
    /// </summary>
    /// <param name="chessType"></param>
    /// <param name="playerTeam"></param>
    /// <returns></returns>
    public ChessUnit GetChessByChessType(ChessType chessType,PlayerTeam playerTeam)
    {
        foreach (var chessUnit in chessTable)
        {
            if (chessUnit!=null && chessUnit.chessType == chessType && chessUnit.playerTeam == playerTeam)
                return chessUnit;
        }

        return null;
    }

    /// <summary>
    /// 返回是否成功
    /// </summary>
    /// <param name="playerTeam"></param>
    /// <param name="startGridPos"></param>
    /// <param name="endPos"></param>
    /// <returns></returns>
    public bool MoveChessToPos(PlayerTeam playerTeam,Vector2Int startGridPos,Vector2Int endPos)
    {
        ChessUnit chessUnit = GetChessUnit(startGridPos);
        if (chessUnit == null)
        {
            TipsDialog.ShowDialog("无可移动棋子",null);
            return false;
        }

        if (chessUnit.playerTeam != playerTeam)
        {
            TipsDialog.ShowDialog("不可移动对方棋子",null);
            return false;
        }
        
        //判断是否要移动到友方棋子位置
        var endChess = GetChessUnit(endPos);
        if (endChess != null && playerTeam == endChess.playerTeam)
        {
            TipsDialog.ShowDialog("目标位置有友方棋子",null);
            return false;
        }

        var ruleResult = chessUnit.moveRule.Check(playerTeam, startGridPos, endPos);
        
        
        if (ruleResult.code==CheckMoveResultCode.Success)
        {
            if (Vector2.Distance(startGridPos, endPos)>1.4f)
            {
                Sequence sequence = DOTween.Sequence();
                sequence.Append(chessUnit.transform.DOJump(chessUnit.transform.position + Vector3.up * 0.5f, 2, 1, 1).SetEase(Ease.OutQuad));
                sequence.Append(chessUnit.transform.DOMove(gridManager.GetWorldPosByLogicPos(endPos), 0.6f)
                    /*.SetEase(Ease.OutQuad)*/);
                sequence.OnComplete(() =>
                {
                    OnChessMoveEnd(chessUnit, startGridPos, endPos);
                });
            }
            else
            {
                chessUnit.transform.DOMove(gridManager.GetWorldPosByLogicPos(endPos), 1.2f).SetEase(Ease.OutQuint)
                    .OnComplete(() => { OnChessMoveEnd(chessUnit, startGridPos, endPos); });
            }

            return true;
        }
        
        TipsDialog.ShowDialog(ruleResult.message,null);
        return false;

    }

    /// <summary>
    /// 棋子达到目标位置后数据处理
    /// </summary>
    /// <param name="chessUnit"></param>
    /// <param name="startGridPos"></param>
    /// <param name="endPos"></param>
    public void OnChessMoveEnd(ChessUnit chessUnit,Vector2Int startGridPos,Vector2Int endPos)
    {
        SetChess(startGridPos,null);
        //攻击终点处棋子
        var endChess = GetChessUnit(endPos);
        if (endChess!=null)
        {
            endChess.OnAttacked();
        }
        //终点位置设置新棋子
        SetChess(endPos,chessUnit);
        chessUnit.OnMoveEnd(endPos);

        //将军判定
        var fightingManager = GameManager.Instance.fightingManager;
        var enemyJiang= GetChessByChessType(ChessType.将,fightingManager.FindAnotherPlayer(chessUnit.playerTeam).playerTeam);
        //Debug.Log(enemyJiang + "," + enemyJiang.logicGridPos);
        if(enemyJiang!=null && chessUnit!=null && chessUnit.moveRule.Check(chessUnit.playerTeam, endPos, enemyJiang.logicGridPos).code == CheckMoveResultCode.Success)
        {
            TipsDialog.ShowDialog("将军，请注意", null);
        }

        //增加将不能面对面的全局判定，如果移动后使自己的将在对面的将在对面，则游戏结束，同时需要再将移动时要求不能移动到面对面的位置
        var jiangChess = GetChessByChessType(ChessType.将, chessUnit.playerTeam);
        if (jiangChess != null)
        {
            int count = 0;//遮挡物
            bool inSameX = false;//同一列
            for (var i = 0; i <= 9; i++)
            {
                var newChess = GetChessUnit(new Vector2Int(jiangChess.logicGridPos.x, i));
                //Debug.Log(jiangChess.logicGridPos.x+","+i+","+newChess);
                if (newChess != null && newChess.chessType != ChessType.将)
                {
                    count++;
                }
                
                if (newChess!=null && newChess.chessType == ChessType.将 && newChess.playerTeam != chessUnit.playerTeam)
                {
                    inSameX = true;
                    
                }
            }

            if (count == 0 && inSameX)
            {
                TipsDialog.ShowDialog("双将会面,"+chessUnit.playerTeam.teamColorString+"方将被击杀", () =>
                {
                    jiangChess.Kill();//双方将面对面了
                });
                
            }
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
