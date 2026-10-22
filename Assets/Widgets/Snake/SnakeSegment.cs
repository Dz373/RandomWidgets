using UnityEngine;

public class SnakeSegment : MonoBehaviour
{
    public SnakeSegment next = null;

    public void Move(Vector3 pos) {
        if (next != null) {
            next.Move(transform.position);
        }

        transform.position = pos;
    }

}
