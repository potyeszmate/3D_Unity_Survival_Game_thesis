using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiSleep : MonoBehaviour
{
    public GameObject sleepPanel;
    public Button[] buttons;

    private void Start()
    {
        Hide();
    }
    // Activates the sleep panel
    internal void Show()
    {
        sleepPanel.SetActive(true);
    }
    // Inactivates the sleep panel
    internal void Hide()
    {
        sleepPanel.SetActive(false);
    }

    // Sets the buttons in the panel to interactive or not interactive
    internal void ToggleAllButtons()
    {
        foreach (var button in buttons)
        {
            button.interactable = !button.interactable;
        }
    }
}
