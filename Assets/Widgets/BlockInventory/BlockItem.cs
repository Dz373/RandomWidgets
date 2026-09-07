using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class BlockItem : MonoBehaviour {
    public bool moving;
    public Vector2[] boxPositions;

    [Header("References")]
    [SerializeField] private RectTransform rect;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private GameObject itemHitbox;
    [SerializeField] private GameObject hitboxContainer;

    private InventoryManager gm;
    private List<GameObject> hitboxes = new List<GameObject>();

    private void Start() {
        gm = FindFirstObjectByType<InventoryManager>();

        foreach (Vector3 pos in boxPositions) {
            GameObject hitbox = Instantiate(itemHitbox, hitboxContainer.transform);
            hitbox.GetComponent<ItemHitbox>().InstantiateHitbox(pos);
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
        moving = true;
        gm.selectedItem = this;
        canvasGroup.blocksRaycasts = false;
        hitboxContainer.GetComponent<RectTransform>().localPosition = -selectedBox;
    }

    public void DeselectItem(bool validGrid) {
        canvasGroup.blocksRaycasts = true;
        moving = false;

        if (validGrid) {
            transform.position = gm.hoverTile.transform.position;
        }

    }
}