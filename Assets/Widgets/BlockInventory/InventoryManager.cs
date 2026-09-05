using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    public int rows;
    public int columns;

    [SerializeField] private RectTransform parentTransform;
    [SerializeField] private GridLayoutGroup gridLayout;
    [SerializeField] private GameObject gridSpace;

    public Dictionary<Vector2Int, BlockItem> grid;

    private void Start() {
        CreateNewInventory();
    }

    private void CreateNewInventory() {
        gridLayout.constraintCount = columns;

        parentTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, columns * 100 + 10);
        parentTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, rows * 100 + 10);

        grid = new Dictionary<Vector2Int, BlockItem>();

        for (int i = transform.childCount - 1; i >= 0; i--) {
            Destroy(transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < columns * rows; i++) {
            Instantiate(gridSpace, transform);
        }
    }
}
