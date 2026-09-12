using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemHitbox : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private BlockItem item;
    private Vector3 boxPosition;

    public void OnPointerDown(PointerEventData eventData) {
        item.SelectItem(boxPosition);
    }

    public void OnPointerUp(PointerEventData eventData) {
        if(item.gm.itemSelectMode == InventoryManager.SelectMode.hold) {
            if (item.DeselectItem())
                item.gm.selectedItem = null;
        }
    }

    public void InstantiateHitbox(Vector3 pos, Color color) {
        item = GetComponentInParent<BlockItem>();
        boxPosition = pos*100;
        GetComponent<RectTransform>().transform.localPosition = boxPosition;
        GetComponent<Image>().color = color;
    }
}
