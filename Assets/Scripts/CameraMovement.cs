using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform target;
    public float smoothing;
    public Vector2 maxPosition;
    public Vector2 minPosition;

    // Start is called before the first frame update
    void Start()
    {

    }

    void LateUpdate()
    {
        if (transform.position != target.position)
        {
            Vector3 nextPosition = new Vector3(target.position.x,
                target.position.y, transform.position.z);
            nextPosition.x = Mathf.Clamp(nextPosition.x, minPosition.x,
                maxPosition.x);
            nextPosition.y = Mathf.Clamp(nextPosition.y, minPosition.y,
                maxPosition.y);
            transform.position = Vector3.Lerp(transform.position, nextPosition,
                smoothing);
        }
    }
}