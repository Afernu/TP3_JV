using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class IncreasingTime : MonoBehaviour
{
    private float elapsedTime = 0f;
    private TextMeshProUGUI Timer;
    private TextMeshProUGUI bestTimeText;

    private const string BestTimeKey = "BestTime";

    private void Awake()
    {
        Timer = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;

        DisplayTime(Timer, elapsedTime);
    }
    public void GameOver()
    {
        float currentTime = elapsedTime;
        float bestTime = PlayerPrefs.GetFloat(BestTimeKey, Mathf.Infinity);

        if (currentTime > bestTime)
        {
            PlayerPrefs.SetFloat(BestTimeKey, currentTime);
            PlayerPrefs.Save();
        }
    }
    private void DisplayTime(TextMeshProUGUI textMesh, float timeInSeconds)
    {
        if (textMesh != null)
        {
            int hours = Mathf.FloorToInt(timeInSeconds / 3600);
            int minutes = Mathf.FloorToInt((timeInSeconds % 3600) / 60);
            int seconds = Mathf.FloorToInt(timeInSeconds % 60);
            textMesh.text = "Time: " + string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
        }
    }
}
