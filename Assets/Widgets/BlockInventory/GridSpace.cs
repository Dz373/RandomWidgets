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
        
        if(gm.selectedItem)
            gm.itemHighlight = gm.selectedItem.CreateHighlight(transform.position);
    }
    public void OnPointerExit(PointerEventData eventData) {
        gm.hoverTile = null;
        
        if(gm.itemHighlight)
            Destroy(gm.itemHighlight);
    }
}
