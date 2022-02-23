using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Create ChessMoveRuleChe", fileName = "ChessMoveRuleChe", order = 0)]
public class ChessMoveRuleChe : ChessMoveRule
{
    
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

            if(count>0)
                return new ChessMoveResultStruct(CheckMoveResultCode.InvalidShape,"有阻挡物");
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
            if(count>0)
                return new ChessMoveResultStruct(CheckMoveResultCode.InvalidShape,"有阻挡物");
           
        } 
        

        if (startPos.x == endPos.x && startPos.y != endPos.y || startPos.x != endPos.x && startPos.y == endPos.y)
        {
            return new ChessMoveResultStruct(CheckMoveResultCode.Success,"移动");
        }
        return new ChessMoveResultStruct(CheckMoveResultCode.InvalidShape,"只能走直线");
        

        
    }
}
