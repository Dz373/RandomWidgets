using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class MatchController : MonoBehaviour {
    public int rows;
    public int columns;
    public List<Color> itemColors;
    public bool finishTurn;

    private MatchItem selectedItem;
    private Vector2Int[] adjCells = { Vector2Int.up, Vector2Int.down, Vector2Int.right, Vector2Int.left };
    public MatchItem[,] items;


    [Header("Object References")]
    [SerializeField] private Transform itemContainer;
    [SerializeField] private GameObject matchItem;

    private void Start() {
        CreateNewGrid();
    }

    private IEnumerator ScoreBoard() {
        finishTurn = false;

        Stack<Vector3Int> board = ScoreEntireBoard();
        while (board.Count > 0) {
            ScoreItems(board);
            
            yield return new WaitForSeconds(1f);

            board = ScoreEntireBoard();
        }
        
        finishTurn = true;
    }

    private void CreateNewGrid() {
        items = new MatchItem[rows, columns];

        for (int i = 0; i < columns; i++) {
            for (int j = 0; j < rows; j++) {
                GenerateItem(j, i, false);
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

    private void GenerateItem(int r, int c, bool moveDown) {
        GameObject newItem = Instantiate(matchItem, itemContainer);
        MatchItem item = newItem.GetComponent<MatchItem>();

        item.itemColor = itemColors[Random.Range(0, itemColors.Count)];
        if (moveDown) {
            int row = GetNumberBelow(c, rows - 1);
            item.itemPos = new Vector3Int(c, row);
            items[c, row] = item;
        }
        else {
            item.itemPos = new Vector3Int(r, c);
            items[r, c] = item;
        }
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

                for(int r = i.y+1; r < rows; r++) {
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
        
        foreach(Vector3Int i in moveVectors) {
            MatchItem moveItem = items[i.x, i.y];

            items[i.x, i.y] = null;
            moveItem.MoveItem(new Vector3Int(i.x, GetNumberBelow(i.x, i.y)));
        }

        foreach (int x in destroyed) {
            GenerateItem(rows - 1, x, true);
        }
    }

    private int GetNumberBelow(int x, int y) {
        int down = 0;
        for (int i = y - 1; i >= 0; i--) {
            if (items[x, i] != null)
                down++;
        }
        return down;
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

            yield return new WaitForSeconds(0.2f);

            StartCoroutine(ScoreBoard());
            
            selectedItem = null;
        }
        else {
            selectedItem = item;
            selectedItem.highlight.SetActive(true);
        }
    }

    private Stack<Vector3Int> GetSameItemsInLine(int r, int c) {
        Stack<MatchItem> rowItems = new Stack<MatchItem>();
        Stack<MatchItem> colItems = new Stack<MatchItem>();

        for (int i = r - 1; i >= 0; i--) {
            if (items[i, c].itemColor == items[r, c].itemColor)
                rowItems.Push(items[i, c]);
            else
                break;
        }
        for (int i = r + 1; i < rows; i++) {
            if (items[i, c].itemColor == items[r, c].itemColor)
                rowItems.Push(items[i, c]);
            else
                break;
        }
        for (int i = c - 1; i >= 0; i--) {
            if (items[r, i].itemColor == items[r, c].itemColor)
                colItems.Push(items[r, i]);
            else
                break;
        }
        for (int i = c + 1; i < rows; i++) {
            if (items[r, i].itemColor == items[r, c].itemColor)
                colItems.Push(items[r, i]);
            else
                break;
        }

        Stack<Vector3Int> total = new Stack<Vector3Int>();
        total.Push(new Vector3Int(r, c));
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

    private bool IsAdjacent(Vector3Int pos) {
        foreach (Vector3Int dir in adjCells) {
            if (selectedItem.itemPos + dir == pos)
                return true;
        }
        return false;
    }
}
