using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.CodeEditor;
using UnityEngine;

public class ChessMoveManager : MonoBehaviour
{
    private GameManager gameManager;

    private GridManager gridManager;
    
    //创建一个二维数组存放实际的棋盘数据
    ChessUnit[,] chessTable;
    // Start is called before the first frame update
    public void Init(int xCount,int yCount)//yCount为行，xCount为列
    {
        chessTable = new ChessUnit[xCount, yCount];//与逻辑坐标系统一,获取世界坐标时再转换
        gameManager=GameManager.Instance;
        gridManager = gameManager.gridManager;
    }

    public void SetChess(Vector2Int logicGridPos,ChessUnit chessUnit)//摆盘的时候给逻辑表赋予棋子位置，0,0为车,1,0为马
    {
        chessTable[logicGridPos.x, logicGridPos.y] = chessUnit;
    }
    public bool IsExitChess(Vector2Int logicGridPos)
    {
        
        return chessTable[logicGridPos.x, logicGridPos.y]==null;
    }

    public ChessUnit GetChessUnit(Vector2Int logicGridPos)
    {
        return chessTable[logicGridPos.x, logicGridPos.y];
    }

    public void MoveChessToPos(PlayerTeam playerTeam,Vector2Int startGridPos,Vector2Int endPos)
    {
        ChessUnit chessUnit = GetChessUnit(startGridPos);
        if (chessUnit == null)
        {
            TipsDialog.ShowDialog("无可移动棋子",null);
            return;
        }

        var ruleResult = chessUnit.moveRule.Check(playerTeam, startGridPos, endPos);
        if (ruleResult.code==CheckMoveResultCode.Success)
        {
            chessUnit.transform.DOMove(gridManager.GetWorldPosByLogicPos(endPos), 1.2f).OnComplete(() =>
            {
                SetChess(startGridPos,null);
                SetChess(endPos,chessUnit);
                chessUnit.OnMoveEnd(endPos);
            });
        }
        else
        {
            TipsDialog.ShowDialog(ruleResult.message,null);
        }
       
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
