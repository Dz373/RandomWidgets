using UnityEngine;
using UnityEngine.EventSystems;

public class GridSpace : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler
{
    private InventoryManager gm;

    private void Start() {
        gm = FindFirstObjectByType<InventoryManager>();
    }

    public void OnPointerEnter(PointerEventData eventData) {
        gm.hoverTile = gameObject;
    }

    public void OnPointerDown(PointerEventData eventData) {
        if (gm.selectedItem.moving)
            gm.selectedItem.OnPointerDown(eventData);
    }
}
