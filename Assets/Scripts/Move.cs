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

    public Move(int row, int col)
    {
        Row = row;
        Col = col;
    }

    public override string ToString()
    {
        return $"Row: {row}, Col: {col}";
    }
}
