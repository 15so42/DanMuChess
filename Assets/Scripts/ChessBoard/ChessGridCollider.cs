using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChessGridCollider : MonoBehaviour
{
    public Vector2 gridIndex = Vector2.zero;
    public TMP_Text text;

    public void SetGridIndex(Vector2 gridIndex)
    {
        this.gridIndex = gridIndex;
        text.text = "[" + gridIndex.y + "," + gridIndex.x + "]";
        //text.text = gridIndex.x + "," + gridIndex.y;

    }

    public Vector2 GetGridIndex()
    {
        return gridIndex;
    }
}
