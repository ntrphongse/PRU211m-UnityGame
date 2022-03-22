using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TImer : MonoBehaviour
{
    public enum TimerState { NotStarted, Started }
    TimerState state = TimerState.NotStarted;
    public float timeValue { get; private set; }
    public float GetTime()
    {
        return timeValue;
    }
    public Text timerText;
    public void StartTimer()
    {
        timeValue = 15;
        state = TimerState.Started;
    }

    public void StopTimer()
    {
        state = TimerState.NotStarted;
        timerText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        if (state == TimerState.Started)
        {
            if (timeValue > 0)
            {
                timeValue -= Time.deltaTime;
            }
            else
            {
                timeValue = 0;
            }
            DisplayTime(timeValue);
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        if (timeToDisplay < 0)
        {
            timeToDisplay = 0;
        }

        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        float mil = timeToDisplay % 1 * 1000;

        timerText.text = string.Format("{0:00}:{1:000}", seconds, mil);
    }
}
