using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    private Button button;

    private void Awake()
    {
        label = GetComponentInChildren<TMP_Text>();
        button = GetComponentInChildren<Button>();
    }

    public void PlayerUpdateCell()
    {
        label.text = gameManager.PlayerChar.ToString();
        button.interactable = false;
        gameManager.PlayerMove(new Move(row, column));
    }

    public void UpdateCell(bool playerTurn) 
    {
        if (gameManager.PlayerTurn)
        {
            label.text = gameManager.PlayerChar.ToString();
            button.interactable = false;
        }
        else
        {
            label.text = gameManager.AIChar.ToString();
            button.interactable = false;
        }
    }

    public void UpdateCell()
    {
        label.text = gameManager.AIChar.ToString();
        button.interactable = false;
    }
}
