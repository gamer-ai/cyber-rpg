using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sign : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isPlayerInRange_)
        {
            if (dialogBox.activeInHierarchy)
            {
                if (dialogText.text != nextDialogStr)
                {
                    dialogText.text = nextDialogStr;
                }
                else
                {
                    dialogBox.SetActive(false);
                }
            }
            else
            {
                dialogBox.SetActive(true);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange_ = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange_ = false;
            dialogBox.SetActive(false);
        }
    }

    public GameObject dialogBox;
    public Text dialogText;
    public string nextDialogStr;
    private bool isPlayerInRange_;
}
