using UnityEngine;

public class MatchItem : MonoBehaviour
{
    public Color itemColor;
    public Vector2 itemPos;

    public GameObject highlight;

    private void Start() {
        transform.localPosition = itemPos;
        GetComponent<SpriteRenderer>().color = itemColor;
    }

    private void OnMouseDown() {
        FindFirstObjectByType<MatchController>().SelectItem(this);
    }

    public void MoveItem(Vector2 pos) {
        itemPos = pos;
        transform.localPosition = pos;
    }
}
