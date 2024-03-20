using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class GameManager : MonoBehaviour
{
    [Header("Game Variables")]
    [SerializeField]
    private List<Cell> cells;

    [SerializeField]
    private bool playerTurn;
    public bool PlayerTurn
    {
        get { return playerTurn; }
    }

    //Variable that checks if the game manager has to use the alpha-beta pruning algorithm
    [SerializeField]
    private bool alphaBeta;

    [Header("Endgame message")]

    [SerializeField]
    private GameObject endgamePanel;

    [SerializeField]
    private TMP_Text endgameDescription;

    //VARIABLES

    private GameBoard gameBoard;

    private GameBoard currentBoard;

    private char playerChar = 'X';
    public char PlayerChar
    {
        get { return playerChar; }
    }

    private char aiChar = 'O';
    public char AIChar
    {
        get { return aiChar; }
    }

    private bool gameEnded;

    private void Start()
    {
        // Initialize empty 3x3 board
        gameBoard = new GameBoard();

        gameEnded = false;

        if (!playerTurn)
        {
            AIMove();
        }
    }

    public void PlayerMove(Move move)
    {
        // Updates board
        gameBoard.ExecuteMove(move, playerChar);
        CheckEndgame();

        // If game hasn't ended makes the AI move
        if (!gameEnded)
        {
            // Starts AI turn
            playerTurn = false;

            if (alphaBeta)
            {
                AIMoveAlphaBeta();
            }
            else
            {
                AIMove();
            }            
        }
    }

    private void AIMove()
    {
        //Initializes bestScore to MaxValue (minimizing turn)
        int bestScore = int.MaxValue;
        Move bestMove = new Move(-1, -1);

        //Creates a copy of current game board
        currentBoard = new GameBoard();
        currentBoard.Board = (char[,]) gameBoard.Board.Clone();

        //Starts minimax for each possible moves after player move
        foreach (Move currentMove in currentBoard.GetNextMoves())
        {
            int currentScore;

            //Executes current possible move
            currentBoard.ExecuteMove(currentMove, aiChar);

            currentScore = Minimax(currentBoard, 0, true);

            //Cleans up move
            currentBoard.ExecuteMove(currentMove, '\0');

            //It's a minimizing turn, so updates bestScore if score < bestScore
            if (currentScore < bestScore)
            {
                bestScore = currentScore;
                bestMove = currentMove;
            }
        }

        // Executes AI best move
        if (bestMove.Row != -1 && bestMove.Col != -1)
        {
            gameBoard.ExecuteMove(bestMove, aiChar);
            cells.Find(x => x.Row == bestMove.Row && x.Column == bestMove.Col).UpdateCell();
        }

        //Checks if game has ended
        CheckEndgame();
    }

    private int Minimax(GameBoard board, int depth, bool isMaximizingPlayer)
    {
        //Checks current node's score
        int score = board.Evaluate();

        //BASE STEP--------------------------------------------------------

        //If Player won
        if (score == 10)
        {
            return score + depth;
        }

        //If AI won
        if (score == -10)
        {
            return score - depth; 
        }

        //If Draw
        if (!board.IsMovesLeft())
        {
            return 0;
        }

        //RECURSIVE STEP----------------------------------------------------

        int bestScore = isMaximizingPlayer ? int.MinValue : int.MaxValue;

        foreach (Move m in board.GetNextMoves()) 
        {
            char currentChar = isMaximizingPlayer ? playerChar : aiChar;
            board.ExecuteMove(m, currentChar);
            bestScore = isMaximizingPlayer
                ? Mathf.Max(bestScore, Minimax(board, depth + 1, false))
                : Mathf.Min(bestScore, Minimax(board, depth + 1, true));
            board.ExecuteMove(m, '\0');
        }

        return bestScore;
    }

    private void AIMoveAlphaBeta()
    {
        //Initializes bestScore to MaxValue (minimizing turn)
        int bestScore = int.MaxValue;
        Move bestMove = new Move(-1, -1);

        //Alpha and beta initialization
        int a = int.MinValue;
        int b = int.MaxValue;

        //Creates a copy of current game board
        currentBoard = new GameBoard();
        currentBoard.Board = (char[,])gameBoard.Board.Clone();

        //Starts minimax for each possible moves after player move
        foreach (Move currentMove in currentBoard.GetNextMoves())
        {
            int currentScore;

            //Executes current possible move
            currentBoard.ExecuteMove(currentMove, aiChar);

            //THESE FIRST STEPS DON'T NEED THE CHECK ON ALPHA AND BETA!!!
            currentScore = AlphaBeta(currentBoard, 0, a, b, true);

            //Cleans up move
            currentBoard.ExecuteMove(currentMove, '\0');

            //It's a minimizing turn, so updates bestScore if score < bestScore
            if (currentScore < bestScore)
            {
                bestScore = currentScore;
                bestMove = currentMove;
            }
        }

        // Executes AI best move
        if (bestMove.Row != -1 && bestMove.Col != -1)
        {
            gameBoard.ExecuteMove(bestMove, aiChar);
            cells.Find(x => x.Row == bestMove.Row && x.Column == bestMove.Col).UpdateCell();
        }

        //Checks if game has ended
        CheckEndgame();
    }

    private int AlphaBeta(GameBoard board, int depth, int alpha, int beta, bool isMaximizingPlayer)
    {
        //BASE STEP (it's the same as minimax)-----------------------------------------------------

        //Checks current node's score
        int score = board.Evaluate();
        
        if (score == 10) //If Player won
        {
            return score + depth;
        }
        
        if (score == -10) //If AI won
        {
            return score - depth;
        }
        
        if (!board.IsMovesLeft()) //If Draw
        {
            return 0;
        }

        //RECURSIVE STEP--------------------------------------------------------------------------

        int bestScore = isMaximizingPlayer ? int.MinValue : int.MaxValue;

        foreach (Move m in board.GetNextMoves())
        {
            char currentChar = isMaximizingPlayer ? playerChar : aiChar;

            board.ExecuteMove(m, currentChar);

            bestScore = isMaximizingPlayer
                ? Mathf.Max(bestScore, Minimax(board, depth + 1, false))
                : Mathf.Min(bestScore, Minimax(board, depth + 1, true));

            if (isMaximizingPlayer)
            {
                if(bestScore > beta)
                {
                    break;
                }
                alpha = Mathf.Max(alpha, bestScore);
            }
            else
            {
                if(bestScore < alpha)
                {
                    break;
                }
                beta = Mathf.Min(beta, bestScore);  
            }

            board.ExecuteMove(m, '\0');
        }

        return bestScore;
    }

    private void CheckEndgame()
    {
        int endgame = gameBoard.Evaluate();

        if (endgame > 0)
        {
            endgameDescription.text = "YOU WON!";
            endgamePanel.SetActive(true);
        }
        else if (endgame < 0)
        {
            endgameDescription.text = "AI WON!";
            endgamePanel.SetActive(true);
        }
        else if (!gameBoard.IsMovesLeft())
        {
            endgameDescription.text = "DRAW!";
            endgamePanel.SetActive(true);
        }
    }
}
