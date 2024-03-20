using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoard 
{
    private char[,] board = new char[3,3];
    public char[,] Board
    {
        get { return board; }
        set { board = value; }
    }

    public GameBoard()
    {
        board = new char[3, 3];
    }

    public void ExecuteMove(Move move, char value)
    {
        Board[move.Row, move.Col] = value;
    }

    public List<Move> GetNextMoves()
    {
        List<Move> moves = new List<Move> ();

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (board[i, j] == '\0')
                {
                    moves.Add(new Move(i,j));
                }
            }
        }

        return moves;
    }

    public bool IsMovesLeft()
    {
        // Check if game is not over
        for (var i = 0; i < 3; i++)
            for (var j = 0; j < 3; j++)
                // Check if at least one cell is not filled with 'X' or 'O'
                if (board[i, j] == '\0')
                    return true;
        return false;
    }

    public int Evaluate()
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
}
