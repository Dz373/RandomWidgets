using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class SnakeController : MonoBehaviour
{
    public int addLength = 3;
    public float interval = 0.25f;
    public Vector3 dir = Vector3.up;

    public SnakeSegment head;
    public SnakeSegment tail;

    private float curTime;
    private bool moveKey = false;

    [SerializeField] GameObject snakeSegment;
    [SerializeField] GameObject snakePellet;

    private void Update() {
        curTime += Time.deltaTime;

        if (!moveKey) {
            if (Input.GetKeyDown(KeyCode.DownArrow) && dir != Vector3.up) {
                dir = Vector3.down;
                moveKey = true;
            }
            if (Input.GetKeyDown(KeyCode.UpArrow) && dir != Vector3.down) {
                dir = Vector3.up;
                moveKey = true;
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow) && dir != Vector3.right) {
                dir = Vector3.left;
                moveKey = true;
            }
            if (Input.GetKeyDown(KeyCode.RightArrow) && dir != Vector3.left) {
                dir = Vector3.right;
                moveKey = true;
            }
        }

        if (curTime >= interval) {
            curTime = 0;

            Vector3 tailPos = tail.transform.position;

            head.Move(head.transform.position + dir);
            moveKey = false;

            if (addLength > 0) {
                GameObject newSeg = Instantiate(snakeSegment);
                newSeg.transform.position = tailPos;
                tail.next = newSeg.GetComponent<SnakeSegment>();
                tail = tail.next;

                addLength--;
            }
        }
    }
}
