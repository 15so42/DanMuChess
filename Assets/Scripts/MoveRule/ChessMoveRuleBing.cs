using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Create ChessMoveRuleBing", fileName = "ChessMoveRuleBing", order = 0)]
public class ChessMoveRuleBing : ChessMoveRule
{

    
    public override ChessMoveResultStruct Check( PlayerTeam playerTeam, Vector2Int startPos, Vector2Int endPos)
    {

        if (playerTeam.inverseDirection)//反向方，只能前进
        {
            if (endPos.y > startPos.y || endPos.y>=5)
            {
                return new ChessMoveResultStruct(CheckMoveResultCode.InvalidShape,"走法错误");
            }

            if (endPos.y < 5 && Math.Abs( Vector2.Distance(startPos, endPos)) <1.4f)
            {
                return new ChessMoveResultStruct(CheckMoveResultCode.Success,"移动");
            }
        }
        else
        {
            if (endPos.y < startPos.y || endPos.y<=4)
            {
                return new ChessMoveResultStruct(CheckMoveResultCode.InvalidShape,"走法错误");
            }

            if (endPos.y >= 5 && Math.Abs( Vector2.Distance(startPos, endPos)) <1.4f)
            {
                return new ChessMoveResultStruct(CheckMoveResultCode.Success,"移动");
            }
        }
       
        
        return new ChessMoveResultStruct(CheckMoveResultCode.InvalidShape,"未知错误");
    }
}
