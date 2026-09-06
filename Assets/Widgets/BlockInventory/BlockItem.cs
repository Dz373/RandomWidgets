using UnityEngine;
using UnityEngine.EventSystems;

public class BlockItem : MonoBehaviour, IPointerDownHandler
{
    public bool moving;

    [SerializeField] private RectTransform rect;
    [SerializeField] private CanvasGroup canvasGroup;
    private InventoryManager gm;
    
    private void Start() {
        gm = FindFirstObjectByType<InventoryManager>();
    }
    
    private void Update() {
        if (moving) {
            transform.position = Input.mousePosition;
        }
    }

    public void OnPointerDown(PointerEventData eventData) {
        moving = !moving;

        if (moving) {
            gm.selectedItem = this;
            canvasGroup.blocksRaycasts = false;
        }
        else {
            gm.selectedItem = null;
            canvasGroup.blocksRaycasts = true;

            transform.position = gm.hoverTile.transform.position;
        }
    }
}