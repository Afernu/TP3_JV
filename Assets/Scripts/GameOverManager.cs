using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    public TextMeshProUGUI timeText; 

    private void Awake()
    {
        // Recupere le playerpref
        float bestTime = PlayerPrefs.GetFloat("BestTime", Mathf.Infinity);

        if (timeText != null)
        {

            int hours = Mathf.FloorToInt(bestTime / 3600);
            int minutes = Mathf.FloorToInt((bestTime % 3600) / 60);
            int seconds = Mathf.FloorToInt(bestTime % 60);

            timeText.text = "Meilleur score: " + string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
        }
    }
}