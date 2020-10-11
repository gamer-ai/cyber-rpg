using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        cameraMovement_ = Camera.main.GetComponent<CameraMovement>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            cameraMovement_.maxPosition += cameraChange;
            cameraMovement_.minPosition += cameraChange;
            collider.transform.position += playerChange;
            if (needText)
            {
                StartCoroutine(ShowText());
            }
        }
    }

    private IEnumerator ShowText()
    {
        textObject.SetActive(true);
        placeText.text = placeName;
        yield return new WaitForSeconds(4.0f);
        textObject.SetActive(false);
    }

    public Vector2 cameraChange;
    public Vector3 playerChange;
    public bool needText;
    public GameObject textObject;
    public string placeName;
    public Text placeText;
    private CameraMovement cameraMovement_;
}
