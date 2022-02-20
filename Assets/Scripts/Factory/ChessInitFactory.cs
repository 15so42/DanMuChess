using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct SpawnStruct
{
    public ChessType chessType;
    public GameObject pfb;
}

public class ChessInitFactory : MonoBehaviour
{
    public GameManager gameManager;
    public GridManager gridManager;

    public List<SpawnStruct> redSpawnTable=new List<SpawnStruct>();
    public List<SpawnStruct> blackSpawnTable = new List<SpawnStruct>();
    
    public void Init(GameManager gameManager)
    {
        this.gameManager = gameManager;
        gridManager = gameManager.gridManager;
    }

    public GameObject FindPfb(PlayerTeam playerTeam, ChessType chessType)
    {
        if (playerTeam.teamId == TeamId.Red)
        {
            return redSpawnTable.Find(x => x.chessType == chessType).pfb;
        }
        if (playerTeam.teamId == TeamId.Black)
        {
            return blackSpawnTable.Find(x => x.chessType == chessType).pfb;
        }

        return null;
    }

    /// <summary>
    /// 哪个队在哪个位置生成何种类型棋子
    /// </summary>
    /// <param name="playerTeam"></param>
    /// <param name="chessType"></param>
    /// <param name="logicGridPos"></param>
    public void SpawnChessUnit(PlayerTeam playerTeam,ChessType chessType,Vector2Int logicGridPos,string path)
    {  
        var convertedGridPos=new Vector2(logicGridPos.y,logicGridPos.x);//转换坐标,逻辑坐标转换为数组坐标
        GameObject chessGo = Instantiate(FindPfb(playerTeam,chessType), gridManager.GetWorldPosByGirdIndex(convertedGridPos), Quaternion.identity);
        if (playerTeam.inverseDirection)
        {
            chessGo.transform.Rotate(Vector3.up,180);//红方棋子旋转180度
        }

        chessGo.transform.position = chessGo.transform.position + Vector3.up * 0.25f;
        
        //配置单位信息
        ChessUnit chessUnit = chessGo.GetComponent<ChessUnit>();
        chessUnit.Init(playerTeam, logicGridPos);
        gameManager.chessMoveManager.SetChess(logicGridPos, chessUnit);
        

    }
}
