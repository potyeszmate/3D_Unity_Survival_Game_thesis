using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryState : BaseState
{

    // This state is responsible for the Inventory system's (and crafting system's) state
    public override void EnterState(AgentController controller)
    {
        base.EnterState(controller);
        // Toggles the inventory panel (button i)
        controllerReference.inventorySystem.ToggleInventory();
        // toggles the crafting panel (button i)
        controllerReference.craftingSystem.ToggleCraftingUI();
        // Test
        Cursor.lockState = CursorLockMode.Confined;
        // Showes the cursor
        Cursor.visible = true;
    }

    // Handles the input ( button i) and toggles the panels
    public override void HandleInventoryInput()
    {
        base.HandleInventoryInput();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        controllerReference.inventorySystem.ToggleInventory();
        controllerReference.craftingSystem.ToggleCraftingUI();
        //Test
        controllerReference.TransitionToState(controllerReference.movementState);
        
    }

    // We can also quit from inventory state by pressing the esc (and the button 'i' as well)
    public override void HandleEscapeInput()
    {
        HandleInventoryInput();
    }

}
