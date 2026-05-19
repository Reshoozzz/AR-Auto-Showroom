using UnityEngine;
using TMPro;

public class FPSCounter : MonoBehaviour
{
    [Header("UI Reference")]
    public TMP_Text fpsText;

    [Header("Settings")]
    public float updateInterval = 0.5f;

    private float accumulatedTime = 0f;
    private int frameCount = 0;
    private float timer = 0f;

    void Update()
    {
        accumulatedTime += Time.unscaledDeltaTime;
        frameCount++;
        timer += Time.unscaledDeltaTime;

        if (timer >= updateInterval)
        {
            float fps = frameCount / accumulatedTime;
            
            if (fpsText != null)
            {
                fpsText.text = $"FPS: {Mathf.RoundToInt(fps)}";
                
                // Color code by performance
                if (fps >= 50)
                    fpsText.color = Color.green;
                else if (fps >= 30)
                    fpsText.color = Color.yellow;
                else
                    fpsText.color = Color.red;
            }

            accumulatedTime = 0f;
            frameCount = 0;
            timer = 0f;
        }
    }
}