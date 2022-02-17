using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessMoveManager : MonoBehaviour
{
    //创建一个二维数组存放实际的棋盘数据
    ChessUnit[,] chessTable;
    // Start is called before the first frame update
    public void Init(int xCount,int yCount)//yCount为行，xCount为列
    {
        chessTable = new ChessUnit[xCount, yCount];//与逻辑坐标系统一,获取世界坐标时再转换
    }

    public void SetChess(Vector2Int logicGridPos,ChessUnit chessUnit)
    {
        chessTable[logicGridPos.x, logicGridPos.y] = chessUnit;
    }
    public bool IsExitChess(Vector2Int logicGridPos)
    {
        
        return chessTable[logicGridPos.y, logicGridPos.x]==null;
    }

    public ChessUnit GetChessUnit(Vector2Int logicGridPos)
    {
        return chessTable[logicGridPos.y, logicGridPos.x];
    }

    public void MoveChessToPos()
    {

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
