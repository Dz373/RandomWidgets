using UnityEngine;
using System.Collections.Generic;

public class SnakeGrow : MonoBehaviour
{
    SnakeController gm;
    List<Vector2> randomPositions = new List<Vector2>();
    Queue<Vector2> removedPositions = new Queue<Vector2>();

    private void Start() {
        gm = FindFirstObjectByType<SnakeController>();
        
        for(int x = gm.xMin; x <= gm.xMax; x++) {
            for(int y = gm.yMin; y <= gm.yMax; y++) {
                randomPositions.Add(new Vector2(x, y));
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D collision) {
        gm.addLength++;

        int newIndex = Random.Range(0, randomPositions.Count);
        while (gm.SnakeOccupied(randomPositions[newIndex])) {
            removedPositions.Enqueue(randomPositions[newIndex]);
            randomPositions.Remove(randomPositions[newIndex]);

            newIndex = Random.Range(0, randomPositions.Count);
        }

        Vector2 newPos = randomPositions[newIndex];

        while(removedPositions.Count > 0) {
            randomPositions.Add(removedPositions.Dequeue());
        }

        transform.position = newPos;
    }
}
