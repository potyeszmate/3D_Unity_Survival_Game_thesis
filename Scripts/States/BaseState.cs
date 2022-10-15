using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseState
{
    // The Base state of the game, when we are not pausing the game (so when the game time is not paused)

    protected AgentController controllerReference;

    // Enters the state
    public virtual void EnterState(AgentController controller)
    {
        this.controllerReference = controller;
    }

    // Handles the Jump input (space)
    public virtual void HandleJumpInput() { }

    // Handles the inventory input (i)
    public virtual void HandleInventoryInput() { }

    // Handles the hotbar input (1-8)
    public virtual void HandleHotbarInput(int hotbarKey) 
    {
        //Debug.Log(hotbarKey);
    }

    public virtual void Update() {}

    public virtual void HandlePrimaryAction() {}

    public virtual void HandleSecondaryAction(){}

    // Checks if we press wsc button and sets the pause menu state
    public virtual void HandleEscapeInput()
    {
        controllerReference.gameManager.ToggleGameMenu();
        if (controllerReference.input.menuState == false)
        {
            controllerReference.input.menuState = true;
        }
        else
        {
            controllerReference.input.menuState = false;
        }
        
    }

}
