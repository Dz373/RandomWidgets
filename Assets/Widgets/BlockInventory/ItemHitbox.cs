using UnityEngine;
using UnityEngine.EventSystems;

public class ItemHitbox : MonoBehaviour, IPointerDownHandler
{
    private BlockItem item;
    private Vector3 boxPosition;

    public void OnPointerDown(PointerEventData eventData) {
        item.SelectItem(boxPosition);
    }

    public void InstantiateHitbox(Vector3 pos) {
        item = GetComponentInParent<BlockItem>();
        boxPosition = pos*100;
        GetComponent<RectTransform>().transform.localPosition = boxPosition;
    }
}
