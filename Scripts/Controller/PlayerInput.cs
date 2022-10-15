using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public Vector2 MovementInputVector { get; private set; }
    public Vector3 MovementDirectionVector { get; private set; }

    public Action OnJump { get; set; }

    public Action OnToggleInventory { get; set; }

    public Action<int> OnHotbarKey { get; set; }

    public Action OnPrimaryAction { get; set; }

    public Action OnSecondaryAction { get; set; }

    private Camera mainCamera;

    private float previousPrimaryActionInput = 0, prevousSecondaryActionInput = 0;

    public Action OnEscapeKey { get; set; }

    public bool menuState = false;

    private void Start()
    {
        mainCamera = Camera.main;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // This checks all the inputs in the game (not the moving inputs, beacasue that is in the movement script)
    private void Update()
    {
        CheckEscapeButton();
        if(menuState == false)
        {
            GetInventoryInput();
            GetHotbarInput();
            GetPrimaryAction();
            GetSecondaryAction();
        }


    }

    // Checks the escape button if it was pressed down or not
    private void CheckEscapeButton()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnEscapeKey?.Invoke();
            
        }
    }


    // Right click listener. Invoke if rightclick pressed
    private void GetSecondaryAction()
    {
        //var inputValue = Input.GetAxisRaw("Fire2");

        //if (prevousSecondaryActionInput == 0)
        //{
            /*
            if (inputValue >= 1)
            {
                OnSecondaryAction?.Invoke();
            }
            */
            if (Input.GetKeyDown(KeyCode.E))
            {
                OnSecondaryAction?.Invoke();
            }


        //}
        //prevousSecondaryActionInput = inputValue;
    }

    // Leftclikc listener. Invoke when leftclick pressed
    private void GetPrimaryAction()
    {
        var inputValue = Input.GetAxis("Fire1");
        if (previousPrimaryActionInput == 0)
        {
            if (inputValue >= 1)
            {
                OnPrimaryAction?.Invoke();
            }
        }
        previousPrimaryActionInput = inputValue;
    }

    // Checks if the hotbar numbers were oressed down (1-10 but we can inactive hotbar places in the editor if we want less)
    private void GetHotbarInput()
    {
        char hotbar0 = '0';
        // hozbar numbers
        for (int i = 0; i < 10; i++)
        {
            // hotbar0 + 1, because we dont use 0 number
            KeyCode keyCode = (KeyCode)((int)hotbar0 + i);
            if (Input.GetKeyDown(keyCode))
            {
                OnHotbarKey?.Invoke(i);
                return;
            }
        }
    }

    // Checks the inventory input (if i pressed)
    private void GetInventoryInput()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {   
            //Debug.Log("GetInventoryInput -> meghívva");
            OnToggleInventory?.Invoke();
        }
    }

    // Checks the jump imput (space). I use jump only in movement so we donnt use this method anywhere right now.
    private void GetJumpInput()
    {
        if (Input.GetAxisRaw("Jump") > 0)
        {
            OnJump?.Invoke();
        }
    }

}
