using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TeamId
{
    Red,
    Black
}

[CreateAssetMenu(menuName="ScriptableObject/PlayerTeam")]
public class PlayerTeam : ScriptableObject
{
    
    public TeamId teamId = TeamId.Red;

    public Color teamColor = Color.red;
     

    public string teamColorString = "Red";

    public string teamName = "队伍名";

    [Header("对面的棋子需要反向")]
    public bool inverseDirection = false;

    [Header("实例化棋子路径")]
    public string instaniatePath = "";

    public InitChessTable initChessTable;

    public void PlaceInitChess(GameManager gameManager)//摆棋
    {
        ChessInitFactory chessInitFactory = gameManager.chessInitFactory;

        foreach (var chessPosStruct in initChessTable.posTable)
        {
            //Debug.Log(this+""+chessPosStruct+""+instaniatePath);
            chessInitFactory.SpawnChessUnit(this,chessPosStruct.chessType,chessPosStruct.pos,instaniatePath);
        }
        
        
    }
   
}
