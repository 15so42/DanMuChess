using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ChessType
{
    将,
    士,
    相,
    马,
    车,
    炮,
    兵

}

public class ChessUnit : MonoBehaviour
{
    public PlayerTeam playerTeam;

    public ChessType chessType;

    public void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
