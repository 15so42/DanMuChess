using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessGridCollider : MonoBehaviour
{
    public Vector2 gridIndex = Vector2.zero;

    public void SetGridIndex(Vector2 gridIndex)
    {
        this.gridIndex = gridIndex;
    }

    public Vector2 GetGridIndex()
    {
        return gridIndex;
    }
}
