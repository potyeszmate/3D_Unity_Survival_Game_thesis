using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeUI : MonoBehaviour
{
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI DayText;
    public Clock clock;

    // Add time to Clock
    private void OnEnable()
    {
        clock.OnMinuteChanged += UpdateTime;
        clock.OnHourChanged += UpdateTime;

    }

    // Take away time from the clock 
    private void Disable()
    {
        clock.OnMinuteChanged -= UpdateTime;
        clock.OnHourChanged -= UpdateTime;

    }

    // Updates the Hour and The Minute.
    public void UpdateTime()
    {
        timeText.text = $"{clock.Hour:00}:{clock.Minute:00}";
        DayText.text = $"{clock.Day:0}";

    }


}
