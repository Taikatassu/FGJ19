using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FPSDisplay : MonoBehaviour {

    private const float updateInterval = 0.3f;
    private const string prefixText = "FPS: ";

    private TMP_Text text;
    private float lastUpdateTime;
    private List<float> frameTimes;

    void Start() {
        text = GetComponent<TMP_Text>();
        lastUpdateTime = Time.time;
        frameTimes = new List<float>();
        text.text = prefixText + (1 / Time.deltaTime).ToString("F0");
    }

    void Update() {
        frameTimes.Add(Time.deltaTime);

        if(Time.time - lastUpdateTime >= updateInterval) {
            lastUpdateTime = Time.time;
            var totalFrameTime = 0f;
            foreach(var ft in frameTimes) {
                totalFrameTime += ft;
            }

            var intervalAverageFPS = 1 / (totalFrameTime / frameTimes.Count);
            text.text = prefixText + intervalAverageFPS.ToString("F0");
            frameTimes = new List<float>();
        }
    }
}