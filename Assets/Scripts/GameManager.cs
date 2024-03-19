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
        CheckEndgame(gameBoard);

        // If game hasn't ended makes the AI move
        if (!gameEnded)
        {
            // Starts AI turn
            playerTurn = false;
            AIMove();
        }        
    }

    public void AIMove() 
    { 
        //Init AI's best score and best move
        int bestScore = int.MinValue;
        Move bestMove = new Move(-1, -1);

        // Iterates through all the moves available given the current game state
        // and searches for the best move using Minimax
        foreach (GameState possibleMove in gameBoard.GetNextStates(aiChar))
        {
            var moveScore = Minimax(possibleMove.State, 0, true);
            if (moveScore <= bestScore)
            {
                continue;
            }

            bestScore = moveScore;
            bestMove = possibleMove.Move;
        }

        // Executes AI best move
        if (bestMove.Row != -1 && bestMove.Col != -1)
        {
            gameBoard.ExecuteMove(bestMove, aiChar);
            cells.Find(x => x.Row == bestMove.Row && x.Column == bestMove.Col).UpdateCell();
        }

        //Checks if game has ended
        CheckEndgame(gameBoard);
    }

    private int Minimax(char[,] board, int depth, bool isMaximizingPlayer)
    {
        // BASE CASE: checks if current node is a terminal node and
        // assigns it a score based on who wins or if it's a draw
        var score = Evaluate(board);

        if (score == 10) return score + 1 / depth; // X won
        if (score == -10) return score - 1 / depth; // O won
        if (!gameBoard.IsMovesLeft()) return 0;

        // RECURSIVE MINIMAX STEP
        var bestScore = int.MinValue;

        foreach (GameState possibleState in gameBoard.GetNextStates(playerChar))
        {
            bestScore = isMaximizingPlayer
                ? Mathf.Max(bestScore, Minimax(possibleState.State, depth + 1, false))
                : Mathf.Min(bestScore, Minimax(possibleState.State, depth + 1, true));
        }

        return bestScore;
    }

    private int Evaluate(char[,] board)
    {
        // Row victories
        for (var row = 0; row < 3; row++)
            if (board[row, 0] == board[row, 1] && board[row, 1] == board[row, 2])
            {
                if (board[row, 0] == 'X')
                    return +10;
                if (board[row, 0] == 'O')
                    return -10;
            }

        // Column victories
        for (var col = 0; col < 3; col++)
            if (board[0, col] == board[1, col] && board[1, col] == board[2, col])
            {
                if (board[0, col] == 'X')
                    return +10;
                if (board[0, col] == 'O')
                    return -10;
            }

        // Diagonals victories
        if (board[0, 0] == board[1, 1] && board[1, 1] == board[2, 2])
        {
            if (board[0, 0] == 'X')
                return +10;
            if (board[0, 0] == 'O')
                return -10;
        }

        if (board[0, 2] == board[1, 1] && board[1, 1] == board[2, 0])
        {
            if (board[0, 2] == 'X')
                return +10;
            if (board[0, 2] == 'O')
                return -10;
        }

        // No victory found
        return 0;
    }

    private void CheckEndgame(GameBoard state)
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
        else if (!state.IsMovesLeft())
        {
            endgameDescription.text = "DRAW!";
            endgamePanel.SetActive(true);
            gameEnded = true;
        }
    }   
}
