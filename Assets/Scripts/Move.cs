using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move 
{
    private int row;
    public int Row
    {
        get { return row; }
        set { row = value; }
    }

    private int col;
    public int Col
    {
        get { return col; }
        set { col = value; }
    }

    private int score;
    public int Score
    {
        get { return score; }
        set { score = value; }
    }

    public Move(int row, int col, int score)
    {
        Row = row;        
        Col = col;        
        Score = score;        
    }

    public Move(int row, int col)
    {
        Row = row;
        Col = col;
    }

    public override string ToString()
    {
        return $"Row: {row}, Col: {col}, Score: {score}";
    }
}
