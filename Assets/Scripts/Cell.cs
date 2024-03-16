using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Cell : MonoBehaviour
{
    [SerializeField]
    private GameManager gameManager;

    [SerializeField]
    private int row;
    public int Row
    {
        get { return row; }
    }

    [SerializeField]
    private int column;
    public int Column
    {
        get {return column; }
    }

    private TMP_Text label;
    public TMP_Text Label
    {
        get { return label; }
        set { label = value; }
    }

    private void Awake()
    {
        label = GetComponentInChildren<TMP_Text>();
    }

    public void PlayerUpdateCell()
    {
        label.text = "X";
        gameManager.UpdateGameState(new Move(row, column));
    }

    public void UpdateCell(bool playerTurn) 
    {
        if (gameManager.PlayerTurn)
        {
            label.text = "X";
        }
        else
        {
            label.text = "O";
        }
    }

    public void UpdateCell()
    {
        label.text = "O";
    }
}
