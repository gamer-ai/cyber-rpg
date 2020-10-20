using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public readonly Vector3 PLAYER_POSITION_OFFSET = new Vector3(0, 0.7f, 0);

    // Start is called before the first frame update
    void Start()
    {
        playerRigid2D_ = GetComponent<Rigidbody2D>();
        animator_ = GetComponent<Animator>();
        pathQueue_ = new Queue<Vector3>();
    }

    void FixedUpdate()
    {
        change_ = Vector3.zero;
        if (Input.GetMouseButtonDown(0))
        {
            // Use mouse to control.
            target_ = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            List<Vector3> path = PathManager.Instance.FindCurrentPath(
                transform.position, target_);
            if (path != null)
            {
                pathQueue_.Clear();
                foreach (Vector3 point in path)
                {
                    pathQueue_.Enqueue(point);
                }
            }
        }
        else if (Input.GetAxisRaw("Horizontal") != 0.0
            || Input.GetAxisRaw("Vertical") != 0.0)
        {
            // Use keyboard to control.
            pathQueue_.Clear();
            change_.x = Input.GetAxisRaw("Horizontal");
            change_.y = Input.GetAxisRaw("Vertical");
        }
        else if (pathQueue_.Count > 0)
        {
            // Follow the path reading from the queue.
            Vector3 firstPoint = pathQueue_.Peek() + PLAYER_POSITION_OFFSET;
            if ((transform.position - firstPoint).magnitude < 0.5)
            {
                Debug.Log("reached");
                pathQueue_.Dequeue();
            }
            else
            {
                Debug.LogFormat("moving to: {0}", firstPoint);
                change_ = Vector3.Normalize(firstPoint - transform.position);
            }
        }
        MoveCharacter();
        AnimateCharacter();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("RoomTransfer"))
        {
            // Move to next room then clean previous status.
            change_ = Vector3.zero;
            pathQueue_.Clear();
        }
    }

    void MoveCharacter()
    {
        if (change_ != Vector3.zero)
        {
            Vector3 velocity = Vector3.Normalize(change_) * speed_;
            Vector3 to = transform.position + velocity * Time.deltaTime;
            playerRigid2D_.MovePosition(to);
        }
    }

    void AnimateCharacter()
    {
        if (change_ != Vector3.zero)
        {
            animator_.SetBool("isMoving", true);
            animator_.SetFloat("moveX", change_.x);
            animator_.SetFloat("moveY", change_.y);
        }
        else
        {
            animator_.SetBool("isMoving", false);
        }
    }

    public float speed_;
    private Rigidbody2D playerRigid2D_;
    private Vector3 change_;
    private Vector3 target_;
    private Animator animator_;
    private Queue<Vector3> pathQueue_;
}
