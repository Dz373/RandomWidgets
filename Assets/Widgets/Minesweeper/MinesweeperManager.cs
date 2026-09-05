using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UIElements;

public class MinesweeperManager : MonoBehaviour
{
    public int rows;
    public int columns;
    public int mines;
    private bool gameOver;
    
    public int[,] map;
    public MinesweeperButton[,] buttonMap;

    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private GameObject buttonContainers;
    [SerializeField] private TMP_InputField rowInput;
    [SerializeField] private TMP_InputField colInput;
    [SerializeField] private TMP_InputField mineInput;

    private void Start() {
        CreateNewMap();

        rowInput.text = rows.ToString();
        colInput.text = columns.ToString();
        mineInput.text = mines.ToString();
    }

    public void FlagCell(Vector2Int cell) {
        buttonMap[cell.x, cell.y].FlagCell();
    }

    public void CellPress(Vector2Int cell) {
        if (buttonMap[cell.x, cell.y].flagged || gameOver)
            return;

        if(buttonMap[cell.x, cell.y].revealed) {
            if(map[cell.x, cell.y] != 0) {
                List<Vector2Int> surroundingCells = GetSurroundingCells(cell.x, cell.y);
                Stack<Vector2Int> stack = new Stack<Vector2Int>();
                List<Vector2Int> usedCells = new List<Vector2Int>();
                int flags = 0;

                foreach (Vector2Int adjCell in surroundingCells) {
                    if (buttonMap[adjCell.x, adjCell.y].flagged)
                        flags++;
                    else {
                        stack.Push(adjCell);
                        usedCells.Add(adjCell);
                    }
                }

                if(flags == map[cell.x, cell.y]) {
                    while (stack.Count > 0) {
                        Vector2Int curCell = stack.Pop();
                        int curVal = map[curCell.x, curCell.y];
                        buttonMap[curCell.x, curCell.y].RevealCell(curVal);

                        if (curVal == 0) {
                            foreach (Vector2Int adjCell in GetSurroundingCells(curCell.x, curCell.y)) {
                                if (usedCells.Contains(adjCell))
                                    continue;
                                stack.Push(adjCell);
                                usedCells.Add(adjCell);
                            }
                        }
                        else if (curVal == -1) {
                            EndGame();
                        }
                    }
                }
            }

            return;
        }

        int cellVal = map[cell.x, cell.y];
        buttonMap[cell.x, cell.y].RevealCell(cellVal);

        if (cellVal == 0) {
            Stack<Vector2Int> stack = new Stack<Vector2Int>();
            List<Vector2Int> usedCells = new List<Vector2Int>();
            
            foreach (Vector2Int adjCell in GetSurroundingCells(cell.x, cell.y)) {
                stack.Push(adjCell);
                usedCells.Add(adjCell);
            }

            while (stack.Count > 0) {
                Vector2Int curCell = stack.Pop();
                int curVal = map[curCell.x, curCell.y];
                buttonMap[curCell.x, curCell.y].RevealCell(curVal);

                if(curVal == 0) {
                    foreach (Vector2Int adjCell in GetSurroundingCells(curCell.x, curCell.y)) {
                        if (usedCells.Contains(adjCell))
                            continue;
                        stack.Push(adjCell);
                        usedCells.Add(adjCell);
                    }
                }
            }
        }
        else if (cellVal == -1) {
            EndGame();
        }
    }

    public void StartNewGame() {
        for (int i = rows*columns - 1; i >= 0; i--) {
            Destroy(buttonContainers.transform.GetChild(i).gameObject);
        }

        rows = int.Parse(rowInput.text);
        columns = int.Parse(colInput.text);
        mines = int.Parse(mineInput.text);
        CreateNewMap();

        gameOver = false;
    }

    private void EndGame() {
        for (int r = 0; r < rows; r++) {
            for (int c = 0; c < columns; c++) {
                if (buttonMap[r, c].revealed || buttonMap[r, c].flagged)
                    continue;

                buttonMap[r, c].RevealCell(map[r, c]);
            }
        }
        gameOver = true;
    }

    private void CreateNewMap() {
        map = new int[rows, columns];
        buttonMap = new MinesweeperButton[rows, columns];

        for (int i = 0; i < mines; i++)
            SetMine();

        for (int r = 0; r < rows; r++) {
            for (int c = 0; c < columns; c++) {
                if (map[r, c] == -1)
                    continue;

                map[r, c] = GetSurroundingMines(r, c);
            }
        }

        for (int r = 0; r < rows; r++) {
            for (int c = 0; c < columns; c++) {
                GameObject newButton = Instantiate(buttonPrefab, buttonContainers.transform);
                buttonMap[r, c] = newButton.GetComponent<MinesweeperButton>();
                buttonMap[r, c].InitializeButton(r, c);
            }
        }

        foreach (MinesweeperButton b in buttonContainers.GetComponentsInChildren<MinesweeperButton>()) {
            b.button.onClick.AddListener(() => CellPress(b.pos));
            b.rightClickButton.onRightClick.AddListener(() => FlagCell(b.pos));
        }
    }

    private void SetMine() {
        int r = Random.Range(0, rows);
        int c = Random.Range(0, columns);

        if (map[r, c] == -1)
            SetMine();

        map[r, c] = -1;
    }

    private int GetSurroundingMines(int row, int col) {
        int mines = 0;
        foreach (Vector2Int cell in GetSurroundingCells(row, col)) {
            if (map[cell.x, cell.y] == -1)
                mines++;
        }
        return mines;
    }

    private List<Vector2Int> GetSurroundingCells(int row, int col) {
        List<Vector2Int> cells = new List<Vector2Int>();
        for (int r = row - 1; r <= row + 1; r++) {
            if (r < 0 || r >= rows)
                continue;
            for (int c = col - 1; c <= col + 1; c++) {
                if (c < 0 || c >= columns)
                    continue;
                if (c == col && r == row)
                    continue;

                cells.Add(new Vector2Int(r, c));
            }
        }

        return cells;
    }

    private string Print2DArray(int[,] arr) {
        string output = "";

        for (int i = 0; i < arr.GetLength(0); i++) {
            for (int j = 0; j < arr.GetLength(1); j++) {
                output += map[i, j] + " ";
            }
            output += "\n";
        }

        return output;
    }
}
