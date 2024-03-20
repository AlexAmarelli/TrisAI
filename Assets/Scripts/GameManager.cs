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
            AIMove();
        }
    }

    void AIMove()
    {
        int bestScore = int.MaxValue;
        Move bestMove = new Move(-1, -1);

        currentBoard = new GameBoard();
        currentBoard.Board = (char[,]) gameBoard.Board.Clone();

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (currentBoard.Board[i, j] == '\0')
                {
                    Move currentMove = new Move(i, j);  
                    currentBoard.ExecuteMove(currentMove, aiChar);
                    int score = Minimax(currentBoard, 0, true);
                    currentBoard.ExecuteMove(currentMove, '\0');

                    if (score < bestScore)
                    {
                        bestScore = score;
                        bestMove = currentMove;
                    }
                }
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

    int Evaluate(char[,] board)
    {
        // Row victories
        for (int row = 0; row < 3; row++)
        {
            if (board[row, 0] == board[row, 1] && board[row, 1] == board[row, 2])
            {
                if (board[row, 0] == 'X')
                    return +10;
                else if (board[row, 0] == 'O')
                    return -10;
            }
        }

        // Column victories
        for (int col = 0; col < 3; col++)
        {
            if (board[0, col] == board[1, col] && board[1, col] == board[2, col])
            {
                if (board[0, col] == 'X')
                    return +10;
                else if (board[0, col] == 'O')
                    return -10;
            }
        }

        // Diagonals victories
        if (board[0, 0] == board[1, 1] && board[1, 1] == board[2, 2])
        {
            if (board[0, 0] == 'X')
                return +10;
            else if (board[0, 0] == 'O')
                return -10;

        }
        if (board[0, 2] == board[1, 1] && board[1, 1] == board[2, 0])
        {
            if (board[0, 2] == 'X')
                return +10;
            else if (board[0, 2] == 'O')
                return -10;
        }

        // No victory found
        return 0;
    }

    int Minimax(GameBoard board, int depth, bool isMaximizingPlayer)
    {
        int score = Evaluate(board.Board);

        if (score == 10) return score + depth;  // X won
        if (score == -10) return score - depth; // O won
        if (!board.IsMovesLeft()) return 0;

        int bestScore = isMaximizingPlayer ? int.MinValue : int.MaxValue;

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (board.Board[i, j] == '\0')
                {
                    char currentChar = isMaximizingPlayer ? playerChar : aiChar;
                    board.ExecuteMove(new Move(i, j), currentChar);
                    bestScore = isMaximizingPlayer
                        ? Mathf.Max(bestScore, Minimax(board, depth + 1, false))
                        : Mathf.Min(bestScore, Minimax(board, depth + 1, true));
                    board.ExecuteMove(new Move(i, j), '\0');
                }
            }
        }
        return bestScore;
    }

    private void CheckEndgame()
    {
        int endgame = Evaluate(gameBoard.Board);

        if (endgame > 0)
        {
            endgameDescription.text = "YOU WON!";
            endgamePanel.SetActive(true);
            //gameEnded = true;
        }
        else if (endgame < 0)
        {
            endgameDescription.text = "AI WON!";
            endgamePanel.SetActive(true);
            //gameEnded = true;
        }
        else if (!gameBoard.IsMovesLeft())
        {
            endgameDescription.text = "DRAW!";
            endgamePanel.SetActive(true);
            //gameEnded = true;
        }
    }
}
