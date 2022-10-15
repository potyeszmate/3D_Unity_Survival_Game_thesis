using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class SleepManager : MonoBehaviour
{
    [Range(0.1f,1)]
    public float timeModifier = 1;
    public UiSleep uiSleep;
    public GameObject sleepPanel;
    public GameObject escPanel;
    public GameObject sunObject;
    public GameObject moonObject;
    public Button button;

    private sun sun;

    public float sunXPos = -0.3862f;     //- 101
    public float sunYPos = 532.2161f;
    public float sunZPos = -718.5735f;      // -818

    public float sunXRot = 44.032f;         // 39
    public float sunYRot = -7f;              // 0
    public float sunZRot = 0f;
    public float sunWRot = 0f;
    

    public Clock clock;
    public PlayerStat playerStat;
    public GameObject WarningText;
    public InputField sleepHour;
    string hoursToSleep;
    public TimeUI timeUI;
    public int thisHour;

    public TextMeshProUGUI timeText;
    public TextMeshProUGUI DayText;

    private void Start()
    {
        uiSleep = GetComponent<UiSleep>();
        WarningText.SetActive(false);
    }
  
    // Showing the sleeping UI panel
    internal void ShowUI()
    {
     
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        uiSleep.Show();
    }

    // Exiting from the sleeping panel
    public void Exit()
    {
        uiSleep.Hide();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Calling the sleep system
    public void Sleep()
    {
        uiSleep.ToggleAllButtons();
        StartCoroutine(SleepCoroutine());
    }

    // Checks if we can sleep or not, if yes we sleep and the time will change
    IEnumerator SleepCoroutine()
    {      

        // if the clock is between 18 and 4 we can sleep.
        if(clock.Hour <= 4.0 || clock.Hour >= 18.0)
        {
            WarningText.SetActive(false);
            //sleepHour.text = " How much do you want to sleep? ";

            yield return new WaitForSecondsRealtime(5);

            clock.Hour = 10;
            clock.Minute = 0;

            playerStat.energy = 10.3f;

            sunObject.transform.position = new Vector3(sunXPos, sunYPos, sunZPos);
            sunObject.transform.rotation = new Quaternion(sunXRot, sunYRot, sunZRot,sunWRot);
            sun.RespawnSun();

            moonObject.transform.position = new Vector3(sunXPos, sunYPos * -1, sunZPos);
            moonObject.transform.rotation = new Quaternion(sunXRot * -1, sunYRot, sunZRot,sunWRot);
            sun.RespawnMoon();

            
            
        }
        else
        {

            WarningText.SetActive(true);
            sleepHour.text = "";
        }
            
        uiSleep.ToggleAllButtons();
    }
    
    private void Update() 
    {
        //Debug.Log(clock.Hour);

        if(sleepPanel.activeSelf == true)
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                Exit();
            }
        }
    }
  

}
