using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState 
{
    private int[,] board = new int[3,3];
    public int[,] Board
    {
        get { return board; }
        set { board = value; }
    }

    public GameState()
    {
        board = new int[3, 3];
    }

    public GameState(int[,] board)
    {
        Board = board;
    }

    public GameState(Move move, int value)
    {
        int[,] newBoard = new int[3, 3];
        newBoard[move.Row, move.Col] = value;
        Board = newBoard;
    }

    public void ExecuteMove(Move move, int value)
    {
        Board[move.Row, move.Col] = value;
    }
}
