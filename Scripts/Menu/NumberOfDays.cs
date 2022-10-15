using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NumberOfDays : MonoBehaviour
{
    public Clock clock;

    public int daysSurvived;

    public GameObject Day;
    public GameObject Days;
    public TextMeshProUGUI daySurvivedText;

    // Start is called before the first frame update
  

    // Update is called once per frame
    void Update()
    {
        clock.Day = daysSurvived;

        daySurvivedText.text = Mathf.FloorToInt(daysSurvived).ToString();

        if(daysSurvived == 1)
        {
            Day.SetActive(true);
            Days.SetActive(false);
        }
            
        else
        {
            Day.SetActive(false);
            Days.SetActive(true);
        }
         
    }
}
