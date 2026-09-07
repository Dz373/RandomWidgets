using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour, IPointerDownHandler
{
    public int rows;
    public int columns;

    [SerializeField] private RectTransform parentTransform;
    [SerializeField] private GridLayoutGroup gridLayout;
    [SerializeField] private GameObject gridSpace;

    public Dictionary<Vector2Int, BlockItem> grid;

    public BlockItem selectedItem;
    public GameObject hoverTile;

    private void Start() {
        CreateNewInventory();
    }

    private void CreateNewInventory() {
        gridLayout.constraintCount = columns;

        float w = columns * gridLayout.cellSize.x + (columns - 1) * gridLayout.spacing.x + 2 * gridLayout.padding.top;
        float l = rows * gridLayout.cellSize.y + (rows - 1) * gridLayout.spacing.y + 2 * gridLayout.padding.top;
        parentTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, w);
        parentTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, l);

        grid = new Dictionary<Vector2Int, BlockItem>();

        for (int i = gridLayout.transform.childCount - 1; i >= 0; i--) {
            Destroy(gridLayout.transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < columns * rows; i++) {
            Instantiate(gridSpace, gridLayout.transform);
        }
    }

    public void OnPointerDown(PointerEventData eventData) {
        if (selectedItem && selectedItem.moving) {
            selectedItem.DeselectItem(hoverTile != null);
            selectedItem = null;
        }
    }
}
