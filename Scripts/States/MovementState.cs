using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementState : BaseState
{
    // Responsible for the movement state (when we are moving)

    // Enters movement state
    public override void EnterState(AgentController controller)
    {
        base.EnterState(controller);
        
    }

    public DetectionSystem detectionSystem;

    // During movement we can handle the inventory input
    public override void HandleInventoryInput()
    {
        base.HandleInventoryInput();
        controllerReference.TransitionToState(controllerReference.inventoryState);
    }

    // During movement we can handle the secondary input (right click)
    public override void HandleSecondaryAction()
    {
        base.HandlePrimaryAction();
        controllerReference.TransitionToState(controllerReference.interactState);
    }

    // During movement we can handle the primary input (left click)
    public override void HandlePrimaryAction()
    {
        base.HandlePrimaryAction(); 

    }

    // During movement we can handle the hotbar inputs (1-8)
    public override void HandleHotbarInput(int hotbarKey)
    {
        base.HandleHotbarInput(hotbarKey);
        controllerReference.inventorySystem.HotbarShortKeyHandler(hotbarKey);
    }

    public override void Update()
    {
        base.Update();
        controllerReference.detectionSystem.PerformDetection(controllerReference.input.MovementDirectionVector);
        
    }


}
