using UnityEngine;

public class MinesweeperManager : MonoBehaviour
{
    public int rows;
    public int columns;
    public int mines;
    
    public int[,] map;
    public MinesweeperButton[,] buttonMap;

    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private GameObject buttonContainers;

    private void Start() {
        map = CreateNewMap();
        print(Print2DArray(map));

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

    public void FlagCell(Vector2Int cell) {
        buttonMap[cell.x, cell.y].FlagCell();
    }

    public void CellPress(Vector2Int cell) {
        if (buttonMap[cell.x, cell.y].revealed || buttonMap[cell.x, cell.y].flagged)
            return;

        int cellVal = map[cell.x, cell.y];

        buttonMap[cell.x, cell.y].RevealCell(cellVal);

        if (cellVal == 0) {
            for (int r = cell.x - 1; r <= cell.x + 1; r++) {
                if (r < 0 || r >= rows)
                    continue;
                for (int c = cell.y - 1; c <= cell.y + 1; c++) {
                    if (c < 0 || c >= columns)
                        continue;

                    CellPress(new Vector2Int(r, c));
                }
            }
        }
    }

    private int[,] CreateNewMap() {
        int[,] newMap = new int[rows, columns];
        buttonMap = new MinesweeperButton[rows, columns];

        for (int i = 0; i < mines; i++)
            SetMine(newMap);

        for (int r = 0; r < rows; r++) {
            for (int c = 0; c < columns; c++) {
                if (newMap[r, c] == -1)
                    continue;

                newMap[r, c] = GetSurroundingMines(newMap, r, c);
            }
        }

        return newMap;
    }

    private void SetMine(int[,] newMap) {
        int r = Random.Range(0, rows);
        int c = Random.Range(0, columns);

        if (newMap[r, c] == -1)
            SetMine(newMap);

        newMap[r, c] = -1;
    }

    private int GetSurroundingMines(int[,] newMap, int row, int col) {
        int mines = 0;
        for (int r = row-1; r <= row+1; r++) {
            if (r < 0 || r >= rows)
                continue;
            for (int c = col-1; c <= col+1; c++) {
                if (c < 0 || c >= columns)
                    continue;

                if (newMap[r, c] == -1)
                    mines++;
            }
        }
        return mines;
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
