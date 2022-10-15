using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookingState : BaseState
{

    // This state is responsible for the Inventory system's (and crafting system's) state
    public override void EnterState(AgentController controller)
    {
        base.EnterState(controller);
        
        // Test
        controllerReference.cookingSystem.ToggleCraftingUI();
        Cursor.lockState = CursorLockMode.Confined;
        // Shows the cursor
        Cursor.visible = true;
    }

    // Handles the input ( button i) and toggles the panels
    public override void HandleInventoryInput()
    {
        base.HandleInventoryInput();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        controllerReference.TransitionToState(controllerReference.movementState);
        
    }

    // We can also quit from inventory state by pressing the esc (and the button 'i' as well)
    public override void HandleEscapeInput()
    {
        HandleInventoryInput();
    }

}
