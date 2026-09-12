using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour, IPointerDownHandler
{
    public int rows;
    public int columns;
    public SelectMode itemSelectMode;

    [SerializeField] private RectTransform parentTransform;
    [SerializeField] private GridLayoutGroup gridLayout;
    [SerializeField] private GameObject gridSpace;

    public Dictionary<Vector3, BlockItem> grid;

    public BlockItem selectedItem;
    public GameObject hoverTile;

    public enum SelectMode { 
        click,
        hold
    }

    private void Start() {
        CreateNewInventory();
    }

    private void CreateNewInventory() {
        gridLayout.constraintCount = columns;

        float w = columns * gridLayout.cellSize.x + (columns - 1) * gridLayout.spacing.x + 2 * gridLayout.padding.top;
        float l = rows * gridLayout.cellSize.y + (rows - 1) * gridLayout.spacing.y + 2 * gridLayout.padding.top;
        parentTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, w);
        parentTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, l);

        grid = new Dictionary<Vector3, BlockItem>();

        for (int i = gridLayout.transform.childCount - 1; i >= 0; i--) {
            Destroy(gridLayout.transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < columns * rows; i++) {
            Instantiate(gridSpace, gridLayout.transform);
        }

        for (int r = 0; r < rows; r++) {
            int y = -(rows - 1) * 50 + 100 * r;
            for (int c = 0; c < columns; c++) {
                int x = -(columns - 1) * 50 + 100 * c;
                grid.Add(new Vector3(y, x), null);
            }
        }
    }
    
    public void PlaceInInventory(Vector3[] cords, BlockItem item) {
        foreach (Vector3 v in cords) {
            grid[v] = item;
        }
    }

    public void RemoveFromInventory(Vector3[] cords) {
        foreach (Vector3 v in cords) {
            grid[v] = null;
        }
    }

    public void OnPointerDown(PointerEventData eventData) {
        if (selectedItem && selectedItem.moving) {
            if(selectedItem.DeselectItem())
                selectedItem = null;
        }
    }

    public bool IsValidSpaces(Vector3[] cords) {
        foreach(Vector3 v in cords) {
            if (!grid.ContainsKey(v))
                return false;
            if (grid[v])
                return false;
        }
        return true;
    }

    public bool IsInBounds(Vector3 pos) {
        float w = parentTransform.rect.width/2;
        float h = parentTransform.rect.height/2;
        return pos.x > -w && pos.x < w && pos.y > -h && pos.y < h;
    }
}
