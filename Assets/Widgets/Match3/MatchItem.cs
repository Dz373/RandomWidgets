using UnityEngine;

public class MatchItem : MonoBehaviour
{
    public Color itemColor;
    public Vector2 itemPos;

    private void Start() {
        transform.localPosition = itemPos;
        GetComponent<SpriteRenderer>().color = itemColor;
    }

    private void OnMouseDown() {
        print(itemPos);
    }
}
