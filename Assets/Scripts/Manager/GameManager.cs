using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    //管理Manager
    public GridManager gridManager;
    public FightingManager fightingManager;
    //public UIManager uIManager;
    public ChessMoveManager chessMoveManager;

    public ChessInitFactory chessInitFactory;


    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        gridManager.Init(this);
        chessInitFactory.Init(this);
        
        fightingManager.Init();
        //moveChessManager由GridManager初始化
        //moveChessManager.Init();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
