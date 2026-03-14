using UnityEngine;
using System.Diagnostics;
using System;
using TMPro;

public class ProgressionTimer : MonoBehaviour
{
    public float currentTime;
    public bool hasInputted = false;
    public bool isOver = false;
    public bool started;
    public TMP_Text timerDisplay;

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
                timerDisplay.text = "Time: " + timer.Minutes.ToString() + ":" + timer.Seconds.ToString() + ":" + timer.Milliseconds.ToString();
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
        }
    }
}
