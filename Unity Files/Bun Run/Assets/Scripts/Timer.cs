using UnityEngine;
using System.Diagnostics;
using System;
using TMPro;

public class ProgressionTimer : MonoBehaviour
{
    public float currentTime;
    bool hasInputted = false;
    public bool isOver = false;
    public bool started;
    public TMP_Text timerDisplay;
    public TMP_Text bestScoreDisplay;
    float thisScore;
    
    // Update is called once per frame
    void Update()
    {
        if (!hasInputted)
        {
            FirstPress();
        }
        else
        {
            if (!isOver)
            {
                if(!started)
                {
                    currentTime = 0f;
                    started = true;
                }
                else
                {
                    currentTime = currentTime + Time.deltaTime;
                }

                TimeSpan timer = TimeSpan.FromSeconds(currentTime);
                timerDisplay.text = "Time: " 
                            + timer.Minutes.ToString("00") + ":" 
                            + timer.Seconds.ToString("00") + ":" 
                            + (timer.Milliseconds / 10).ToString("00");
            }
        }
    }
    /// <summary>
    /// Starts the timer when player presses any button
    /// </summary>
    void FirstPress()
    {
        if (Input.anyKey)
        {
            print("You've pressed something");
            hasInputted = true;
            ShowBestScore();
        }
    }

    void ShowBestScore()
    {
        TimeSpan bestTime = TimeSpan.FromSeconds(GetBestScore());
        if (bestTime == TimeSpan.Zero) bestTime = TimeSpan.FromSeconds(0);
        bestScoreDisplay.text = "Best: " 
                                + bestTime.Minutes.ToString("00") + ":" 
                                + bestTime.Seconds.ToString("00") + ":" 
                                + (bestTime.Milliseconds / 10).ToString("00");
    }

    public void SetBestScore()
    {
        thisScore = currentTime;
        float best = PlayerPrefs.GetFloat("BestScore");
        if (isOver && best > thisScore || isOver && best == 0)
        {
            PlayerPrefs.SetFloat("BestScore", thisScore);
            best = thisScore;
        }
    }

    public static float GetBestScore()
    {
        return PlayerPrefs.GetFloat("BestScore");
    }
}
