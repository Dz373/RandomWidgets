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
    private Vector2[] adjCells = {Vector2.up, Vector2.down, Vector2.right, Vector2.left };

    [Header("Object References")]
    [SerializeField] private Transform itemContainer;
    [SerializeField] private GameObject matchItem;

    private void Start() {
        CreateNewGrid();
    }

    private void CreateNewGrid() {
        for (int i = 0; i < rows; i++) {
            for(int j = 0; j < columns; j++) {
                GameObject newItem = Instantiate(matchItem, itemContainer);
                MatchItem item = newItem.GetComponent<MatchItem>();

                item.itemColor = itemColors[Random.Range(0, itemColors.Count)];
                item.itemPos = new Vector2(i, j);
            }
        }
    }

    public void SelectItem(MatchItem item) {
        if (selectedItem) {
            if (!IsAdjacent(item.itemPos))
                return;

            Vector2 temp = selectedItem.itemPos;
            selectedItem.MoveItem(item.itemPos);
            item.MoveItem(temp);

            selectedItem.highlight.SetActive(false);
            selectedItem = null;
        }
        else {
            selectedItem = item;
            selectedItem.highlight.SetActive(true);

        }
    }

    private bool IsAdjacent(Vector2 pos) {
        foreach(Vector2 dir in adjCells) {
            if (selectedItem.itemPos + dir == pos)
                return true;
        }
        return false;
    }
}
