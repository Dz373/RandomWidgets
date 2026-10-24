using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;

public class SnakeController : MonoBehaviour
{
    [Header("Snake")]
    public int addLength = 3;
    public float interval = 0.25f;
    public Vector3 dir = Vector3.up;

    [Header("Map Bounds")]
    public int xMin;
    public int xMax;
    public int yMin;
    public int yMax;

    [Header("Object References")]
    public SnakeSegment head;
    public SnakeSegment tail;
    [SerializeField] GameObject snakeSegment;

    private int totalLength = 1;
    private float curTime;
    private bool moveKey = false;

    private void Update() {
        curTime += Time.deltaTime;

        if (!moveKey) {
            if (Input.GetKeyDown(KeyCode.S) && dir != Vector3.up) {
                dir = Vector3.down;
                moveKey = true;
            }
            if (Input.GetKeyDown(KeyCode.W) && dir != Vector3.down) {
                dir = Vector3.up;
                moveKey = true;
            }
            if (Input.GetKeyDown(KeyCode.A) && dir != Vector3.right) {
                dir = Vector3.left;
                moveKey = true;
            }
            if (Input.GetKeyDown(KeyCode.D) && dir != Vector3.left) {
                dir = Vector3.right;
                moveKey = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            dir = Vector3.zero;
        }

        if (curTime >= interval && dir != Vector3.zero) {
            curTime = 0;

            Vector3 tailPos = tail.transform.position;

            head.Move(head.transform.position + dir);
            moveKey = false;

            if (OutOfBounds(head.transform.position))
                RestartGame();

            if (addLength > 0) {
                GameObject newSeg = Instantiate(snakeSegment);
                newSeg.transform.position = tailPos;
                tail.next = newSeg.GetComponent<SnakeSegment>();
                tail = tail.next;

                addLength--;

                if (++totalLength % 2 == 0)
                    newSeg.GetComponent<SpriteRenderer>().color = Color.black;
                else
                    newSeg.GetComponent<SpriteRenderer>().color = Color.red;
                tail.segmentNum = totalLength;
            }
        }
    }

    public bool SnakeOccupied(Vector3 pos) {
        SnakeSegment cur = head;
        while (cur != null && cur.transform.position != pos)
            cur = cur.next;

        if (cur == null)
            return false;
        if (cur.transform.position == pos)
            return true;

        return false;
    }

    private bool OutOfBounds(Vector3 pos) {
        if (pos.x < xMin || pos.x > xMax)
            return true;
        if (pos.y < yMin || pos.y > yMax)
            return true;
        return false;
    }

    public void RestartGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
