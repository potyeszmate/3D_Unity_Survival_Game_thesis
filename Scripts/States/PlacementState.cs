using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementState : MovementState
{
    //Responsible for the placement state (when we are placing a structure)
    PlacementHelper placementHelper;
    GameObject structureToPlace;
    public override void EnterState(AgentController controller)
    {
        base.EnterState(controller);
        CreateStructureToPlace();
    }

    // Creates the structures object in the map in front of us
    private void CreateStructureToPlace()
    {
        placementHelper = ItemSpawnManager.instance.CreateStructure(controllerReference.inventorySystem.selectedStructureData);
        placementHelper.PrepareForMovement();

    }

    // We can quit from the placing by pressing the wsc button (destroyes the object in front of us)
    public override void HandleEscapeInput()
    {
        
        if (placementHelper.isActiveAndEnabled)
        {
            DestroyPlacedObject();
        }
        
        controllerReference.TransitionToState(controllerReference.movementState);
    }

    // Handles jump during the placement state
    public override void HandleJumpInput()
    {
    }

    // Handles inventory input during the placement state
    public override void HandleInventoryInput()
    {
    }

    // Handles secondary input during the placement state
    public override void HandleSecondaryAction()
    {

    }

    // Handles primary input (left click) during the placement state, places the object when right position
    public override void HandlePrimaryAction()
    {   
        // If the ovject is in a correct location then we can place it tio the ground
        if (placementHelper.CorrectLocation)
        {
            var structureComponent = placementHelper.PrepareForPlacement();
            structureComponent.SetData(controllerReference.inventorySystem.selectedStructureData);
            placementHelper.enabled = false;
            controllerReference.inventorySystem.RemoveSelectedStructureFromInventory();
            controllerReference.buildingPlacementStroage.SaveStructureReference(structureComponent);
            HandleEscapeInput();
        }
    }

    // HAndles the hotbar numbers
    public override void HandleHotbarInput(int hotbarKey)
    {
    }

    public override void Update()
    {
        
    }

    // Destroyes the placable object
    private void DestroyPlacedObject()
    {
        Debug.Log("Destroying placed object");
        placementHelper.DestroyStructure();
    }
}
