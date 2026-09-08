using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class BlockItem : MonoBehaviour {
    public bool moving;
    public bool inInventory;
    public Color color;
    public Vector3[] boxPositions;

    [Header("References")]
    [SerializeField] private RectTransform rect;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private GameObject itemHitbox;
    [SerializeField] private GameObject hitboxContainer;

    private InventoryManager gm;
    private List<GameObject> hitboxes = new List<GameObject>();
    private Vector3 selectOffset;

    private void Start() {
        gm = FindFirstObjectByType<InventoryManager>();

        foreach (Vector3 pos in boxPositions) {
            GameObject hitbox = Instantiate(itemHitbox, hitboxContainer.transform);
            hitbox.GetComponent<ItemHitbox>().InstantiateHitbox(pos, color);
            hitboxes.Add(hitbox);
        }
    }

    private void Update() {
        if (moving) {
            transform.position = Input.mousePosition;

            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (scroll != 0) {
                if(scroll > 0)
                    transform.RotateAround(transform.position, Vector3.forward, 90f);
                else
                    transform.RotateAround(transform.position, Vector3.forward, -90f);
            }
        }
    }

    public void SelectItem(Vector3 selectedBox) {
        if (gm.selectedItem != null) {
            if(!gm.IsInBounds(rect.localPosition))
                gm.OnPointerDown(null);
            return;
        }

        if (inInventory) {
            gm.RemoveFromInventory(GetBoxCords());
        }

        moving = true;
        gm.selectedItem = this;
        canvasGroup.blocksRaycasts = false;
        selectOffset = -selectedBox;
        hitboxContainer.GetComponent<RectTransform>().localPosition = selectOffset;
    }

    public bool DeselectItem() {
        if (gm.hoverTile) {
            transform.position = gm.hoverTile.transform.position;
            Vector3[] itemSpaces = GetBoxCords();

            if (gm.IsValidSpaces(itemSpaces)) {
                gm.PlaceInInventory(itemSpaces, this);
                inInventory = true;
            }
            else
                return false;
        }

        canvasGroup.blocksRaycasts = true;
        moving = false;
        return true;
    }

    private Vector3[] GetBoxCords() {
        Vector3[] cords = new Vector3[boxPositions.Length];

        for(int i = 0; i < boxPositions.Length; i++) {
            cords[i] = rect.localPosition + boxPositions[i] * 100 + selectOffset;
        }

        return cords;
    }
}