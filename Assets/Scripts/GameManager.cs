using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Game Settings")]

    [SerializeField]
    private List<Cell> cells;

    [SerializeField]
    private int maxDepth;

    [Header("Endgame message")] 

    [SerializeField]
    private GameObject endgamePanel;

    [SerializeField]
    private TMP_Text endgameDescription;

    private GameState gameState;

    private List<Move> moves = new List<Move>();

    private bool playerTurn;
    public bool PlayerTurn 
    { 
        get { return playerTurn; } 
    }

    private int winScore = 3;

    private Move playerMove;

    private int currentMovesLength;

    private bool gameEnded;

    private void Start()
    {
        gameEnded = false;

        // Initialize empty 3x3 board with all zeros
        gameState = new GameState();

        // Init list of possible moves
        for(int i = 0; i < gameState.Board.GetLength(0); i++)
        {
            for(int j = 0; j  < gameState.Board.GetLength(1); j++)
            {
                moves.Add(new Move(i, j, 0));
            }
        }

        playerTurn = true;
    }

    public void UpdateGameState(Move newMove)
    {
        if (playerTurn)
        {
            // Updates board
            gameState.ExecuteMove(newMove, 1);

            //In an AIvAI game, updates board cell
            if (GameData.allAI)
            {
                cells.Find(x => x.Row == newMove.Row && x.Column == newMove.Col).UpdateCell(playerTurn);
            }

            // Removes move frome moves list
            Move removedMove = moves.Find(x => x.Row == newMove.Row && x.Col == newMove.Col);
            playerMove = removedMove;
            moves.Remove(removedMove);

            CheckEndgame(gameState);

            //Starts AI turn
            playerTurn = false;
            AITurn();
        }
        else
        {
            // Updates board
            gameState.ExecuteMove(newMove, -1);
            cells.Find(x => x.Row == newMove.Row && x.Column == newMove.Col).UpdateCell();

            // Removes move frome moves list
            Move removedMove = moves.Find(x => x.Row == newMove.Row && x.Col == newMove.Col);
            moves.Remove(removedMove);

            CheckEndgame(gameState);

            playerTurn = true;
        }
    }

    private void AITurn()
    {
        GameState currentGameState = new GameState(gameState.Board);

        Move aiMove = Minimax(currentGameState, playerMove, maxDepth, true);
        UpdateGameState(aiMove);
    }

    private Move Minimax(GameState currentGameState, Move currentMove, int depth, bool maximazingPlayer)
    {
        int score = Evaluate(currentGameState.Board);

        List<Move> possibleMoves = currentGameState.GetMoves();

        if (possibleMoves.Count == 0 || score != 0 || depth == 0)
        {
            currentMove.Score = score;
            //Debug.Log(currentMove);
            return currentMove;
        }

        Move bestMove = null;

        if (maximazingPlayer)
        {
            int value = -1000;

            foreach (Move m in possibleMoves)
            {
                GameState nextGameState = new GameState(currentGameState.Board);
                nextGameState.ExecuteMove(m, -1);
                Move nextMove = Minimax(nextGameState, m, depth - 1, false);
                if (nextMove.Score > value)
                {
                    bestMove = nextMove;
                    value = Mathf.Max(value, nextMove.Score);
                    bestMove.Score = value;
                }
            }

            //Debug.Log(bestMove);
            return bestMove;
        }
        else
        {
            int value = 1000;

            foreach (Move m in possibleMoves)
            {
                GameState nextGameState = new GameState(currentGameState.Board);
                nextGameState.ExecuteMove(m, 1);
                Move nextMove = Minimax(nextGameState, m, depth - 1, true);
                if (nextMove.Score < value)
                {
                    bestMove = nextMove;
                    value = Mathf.Min(value, nextMove.Score);
                    bestMove.Score = value;
                }
            }

            //Debug.Log(bestMove);
            return bestMove;
        }
    }

    private int Evaluate(int[,] board)
    {
        //Check diagonal
        if (board[1,1] != 0 &&
            ((board[0,0] == board[1,1] && board[2,2] == board[1,1]) || 
             (board[0,2] == board[1,1] && board[2,0] == board[1,1])))
        {
            return board[1,1];
        }

        //Checking horizontal 
        for (int i = 0; i < board.GetLength(0); i++)
        {
            if (board[i, 0] != 0 && board[i, 0] == board[i, 1] && board[i, 2] == board[i, 1])
            {
                return board[i, 0];
            }
        }

        //Checking horizontal 
        for (int i = 0; i < board.GetLength(0); i++)
        {
            if (board[0, i] != 0 && board[0, i] == board[1, i] && board[2, i] == board[1, i])
            {
                return board[0, i];
            }
        }

        return 0;
    }    

    private void CheckEndgame(GameState state)
    {
        if (!gameEnded)
        {
            int endgame = Evaluate(state.Board);

            if (endgame > 0)
            {
                endgameDescription.text = "YOU WON!";
                endgamePanel.SetActive(true);
                gameEnded = true;
            }
            else if (endgame < 0)
            {
                endgameDescription.text = "AI WON!";
                endgamePanel.SetActive(true);
                gameEnded = true;
            }
            else
            {
                if (state.GetMoves().Count == 0)
                {
                    endgameDescription.text = "DRAW!";
                    endgamePanel.SetActive(true);
                    gameEnded = true;
                }
            }
        }        
    }
}
