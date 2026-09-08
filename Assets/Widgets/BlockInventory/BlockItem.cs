using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Collections;

public class BlockItem : MonoBehaviour {
    public bool moving;
    public bool inInventory;
    private bool rotating;
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
                if (rotating)
                    return;
                if (scroll > 0)
                    StartCoroutine(RotateItem(90f));
                else
                    StartCoroutine(RotateItem(-90f));
            }
        }
    }

    private IEnumerator RotateItem(float degree) {
        rotating = true;

        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, rect.rotation.eulerAngles.z + degree));
        while (Quaternion.Angle(transform.rotation, targetRotation) > 0.01f) {
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 30 * Time.deltaTime);
            yield return null;
        }

        transform.rotation = targetRotation;
        rotating = false;
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
            cords[i] = boxPositions[i]*100 + selectOffset;

            if (rect.rotation.eulerAngles.z == 90)
                cords[i] = new Vector3(-cords[i].y, cords[i].x);
            else if (rect.rotation.eulerAngles.z == 180)
                cords[i] = new Vector3(-cords[i].x, -cords[i].y);
            else if (rect.rotation.eulerAngles.z == 270)
                cords[i] = new Vector3(cords[i].y, -cords[i].x);

            cords[i] += rect.localPosition;
        }

        return cords;
    }
}