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
    public MoveChessManager moveChessManager;

    public ChessInitFactory chessInitFactory;


    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        chessInitFactory.Init();
        gridManager.Init();
        fightingManager.Init();
        moveChessManager.Init();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
