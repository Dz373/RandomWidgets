using UnityEngine;

public class SnakeDetector : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.TryGetComponent<SnakeSegment>(out SnakeSegment comp))
            if(comp.segmentNum != 2)
                FindFirstObjectByType<SnakeController>().RestartGame();
    }
}
