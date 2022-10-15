using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStorageButtonsHelper : MonoBehaviour
{
    public Action OnUseBtnClick, OnDropBtnClick;
    public Button useBtn, dropBtn;
    // Start is called before the first frame update
    void Start()
    {
        useBtn.onClick.AddListener(() => OnUseBtnClick?.Invoke());
        dropBtn.onClick.AddListener(() => OnDropBtnClick?.Invoke());
    }

    // Hide the buttons in the inventory
    public void HideAllButons()
    {
        ToggleDropButton(false);
        ToggleUseButton(false);
    }

    // Toggle (on and off) the Use button
    public void ToggleUseButton(bool val)
    {
        useBtn.interactable = val;
    }

    // Toggle (on and off) the Drop button
    public void ToggleDropButton(bool val)
    {
        dropBtn.interactable = val;
    }
}
