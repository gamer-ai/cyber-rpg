using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PotMovement : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        animator_ = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Smash()
    {
        animator_.SetBool("isSmash", true);
        StartCoroutine(ExitWorld());
    }

    private IEnumerator ExitWorld()
    {
        yield return new WaitForSeconds(0.3f);
        gameObject.SetActive(false);
    }

    private Animator animator_;
}
