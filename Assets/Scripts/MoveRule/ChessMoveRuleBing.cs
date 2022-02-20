using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Create ChessMoveRuleBing", fileName = "ChessMoveRuleBing", order = 0)]
public class ChessMoveRuleBing : ChessMoveRule
{

    /// <summary>
    /// 移动到自身或者棋盘外在更上层判断
    /// </summary>
    /// <param name="playerTeam"></param>
    /// <param name="startPos"></param>
    /// <param name="endPos"></param>
    /// <returns></returns>
    public override ChessMoveResultStruct Check( PlayerTeam playerTeam, Vector2Int startPos, Vector2Int endPos)
    {

        if (playerTeam.inverseDirection)//反向方，只能前进
        {
            
            //new
            if (startPos.y >= 5 && endPos.y < startPos.y && endPos.x == startPos.x &&
                Vector2.Distance(startPos, endPos) < 1.4f)
            {
                return new ChessMoveResultStruct(CheckMoveResultCode.Success,"移动");
            }
            
            if (startPos.y < 5 && endPos.y<=startPos.y && Vector2.Distance(startPos, endPos) < 1.4f)
            {
                return new ChessMoveResultStruct(CheckMoveResultCode.Success,"移动");
            }
        }
        else
        {
            //new
            if (startPos.y <= 4 && endPos.y > startPos.y && endPos.x == startPos.x &&
                Vector2.Distance(startPos, endPos) < 1.4f)
            {
                return new ChessMoveResultStruct(CheckMoveResultCode.Success,"移动");
            }
            
            if (startPos.y > 4 && endPos.y>=startPos.y && Vector2.Distance(startPos, endPos) < 1.4f)
            {
                return new ChessMoveResultStruct(CheckMoveResultCode.Success,"移动");
            }
        }
       
        
        return new ChessMoveResultStruct(CheckMoveResultCode.InvalidShape,"走法错误");
    }
}
