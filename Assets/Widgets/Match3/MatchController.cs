using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class MatchController : MonoBehaviour
{
    public int rows;
    public int columns;
    public List<Color> itemColors;

    [Header("Object References")]
    [SerializeField] private Transform itemContainer;
    [SerializeField] private GameObject item;

    private void Start() {
        CreateNewGrid();
    }

    private void CreateNewGrid() {
        for (int i = 0; i < rows; i++) {
            for(int j = 0; j < columns; j++) {
                GameObject newItem = Instantiate(item, itemContainer);
                MatchItem matchItem = newItem.GetComponent<MatchItem>();

                matchItem.itemColor = itemColors[Random.Range(0, itemColors.Count)];
                matchItem.itemPos = new Vector2(i, j);
            }
        }
    }
}
