using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ChessUnitPos
{
    public Vector2Int pos;
    public ChessType chessType;

};

[CreateAssetMenu(menuName = "ScriptableObject/InitChessTable")]
public class InitChessTable : ScriptableObject
{
    public List<ChessUnitPos> posTable;
    
}
