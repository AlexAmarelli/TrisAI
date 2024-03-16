using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private List<Cell> cells;

    private GameState gameState;

    private bool playerTurn;
    public bool PlayerTurn 
    { 
        get { return playerTurn; } 
    }

    private void Start()
    {
        gameState = new GameState();
        playerTurn = true;
    }

    public void UpdateGameState(Move newMove)
    {
        if (playerTurn)
        {
            gameState.ExecuteMove(newMove, 1);
            if (GameData.allAI)
            {
                cells.Find(x => x.Row == newMove.Row && x.Column == newMove.Col).UpdateCell(playerTurn);
            }
            playerTurn = false;
        }
        else
        {
            gameState.ExecuteMove(newMove, -1);
            cells.Find(x => x.Row == newMove.Row && x.Column == newMove.Col).UpdateCell();
            playerTurn = true;
        }
    }
}
