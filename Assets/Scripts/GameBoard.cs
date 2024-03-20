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
}
