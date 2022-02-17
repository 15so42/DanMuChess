using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int xCount = 9;
    public int yCount=10;

    public float horSize = 1;
    public float verSize=1;

    public GameObject chessGridColliderPfb;

    public ChessMoveManager chessMoveManager;
    public GameManager gameManager;

    Vector3[,] grid;

    // Start is called before the first frame update
    public void Init(GameManager gameManager)
    {
        this.gameManager = gameManager;
        
        grid = new Vector3[yCount, xCount];//存储的信息
        CalPositionToGrid();

        chessMoveManager = gameManager.chessMoveManager;
        chessMoveManager.Init(xCount, yCount);

        //SpawnColliders();

    }

    void SpawnColliders()
    {
        for (var i = 0; i < yCount; i++)
        {
            for (var j = 0; j < xCount; j++)
            {
                GameObject collider = GameObject.Instantiate(chessGridColliderPfb, grid[i, j], Quaternion.identity);
                collider.GetComponent<ChessGridCollider>().SetGridIndex(new Vector2(i, j));
            }
        }

    }

    void CalPositionToGrid()
    {
        for (var i = 0; i < yCount; i++)//实际坐标
        {
            for(var j = 0; j < xCount; j++)
            {
                //注意如果要让实际棋盘坐标方向和逻辑坐标方向一致的话需要用j来乘右边，i来乘左边
                grid[i, j] = transform.position + Vector3.right * j * horSize + Vector3.forward * i * verSize;
                //为棋盘每个格子生成棋盘碰撞体：
                
            }
        }

    }

    public Vector3 GetWorldPosByGirdIndex(Vector2 gridPos)
    {
       
        return grid[(int)gridPos.x, (int)gridPos.y];
    }

    private void OnDrawGizmos()
    {
        //return;
        grid = new Vector3[yCount, xCount];//10行9列的数组
        CalPositionToGrid();
        for (var i = 0; i < xCount; i++)//按照人类可识别的序号打印，还是10行9列，不过按列从左往右数
        {
            for (var j = 0; j < yCount; j++)
            {             
               Gizmos.DrawWireSphere(GetWorldPosByGirdIndex(new Vector2(j,i)), 0.25f);
            }
        }
    }
}
