using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Create ChessMoveRulePao", fileName = "ChessMoveRulePao", order = 0)]
public class ChessMoveRulePao : ChessMoveRule
{

    public ChessMoveResultStruct CheckByCount(PlayerTeam playerTeam, int count, Vector2Int endPos)
    {
        if (count > 1)
        {
            return new ChessMoveResultStruct(CheckMoveResultCode.InvalidShape,"阻挡物太多");
        }
        else
        {
            if (count == 1)
            {
                if (chessMoveManager.IsExitChess(endPos))//目标位置有棋子
                {
                    if(chessMoveManager.GetChessUnit(endPos).playerTeam!=playerTeam)//且是敌军
                        return new ChessMoveResultStruct(CheckMoveResultCode.Success,"攻击");
                }
                else
                {
                    return new ChessMoveResultStruct(CheckMoveResultCode.InvalidShape,"无攻击目标");
                }
            }
            else
            {
                if (chessMoveManager.IsExitChess(endPos)) //目标位置有棋子
                {
                    return new ChessMoveResultStruct(CheckMoveResultCode.InvalidShape, "无法移动到目标位置");
                }
            }

        }
        return new ChessMoveResultStruct(CheckMoveResultCode.Success,"移动");
    }
    
    public override ChessMoveResultStruct Check( PlayerTeam playerTeam, Vector2Int startPos, Vector2Int endPos)
    {

        if (endPos.x == startPos.x && endPos.y != startPos.y)//竖向移动
        {
            var step = endPos.y - startPos.y > 0 ? 1 : -1;
            int count = 0;
            for (int i = startPos.y+step; step>0 ?  i <= endPos.y-step : i>=endPos.y-step; i += step)
            {
                //Debug.Log("炮检查竖向坐标"+endPos.x+","+i);
                if (chessMoveManager.IsExitChess(new Vector2Int(endPos.x, i)))//路途上有棋子
                {
                    count++;
                }
            }

            return CheckByCount(playerTeam, count, endPos);
        }
        //横向
        if (endPos.y == startPos.y && endPos.x != startPos.x)//竖向移动
        {
            var step = endPos.x - startPos.x > 0 ? 1 : -1;
            int count = 0;
            for (int i = startPos.x+step; step>0 ?  i <= endPos.x-step : i>=endPos.x-step; i += step)
            {
                //Debug.Log("炮检查横向坐标"+i+","+endPos.y);
                if (chessMoveManager.IsExitChess(new Vector2Int(i, endPos.y)))//路途上有棋子
                {
                    count++;
                }
            }
            return CheckByCount(playerTeam, count, endPos);
           
        } 
       

        return new ChessMoveResultStruct(CheckMoveResultCode.InvalidShape,"未知错误");
    }
}
