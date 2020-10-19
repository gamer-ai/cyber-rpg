using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        playerRigid2D_ = GetComponent<Rigidbody2D>();
        animator_ = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            target_ = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Debug.LogFormat("{0}, {1}", target_.x, target_.y);
            List<Vector2Int> path = PathManager.Instance.FindCurrentPath(
                transform.position, target_);
            if (path != null)
            {
                foreach (Vector2Int point in path)
                {
                    Debug.Log(point);
                }
            }
        }
        else
        {
            change_ = Vector3.zero;
            change_.x = Input.GetAxisRaw("Horizontal");
            change_.y = Input.GetAxisRaw("Vertical");
            MoveCharacter();
            AnimateCharacter();
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
}
