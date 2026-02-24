using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPSCount : MonoBehaviour
{
    public Text fpsText; // Reference to a UI Text component

    private int frameCount = 0;
    private float dt = 0.0f;
    private float fps = 0.0f;
    private float updateRate = 4.0f;  // 4 updates per sec.

    void Start()
    {
        if (fpsText == null)
        {
            //Debug.LogError("Please assign a UI Text component to DisplayFPS script.");
        }
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        frameCount++;
        dt += Time.deltaTime;
        if (dt > 1.0f / updateRate)
        {
            fps = frameCount / dt;
            frameCount = 0;
            dt -= 1.0f / updateRate;
            if (fpsText != null)
            {
                fpsText.text = "FPS: " + Mathf.Ceil(fps).ToString();
            }
        }
    }
}
