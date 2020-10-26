using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerSate
{
    WALK,
    ATTACK,
    ITERACT,
    IDLE
}

public class PlayerMovement : MonoBehaviour
{
    // public readonly Vector3 PLAYER_POSITION_OFFSET = new Vector3(0, 0.7f, 0);

    // Start is called before the first frame update
    void Start()
    {
        playerRigid2D_ = GetComponent<Rigidbody2D>();
        animator_ = GetComponent<Animator>();
        pathQueue_ = new Queue<Vector3>();
        debugLine_ = GetComponent<LineRenderer>();
        playerSate_ = PlayerSate.IDLE;
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
                int index = 0;
                debugLine_.positionCount = path.Count;
                foreach (Vector3 point in path)
                {
                    pathQueue_.Enqueue(point);
                    debugLine_.SetPosition(index, new Vector3(point.x, point.y, -5.0f));
                    index++;
                }
            }
            playerSate_ = PlayerSate.WALK;
        }
        else if (Input.GetButtonDown("Attack") &&
            playerSate_ != PlayerSate.ATTACK)
        {
            playerSate_ = PlayerSate.ATTACK;
            StartCoroutine(ShowAttack());
        }
        else if (Input.GetAxisRaw("Horizontal") != 0.0
            || Input.GetAxisRaw("Vertical") != 0.0)
        {
            // Use keyboard to control.
            pathQueue_.Clear();
            change_.x = Input.GetAxisRaw("Horizontal");
            change_.y = Input.GetAxisRaw("Vertical");
            playerSate_ = PlayerSate.WALK;
        }
        else if (pathQueue_.Count > 0)
        {
            // Follow the path reading from the queue.
            Vector3 firstPoint = pathQueue_.Peek();
            if ((transform.position - firstPoint).magnitude < 0.5)
            {
                pathQueue_.Dequeue();
            }
            else
            {
                change_ = Vector3.Normalize(firstPoint - transform.position);
            }
            playerSate_ = PlayerSate.WALK;
        }
        MoveCharacter();
        AnimateCharacter();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        change_ = Vector3.zero;
        pathQueue_.Clear();
        MoveCharacter();
        AnimateCharacter();
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
        switch (playerSate_)
        {
            case PlayerSate.WALK:
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
                break;
            case PlayerSate.ATTACK:
                break;
            default:
                // Clean animator states.
                animator_.SetBool("isAttacking", false);
                animator_.SetBool("isMoving", false);
                break;
        }
    }

    private IEnumerator ShowAttack() {
        animator_.SetBool("isAttacking", true);
        yield return new WaitForSeconds(0.3f);
        animator_.SetBool("isAttacking", false);
        playerSate_ = PlayerSate.IDLE;
    }

    public float speed_;
    private Rigidbody2D playerRigid2D_;
    private Vector3 change_;
    private Vector3 target_;
    private Animator animator_;
    private Queue<Vector3> pathQueue_;
    private LineRenderer debugLine_;
    private PlayerSate playerSate_;
}
