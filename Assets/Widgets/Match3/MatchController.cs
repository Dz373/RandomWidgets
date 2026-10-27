using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class MatchController : MonoBehaviour
{
    public int rows;
    public int columns;
    public List<Color> itemColors;

    private MatchItem selectedItem;
    private Vector2Int[] adjCells = {Vector2Int.up, Vector2Int.down, Vector2Int.right, Vector2Int.left };
    public MatchItem[,] items;

    [Header("Object References")]
    [SerializeField] private Transform itemContainer;
    [SerializeField] private GameObject matchItem;

    private void Start() {
        CreateNewGrid();
    }

    private void CreateNewGrid() {
        items = new MatchItem[rows, columns];

        for (int i = 0; i < rows; i++) {
            for(int j = 0; j < columns; j++) {
                GenerateItem(i, j);
            }
        }
    }

    private void GenerateItem(int r, int c) {
        GameObject newItem = Instantiate(matchItem, itemContainer);
        MatchItem item = newItem.GetComponent<MatchItem>();

        item.itemColor = itemColors[Random.Range(0, itemColors.Count)];
        item.itemPos = new Vector3Int(r, c);

        items[r, c] = item;
    }

    public void SelectItem(MatchItem item) {
        if (selectedItem) {
            if (!IsAdjacent(item.itemPos))
                return;

            Vector3Int temp = selectedItem.itemPos;
            selectedItem.MoveItem(item.itemPos);
            item.MoveItem(temp);

            List<Vector3Int> items1 = GetSameItemsInLine(item.itemPos.x, item.itemPos.y);
            if(items1.Count >= 3) {
                foreach (Vector3Int i in items1) {
                    Destroy(items[i.x, i.y].gameObject);

                    GenerateItem(i.x, i.y);
                }
            }
            List<Vector3Int> items2 = GetSameItemsInLine(selectedItem.itemPos.x, selectedItem.itemPos.y);
            if (items2.Count >= 3) {
                foreach (Vector3Int i in items2) {
                    Destroy(items[i.x, i.y].gameObject);
                    
                    GenerateItem(i.x, i.y);
                }
            }

            selectedItem.highlight.SetActive(false);
            selectedItem = null;
        }
        else {
            selectedItem = item;
            selectedItem.highlight.SetActive(true);
        }
    }

    private List<Vector3Int> GetSameItemsInLine(int r, int c) {
        List<MatchItem> rowItems = new List<MatchItem>();
        List<MatchItem> colItems = new List<MatchItem>();

        for(int i = r-1; i >= 0; i--) {
            if (items[i, c].itemColor == items[r, c].itemColor)
                rowItems.Add(items[i, c]);
            else
                break;
        }
        for (int i = r+1; i < rows; i++) {
            if (items[i, c].itemColor == items[r, c].itemColor)
                rowItems.Add(items[i, c]);
            else
                break;
        }
        for (int i = c - 1; i >= 0; i--) {
            if (items[r, i].itemColor == items[r, c].itemColor)
                colItems.Add(items[r, i]);
            else
                break;
        }
        for (int i = c + 1; i < rows; i++) {
            if (items[r, i].itemColor == items[r, c].itemColor)
                colItems.Add(items[r, i]);
            else
                break;
        }

        List<Vector3Int> total = new List<Vector3Int>();
        total.Add(new Vector3Int(r, c));
        if(rowItems.Count >= 2) {
            foreach (MatchItem i in rowItems)
                total.Add(i.itemPos);
        }
        if(colItems.Count >= 2) {
            foreach (MatchItem i in colItems)
                total.Add(i.itemPos);
        }

        return total;
    }

    private bool IsAdjacent(Vector3Int pos) {
        foreach(Vector3Int dir in adjCells) {
            if (selectedItem.itemPos + dir == pos)
                return true;
        }
        return false;
    }
}
