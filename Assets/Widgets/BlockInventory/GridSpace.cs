using UnityEngine;
using UnityEngine.EventSystems;

public class GridSpace : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private InventoryManager gm;

    private void Start() {
        gm = FindFirstObjectByType<InventoryManager>();
    }

    public void OnPointerEnter(PointerEventData eventData) {
        gm.hoverTile = gameObject;
    }
    public void OnPointerExit(PointerEventData eventData) {
        gm.hoverTile = null;
    }
}
