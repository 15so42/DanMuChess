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

        //Debug.Log(path+"/"+playerTeam.teamColorString+"_"+chessType.ToString());

        GameObject pfb = Resources.Load<GameObject>(path+"/"+playerTeam.teamColorString+"_"+chessType.ToString());//可优化存入缓存队列中，方便下次从缓存中再取用，减少Resource.Load次数

        var convertedGridPos=new Vector2(gridPos.y,gridPos.x);//转换坐标,逻辑坐标转换为数组坐标
        GameObject chessGo = Instantiate(pfb, gridManager.GetWorldPosByGirdIndex(convertedGridPos), Quaternion.identity);
        if (playerTeam.teamId == TeamId.Red)
        {
            chessGo.transform.Rotate(Vector3.up,180);//红方棋子旋转180度
        }

        chessGo.transform.position = chessGo.transform.position + Vector3.up * 0.25f;
        
        //配置单位信息
        ChessUnit chessUnit = chessGo.AddComponent<ChessUnit>();
        chessUnit.Init(playerTeam, gridPos);

    }
}
