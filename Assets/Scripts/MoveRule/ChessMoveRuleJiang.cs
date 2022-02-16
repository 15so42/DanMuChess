using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Create ChessMoveRuleJiang", fileName = "ChessMoveRuleJiang", order = 0)]
public class ChessMoveRuleJiang : ChessMoveRule
{
    
    public override ChessMoveResultStruct Check( PlayerTeam playerTeam, Vector2Int startPos, Vector2Int endPos)
    {
        
        //只能在345列移动
        if (endPos.x < 3 || endPos.x > 5)
            return new ChessMoveResultStruct(CheckMoveResultCode.InvalidColumn,"只能在3至5列内移动");
        if (playerTeam.inverseDirection)//只能在789行移动
        {
            if (endPos.y < 7)
                return new ChessMoveResultStruct(CheckMoveResultCode.InvalidRow,"只能在7-9行内移动");
        }
        else
        {
            if (endPos.y > 2)//只能在012行行动
                return new ChessMoveResultStruct(CheckMoveResultCode.InvalidRow,"只能在0-2行内移动");
        }
        //距离判定
        if (Vector2.Distance(startPos, endPos) > 1)
            return new ChessMoveResultStruct(CheckMoveResultCode.InvalidShape,"错误走法，只能走距离为一格的斜线");

        return new ChessMoveResultStruct(CheckMoveResultCode.Success,"移动");
    }
}
