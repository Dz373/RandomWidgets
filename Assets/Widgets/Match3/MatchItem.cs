using UnityEngine;

public class MatchItem : MonoBehaviour
{
    public Color itemColor;
    public Vector3Int itemPos;

    public GameObject highlight;
    private MatchController gm;

    private Vector3Int target;
    private bool finishMoving;

    private void Start() {
        GetComponent<SpriteRenderer>().color = itemColor;
        gm = FindFirstObjectByType<MatchController>();

        transform.localPosition = new Vector3(itemPos.x, itemPos.y + gm.rows);
        target = itemPos;
        finishMoving = false;
    }

    private void Update() {
        if (finishMoving)
            return;

        float step = Time.deltaTime * 5;
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, target, step);

        if(Vector3.Distance(transform.localPosition, target) < 0.001) {
            finishMoving = true;
        }
    }

    private void OnMouseDown() {
        StartCoroutine(gm.SelectItem(this));
    }

    public void MoveItem(Vector3Int pos) {
        itemPos = pos;
        gm.items[itemPos.x, itemPos.y] = this;

        target = pos;
        finishMoving = false;
    }

    public void ChangeItem(Color newColor) {
        itemColor = newColor;
        GetComponent<SpriteRenderer>().color = itemColor;
    }
}
