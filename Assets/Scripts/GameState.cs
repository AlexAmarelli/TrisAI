using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState 
{
    private char[,] state = new char[3, 3];
    public char[,] State
    {
        get { return state; }
        set { state = value; }
    }

    private Move move;
    public Move Move
    {
        get { return move; }
        set { move = value; }
    }

    public GameState(char[,] possibleState, Move move)
    {
        this.State = possibleState;
        this.move = move;
    }    
}
