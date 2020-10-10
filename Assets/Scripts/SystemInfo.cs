using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemInfo : MonoBehaviour
{
    // Start is called before the first frame update
    void OnGUI()
    {
        int w = Screen.width, h = Screen.height;

        GUIStyle style = new GUIStyle();

        Rect rect = new Rect(0, 0, w, h * 2 / 100);
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = h * 2 / 100;
        style.normal.textColor = new Color(0.0f, 0.0f, 0.5f, 1.0f);
        float msec = deltaTime_ * 1000.0f;
        float fps = 1.0f / deltaTime_;
        string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
        GUI.Label(rect, text, style);

    }

    // Update is called once per frame
    void Update()
    {
        deltaTime_ += (Time.unscaledDeltaTime - deltaTime_) * 0.1f;

    }

    private float deltaTime_ = 0.0f; 
}
