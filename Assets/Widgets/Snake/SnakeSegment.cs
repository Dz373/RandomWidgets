using UnityEngine;

public class SnakeSegment : MonoBehaviour {
    public SnakeSegment next = null;
    public int segmentNum = 0;
    public Vector2 target;

    SnakeController gm;

    private void Start() {
        gm = FindFirstObjectByType<SnakeController>();
        target = transform.position;
    }

    private void Update() {
        float step = Time.deltaTime / gm.interval;
        transform.position = Vector2.MoveTowards(transform.position, target, step);
    }

    public void Move(Vector2 tar) {
        target = tar;

        if (next != null) {
            next.Move(transform.position);
        }
    }
}
