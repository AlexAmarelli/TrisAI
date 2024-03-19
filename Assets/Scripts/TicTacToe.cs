using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TicTacToe : MonoBehaviour
{
    [SerializeField] private Button[] buttons;

    //[SerializeField] private TMP_Text gameStatus;

    [Header("Endgame message")]

    [SerializeField]
    private GameObject endgamePanel;

    [SerializeField]
    private TMP_Text endgameDescription;

    [SerializeField] private bool AIStart;

    private readonly char[,] board = new char[3, 3];

    private void Awake()
    {
        if (AIStart)
            MakeAIMove();
    }

    public void PlayerMove()
    {
        GameObject selectedButton = EventSystem.current.currentSelectedGameObject;
        int index = Array.IndexOf(buttons, selectedButton.GetComponent<Button>());
        int x = index / 3;
        int y = index % 3;

        if (board[x, y] == '\0') // Check if cell is empty
        {
            board[x, y] = 'X'; // 'X' for the player
            UpdateButtonUI(x, y, 'X');
            CheckEndgame();

            MakeAIMove();
        }
    }

    void MakeAIMove()
    {
        int bestScore = int.MaxValue;
        int[] bestMove = { -1, -1 };

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (board[i, j] == '\0')
                {
                    board[i, j] = 'O';
                    int score = Minimax(board, 0, true);
                    board[i, j] = '\0';

                    if (score < bestScore)
                    {
                        bestScore = score;
                        bestMove[0] = i;
                        bestMove[1] = j;
                    }
                }
            }
        }

        // AI best move
        if (bestMove[0] != -1)
        {
            board[bestMove[0], bestMove[1]] = 'O';
            UpdateButtonUI(bestMove[0], bestMove[1], 'O');
        }

        CheckEndgame();
    }

    public void UpdateButtonUI(int x, int y, char playerSymbol)
    {
        int index = x * 3 + y;
        buttons[index].GetComponentInChildren<TextMeshProUGUI>().text = playerSymbol.ToString();
        buttons[index].interactable = false;
    }

    int Evaluate(char[,] board)
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

    int Minimax(char[,] board, int depth, bool isMaximizingPlayer)
    {
        int score = Evaluate(board);

        if (score == 10) return score ;  // X won
        if (score == -10) return score ; // O won
        if (!isMovesLeft(board)) return 0;

        if (isMaximizingPlayer)
        {
            int bestScore = int.MinValue;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (board[i, j] == '\0')
                    {
                        board[i, j] = 'X';
                        bestScore = Mathf.Max(bestScore, Minimax(board, depth + 1, !isMaximizingPlayer));
                        board[i, j] = '\0';
                    }
                }
            }
            return bestScore;
        }
        else
        {
            int bestScore = int.MaxValue;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (board[i, j] == '\0')
                    {
                        board[i, j] = 'O';
                        bestScore = Mathf.Min(bestScore, Minimax(board, depth + 1, !isMaximizingPlayer));
                        board[i, j] = '\0';
                    }
                }
            }
            return bestScore;
        }
    }

    bool isMovesLeft(char[,] board)
    {
        // Check if game is not over
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                // Check if at least one cell is not filled with 'X' or 'O'
                if (board[i, j] == '\0')
                    return true;
            }
        }
        return false;
    }

    private void CheckEndgame()
    {
        int endgame = Evaluate(board);

        if (endgame > 0)
        {
            endgameDescription.text = "YOU WON!";
            endgamePanel.SetActive(true);
            //gameEnded = true;
        }
        else if (endgame < 0)
        {
            endgameDescription.text = "AI WON!";
            endgamePanel.SetActive(true);
            //gameEnded = true;
        }
        else if (!isMovesLeft(board))
        {
            endgameDescription.text = "DRAW!";
            endgamePanel.SetActive(true);
            //gameEnded = true;
        }
    }

    private void DisableAllButtons()
    {
        for (var i = 0; i < buttons.Length; i++) buttons[i].interactable = false;
    }

    private IEnumerator Reload()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("Main");
    }
}
