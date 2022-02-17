using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Create ChessMoveRuleMa", fileName = "ChessMoveRuleMa", order = 0)]
public class ChessMoveRuleMa : ChessMoveRule
{
    
    public override ChessMoveResultStruct Check( PlayerTeam playerTeam, Vector2Int startPos, Vector2Int endPos)
    {
        
        //只能在0,2,4,6,8列移动
        /*if ((int) endPos.x % 2 != 0)
        {
            return new ChessMoveResultStruct(CheckMoveResultCode.InvalidColumn,"只能移动到偶数列");
        }

        if (playerTeam.inverseDirection)//只能在789行移动
        {
            if (endPos.y < 5 )
                return new ChessMoveResultStruct(CheckMoveResultCode.CantCrossRiver,"象不能过河");
            if ((int)endPos.y % 2 == 0)
            {
                return new ChessMoveResultStruct(CheckMoveResultCode.CantCrossRiver,"只能移动到奇数行");
            }
        }
        else
        {
            if (endPos.y > 4)//只能在012行行动
                return new ChessMoveResultStruct(CheckMoveResultCode.CantCrossRiver,"象不能过河");
            if((int)endPos.y %2 == 1)
                return new ChessMoveResultStruct(CheckMoveResultCode.CantCrossRiver,"只能移动到偶数行");
        }*/
        //距离判定
        if (Vector2.Distance(startPos, endPos) <=2 || Vector2.Distance(startPos, endPos) >=3)//斜线
            return new ChessMoveResultStruct(CheckMoveResultCode.InvalidShape,"马只能走日字");
        
        //判断绊脚马
        if (Mathf.Abs((int)startPos.y-(int)endPos.y)==2)//上下走
        {
            if (endPos.y>startPos.y && chessMoveManager.IsExitChess(new Vector2Int(startPos.x, startPos.y + 1)))
            {
                return new ChessMoveResultStruct(CheckMoveResultCode.InvalidShape,"绊脚马");
            }
            if (endPos.y<startPos.y && chessMoveManager.IsExitChess(new Vector2Int(startPos.x, startPos.y - 1)))
            {
                return new ChessMoveResultStruct(CheckMoveResultCode.InvalidShape,"绊脚马");
            }
        }
        
        if (Mathf.Abs((int)startPos.x-(int)endPos.x)==2)//向上走
        {
            if (endPos.x>startPos.x && chessMoveManager.IsExitChess(new Vector2Int(startPos.x+1, startPos.y)))
            {
                return new ChessMoveResultStruct(CheckMoveResultCode.InvalidShape,"绊脚马");
            }
            if (endPos.x<startPos.x && chessMoveManager.IsExitChess(new Vector2Int(startPos.x-1, startPos.y)))
            {
                return new ChessMoveResultStruct(CheckMoveResultCode.InvalidShape,"绊脚马");
            }
        }
        

        return new ChessMoveResultStruct(CheckMoveResultCode.Success,"移动");
    }
}
