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
        if(Board[move.Row, move.Col] == '\0')
        {
            Board[move.Row, move.Col] = value;
        }        
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

    public List<GameState> GetNextStates(char player)
    {
        var result = new List<GameState>();

        for (var x = 0; x < 3; x++)
        {
            for (var y = 0; y < 3; y++)
            {
                if (board[x, y] != '\0')
                {
                    continue;
                }

                var possibleState = (char[,]) board.Clone();
                possibleState[x, y] = player;
                result.Add(new GameState(possibleState, new Move(x,y)));
            }
        }

        return result;
    }
}
