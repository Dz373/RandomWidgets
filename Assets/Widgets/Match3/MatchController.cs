using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class MatchController : MonoBehaviour {
    public int rows;
    public int columns;
    public List<Color> itemColors;
    public bool finishTurn;
    public int moves;

    private MatchItem selectedItem;
    private Vector2Int[] adjCells = { Vector2Int.up, Vector2Int.down, Vector2Int.right, Vector2Int.left };
    public MatchItem[,] items;


    [Header("Object References")]
    [SerializeField] private Transform itemContainer;
    [SerializeField] private GameObject matchItem;

    private void Start() {
        CreateNewGrid();
    }

    private void CreateNewGrid() {
        items = new MatchItem[columns, rows];

        for (int c = 0; c < columns; c++) {
            for (int r = 0; r < rows; r++) {
                GenerateItem(c, r, false);
            }
        }

        Stack<Vector3Int> board = ScoreEntireBoard();
        while(board.Count > 0) {
            foreach (Vector3Int v in board) {
                items[v.x, v.y].ChangeItem(itemColors[Random.Range(0, itemColors.Count)]);
            }
            board = ScoreEntireBoard();
        }

        finishTurn = true;
    }

    private void GenerateItem(int c, int r, bool moveDown) {
        GameObject newItem = Instantiate(matchItem, itemContainer);
        MatchItem item = newItem.GetComponent<MatchItem>();

        item.itemColor = itemColors[Random.Range(0, itemColors.Count)];
        if (moveDown) {
            int row = GetNumberBelow(c, rows - 1);
            item.itemPos = new Vector3Int(c, row);
            items[c, row] = item;
        }
        else {
            item.itemPos = new Vector3Int(c, r);
            items[c, r] = item;
        }
    }

    public IEnumerator SelectItem(MatchItem item) {
        if (!finishTurn)
            yield break;

        if (selectedItem) {
            if (!IsAdjacent(item.itemPos))
                yield break;
            selectedItem.highlight.SetActive(false);

            Vector3Int temp = selectedItem.itemPos;
            selectedItem.MoveItem(item.itemPos);
            item.MoveItem(temp);
            moves--;

            yield return new WaitForSeconds(0.3f);

            if(ScoreEntireBoard().Count == 0) {
                temp = selectedItem.itemPos;
                selectedItem.MoveItem(item.itemPos);
                item.MoveItem(temp);
                moves++;

                yield return new WaitForSeconds(0.2f);
            }

            StartCoroutine(ScoreBoard());
            
            selectedItem = null;
        }
        else {
            selectedItem = item;
            selectedItem.highlight.SetActive(true);
        }
    }

    private Stack<Vector3Int> GetSameItemsInLine(int c, int r) {
        Stack<MatchItem> rowItems = new Stack<MatchItem>();
        Stack<MatchItem> colItems = new Stack<MatchItem>();

        for (int i = c - 1; i >= 0; i--) {
            if (items[i, r].itemColor == items[c, r].itemColor) 
                rowItems.Push(items[i, r]);
            else break;
        }
        for (int i = c + 1; i < columns; i++) {
            if (items[i, r].itemColor == items[c, r].itemColor) 
                rowItems.Push(items[i, r]);
            else break;
        }

        for (int i = r - 1; i >= 0; i--) {
            if (items[c, i].itemColor == items[c, r].itemColor) 
                colItems.Push(items[c, i]);
            else break;
        }
        for (int i = r + 1; i < rows; i++) {
            if (items[c, i].itemColor == items[c, r].itemColor) 
                colItems.Push(items[c, i]);
            else break;
        }

        Stack<Vector3Int> total = new Stack<Vector3Int>();
        total.Push(new Vector3Int(c, r));
        if (rowItems.Count >= 2) {
            while (rowItems.Count > 0)
                total.Push(rowItems.Pop().itemPos);
        }
        if (colItems.Count >= 2) {
            while (colItems.Count > 0)
                total.Push(colItems.Pop().itemPos);
        }

        return total;
    }

    private IEnumerator ScoreBoard() {
        finishTurn = false;

        Stack<Vector3Int> board = ScoreEntireBoard();

        while (board.Count > 0) {
            ScoreItems(board);

            yield return new WaitForSeconds(rows * 0.2f);

            board = ScoreEntireBoard();
        }

        finishTurn = true;
    }

    private Stack<Vector3Int> ScoreEntireBoard() {
        Stack<Vector3Int> itemsToScore = new Stack<Vector3Int>();

        foreach (MatchItem i in items) {
            Stack<Vector3Int> itemsStack = GetSameItemsInLine(i.itemPos.x, i.itemPos.y);
            if(itemsStack.Count >= 3) {
                while (itemsStack.Count > 0) {
                    Vector3Int nextItem = itemsStack.Pop();
                    if (!itemsToScore.Contains(nextItem))
                        itemsToScore.Push(nextItem);
                }
            }
        }

        return itemsToScore;
    }

    public void ScoreItems(Stack<Vector3Int> list) {
        List<Vector3Int> moveVectors = new List<Vector3Int>();
        List<int> destroyed = new List<int>();

        if (list.Count >= 3) {
            while (list.Count > 0) {
                Vector3Int i = list.Pop();

                if (moveVectors.Contains(i))
                    moveVectors.Remove(i);

                Destroy(items[i.x, i.y].gameObject);
                items[i.x, i.y] = null;
                destroyed.Add(i.x);

                for (int r = i.y + 1; r < rows; r++) {
                    Vector3Int newVec = new Vector3Int(i.x, r);
                    if (items[i.x, r] == null)
                        break;
                    else if (moveVectors.Contains(newVec))
                        break;
                    else
                        moveVectors.Add(newVec);
                }
            }
        }

        moveVectors.Sort((a, b) => a.y.CompareTo(b.y));
        foreach (Vector3Int i in moveVectors) {
            MatchItem moveItem = items[i.x, i.y];

            items[i.x, i.y] = null;
            moveItem.MoveItem(new Vector3Int(i.x, GetNumberBelow(i.x, i.y)));
        }

        foreach (int c in destroyed) {
            GenerateItem(c, rows-1, true);
        }
    }

    private int GetNumberBelow(int col, int row) {
        int availableRow = row;
        for (int i = row - 1; i >= 0; i--) {
            if (items[col, i] == null) {
                availableRow = i;
            }
            else
                break;
        }
        return availableRow;
    }

    private bool IsAdjacent(Vector3Int pos) {
        foreach (Vector3Int dir in adjCells) {
            if (selectedItem.itemPos + dir == pos)
                return true;
        }
        return false;
    }
}
