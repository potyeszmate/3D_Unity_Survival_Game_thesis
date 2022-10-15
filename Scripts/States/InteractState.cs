using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractState : BaseState
{   
   // public GameObject cookingPanel;


    // This methode checks if we are in a structure placing state or in a item picking state
    public override void EnterState(AgentController controller)
    {
        base.EnterState(controller);
        
        // enters structure picking state if the used item is a structute
        var usableStructure = controllerReference.detectionSystem.IUsableCollider;
        if(usableStructure != null)
        {
            usableStructure.GetComponent<IUsable>().Use();
            
            return;
        }
        
        // If we have the correct collider the it enters to the picking up state 
        var resultCollider = controllerReference.detectionSystem.CurrentCollider;
        if (resultCollider != null)
        {
            var ipickable = resultCollider.GetComponent<IPickable>();
            var remainder = controllerReference.inventorySystem.AddToStorage(ipickable.PickUp());
            ipickable.SetCount(remainder);
            if (remainder > 0)
            {
                Debug.Log("Cant pick up");
            }
        }
    }

    public override void Update()
    {
        base.Update();
        controllerReference.TransitionToState(controllerReference.movementState);
    }
}
