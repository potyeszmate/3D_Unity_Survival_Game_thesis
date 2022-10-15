using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StructuresInteractionManager : MonoBehaviour
{
    public SleepManager sleepManager;

    // Shows the Sleep UI Panel
    public void ShowSLeepUI()
    {
        sleepManager.ShowUI();
    }


}
