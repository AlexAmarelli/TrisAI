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
        for (int i = 0; i < board.GetLength(0); i++)
        {
            for(int j = 0; j < board.GetLength(1); j++)
            {
                Board[i,j] = board[i,j];
            }
        }
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

    public List<Move> GetMoves()
    {
        List<Move> list = new List<Move>();

        for (int i = 0; i < board.GetLength(0); i++)
        {
            for(int j = 0; j < board.GetLength(1); j++)
            {
                int value = board[i,j];
                if(value == 0)
                {
                    list.Add(new Move(i, j));
                }
            }
        }

        return list;
    }
}
