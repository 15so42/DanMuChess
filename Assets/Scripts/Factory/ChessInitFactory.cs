using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ChessInitFactory : MonoBehaviour
{

    public GridManager gridManager;

    public void Init()
    {
        gridManager = GameManager.Instance.gridManager;
    }

   

    /// <summary>
    /// 哪个队在哪个位置生成何种类型棋子
    /// </summary>
    /// <param name="playerTeam"></param>
    /// <param name="chessType"></param>
    /// <param name="gridPos"></param>
    public void SpawnChessUnit(PlayerTeam playerTeam,ChessType chessType,Vector2 gridPos,string path)
    {
        if (path == "")
            Debug.LogError("棋子单位生成未配置实例化路径");

        Debug.Log(path+"/"+playerTeam.teamColorString+"_"+chessType.ToString());

        GameObject pfb = Resources.Load<GameObject>(path+"/"+playerTeam.teamColorString+"_"+chessType.ToString());//可优化存入缓存队列中，方便下次从缓存中再取用，减少Resource.Load次数

        var convertedGridPos=new Vector2(gridPos.y,gridPos.x);//转换坐标
        GameObject chessGo = Instantiate(pfb, gridManager.GetWorldPosByGirdIndex(convertedGridPos), Quaternion.identity);

    }
}
