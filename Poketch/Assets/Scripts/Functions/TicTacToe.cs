using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TicTacToe : Function
{
    enum CellState
    {
        Blank,
        Eevee,
        Pikachu
    }

    CellState[,] board = new CellState[3, 3];
    CellState currentTurn = CellState.Pikachu;

    [Header("Result Table")]
    [SerializeField] Image currentTurnImage;
    [SerializeField] Text pikaWins;
    [SerializeField] Text eeveeWins;

    [Header("Images")]
    [SerializeField] Sprite pika;
    [SerializeField] Sprite eevee;

    [Header("Win Menu")]
    [SerializeField] GameObject winMenu;
    [SerializeField] Image pokeWinImage;
    [SerializeField] GameObject drawText;
    [SerializeField] GameObject winText;

    [Header("Buttons")]
    [SerializeField] CanvasGroup buttons;
    [SerializeField] Button resetButton;

    [Header("Win Lines")]
    [SerializeField] GameObject[] lines;

    Button[] tableButtons = new Button[9];

    int moves = 0;
    int pikaWons = 0;
    int eeveeWons = 0;

    public override void OnCreate(JSONNode jsonObject)
    {
        for (int i = 0; i < buttons.transform.childCount; ++i)
        {
            tableButtons[i] = buttons.transform.GetChild(i).GetComponent<Button>();
        }

        if (jsonObject != null)
        {
            pikaWons = jsonObject.GetValueOrDefault("pikaWins", 0);
            eeveeWons = jsonObject.GetValueOrDefault("eeveeWins", 0);

            pikaWins.text = pikaWons.ToString();
            eeveeWins.text = eeveeWons.ToString();
        }
    }

    public void CellClicked(Button buttonPushed)
    {
        ++moves;
        buttonPushed.interactable = false;
        SoundManager.Instance.PlaySFX(SoundManager.SFX.ResetCounter);

        float p = int.Parse(buttonPushed.gameObject.name) * (1.0f / 3.0f);
        int row = Mathf.FloorToInt(p);
        int column = Mathf.RoundToInt((p - row) * 3);

        board[row, column] = currentTurn;
        buttonPushed.image.sprite = currentTurn == CellState.Pikachu ? pika : eevee;

        int result = CheckWinCondition(row, column);

        if (result != -1)
        {
            switch (result)
            {
                case 0:
                    StartCoroutine(WinLineAnim(lines[row]));
                    break;
                case 1:
                    StartCoroutine(WinLineAnim(lines[3 + column]));
                    break;
                case 2:
                    StartCoroutine(WinLineAnim(lines[6]));
                    break;
                case 3:
                    StartCoroutine(WinLineAnim(lines[7]));
                    break;
            }

            buttons.interactable = false;
            resetButton.interactable = false;

            switch (currentTurn)
            {
                case CellState.Blank:
                    break;
                case CellState.Eevee:
                    ++eeveeWons;
                    eeveeWins.text = eeveeWons.ToString();
                    break;
                case CellState.Pikachu:
                    ++pikaWons;
                    pikaWins.text = pikaWons.ToString();
                    break;
            }

            SoundManager.Instance.PlaySFX(SoundManager.SFX.Refresh);

            SaveResults();

            return;
        }

        if (moves > 8)
        {
            buttons.interactable = false;
            winMenu.SetActive(true);
            drawText.SetActive(true);
            winText.SetActive(false);
            pokeWinImage.gameObject.SetActive(false);
        }

        currentTurn = currentTurn == CellState.Pikachu ? CellState.Eevee : CellState.Pikachu;
        currentTurnImage.sprite = currentTurn == CellState.Pikachu ? pika : eevee;
    }

    int CheckWinCondition(int row, int column)
    {
        Vector2Int result = new Vector2Int(-1, -1);

        //check columns
        for (int i = 0; i < 3; ++i)
        {
            if (board[row, i] != currentTurn) break;

            if (i == 2)
            {
                return 0;
            }
        }

        //check rows
        for (int i = 0; i < 3; ++i)
        {
            if (board[i, column] != currentTurn) break;

            if (i == 2)
            {
                return 1;
            }
        }

        //check diagional
        if (row == column)
        {
            //we're on a diagonal
            for (int i = 0; i < 3; ++i)
            {
                if (board[i, i] != currentTurn) break;

                if (i == 2)
                {
                    return 2;
                }
            }
        }

        //check anti diagonal
        if (row + column == 2)
        {
            for (int i = 0; i < 3; ++i)
            {
                if (board[i, 2 - i] != currentTurn) break;

                if (i == 2)
                {
                    return 3;
                }
            }
        }

        return -1;
    }

    public void ResetInfo()
    {
        pikaWons = 0;
        eeveeWons = 0;

        pikaWins.text = "0";
        eeveeWins.text = "0";
        SaveResults();
    }

    void SaveResults()
    {
        JSONNode json = new JSONObject();
        json.Add("pikaWins", pikaWons);
        json.Add("eeveeWins", eeveeWons);
        FunctionController.Instance.SaveFunctionInfo(GetType().Name, json);
    }

    public void StartOver()
    {
        winMenu.SetActive(false);
        buttons.interactable = true;
        moves = 0;

        for (int i = 0; i < 3; ++i)
        {
            for (int j = 0; j < 3; ++j)
            {
                board[i, j] = CellState.Blank;
            }
        }

        for (int i = 0; i < tableButtons.Length; ++i)
        {
            tableButtons[i].image.sprite = null;
            tableButtons[i].interactable = true;
        }
    }

    IEnumerator WinLineAnim(GameObject line)
    {
        for (int i = 0; i < 6; ++i)
        {
            line.SetActive(!line.activeSelf);
            yield return new WaitForSeconds(0.5f);
        }

        winMenu.SetActive(true);
        pokeWinImage.sprite = currentTurn == CellState.Pikachu ? pika : eevee;
        drawText.SetActive(false);
        winText.SetActive(true);
        pokeWinImage.gameObject.SetActive(true);
        resetButton.interactable = true;
    }
}
