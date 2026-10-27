using UnityEngine;

public class MatchItem : MonoBehaviour
{
    public Color itemColor;
    public Vector3Int itemPos;

    public GameObject highlight;
    private MatchController gm;

    private void Start() {
        transform.localPosition = itemPos;
        GetComponent<SpriteRenderer>().color = itemColor;
        gm = FindFirstObjectByType<MatchController>();
    }

    private void OnMouseDown() {
        gm.SelectItem(this);
    }

    public void MoveItem(Vector3Int pos) {
        itemPos = pos;
        transform.localPosition = pos;
        gm.items[itemPos.x, itemPos.y] = this;
    }
}
