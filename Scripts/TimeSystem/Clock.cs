using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;


public class Clock : MonoBehaviour
{

    public Action OnMinuteChanged;
    public Action OnHourChanged; 
    
    public int Hour = 14;
    public int Minute = 00;

    public float Day = 1;

    private float minuteToRealTime = 0.5f;  // 0.5f
    private float timer;

    public int MinuteNormal;
    public int HourNormal;
    public float DayNormal;

    public GameObject DayAudio;
    public GameObject NightAudio;



    // Starting time is 14 O' clock
    void Start() 
    {
        MinuteNormal = Minute;
        HourNormal = Hour;
        DayNormal = Day;
        timer = minuteToRealTime;
    }
    
    // A simple Clock algorithm
    void Update() 
    {
        timer -= Time.deltaTime;

        // First it increases the minue
        if( timer <= 0)
        {
            Minute ++;
            OnMinuteChanged?.Invoke();
            // If more then 60 minute then increase the hour
            if(Minute >= 60)
            {
                Hour++;
                Minute = 0;
                OnHourChanged?.Invoke();

                //if more than 24 hour then dont increase anything, start it againt with hour = 0
                if(Hour >= 24)
                {
                Hour = 0;
                }
            }

            timer = minuteToRealTime;
        }

        // Always check if we need to increase the Survived Day number
        TimeCheck();

        if(Hour >= 6 && Hour <= 19)
        {
            DayAudio.SetActive(true);
            NightAudio.SetActive(false);
        }
        else if((Hour <= 5 && Minute >= 1) || (Hour >= 20 && Minute >= 1))
        {
            DayAudio.SetActive(false); 
            NightAudio.SetActive(true);
        }


    }

    public void TimeCheck()
    {   
        // If its 13 O' clock we increase the survived day with 1
        if(Hour == 13 && Minute == 0)
        {   
            Day += 0.03448275862f;
        }
    }

    // Sleep hour method adds the given amount of hour to the clock, that we want to sleep
    public void SleepAddHour(int hour)
    {
        int addedHour = Hour+=hour;

        if( addedHour >= 24)
        {
            Hour = addedHour - 24;
        }
        else
        {
            Hour+=hour;
        }
        
    }

}
