using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionSystem : MonoBehaviour
{
    public Action<Collider, Vector3> OnAttackSuccessful;
    // All available colliders are stored
    private List<Collider> collidersList = new List<Collider>();
    private Collider currentCollider;
    private List<Material[]> currentColliderMaterialsList = new List<Material[]>();
    // LayerMask which allows you to interact only with the collider of the required objects (items)
    public LayerMask objectDetectionMask;
    
    public Material selectionMaterial;
    // How far away we can interact with the item
    public float detectionRadius;
    
    public Collider CurrentCollider { get => currentCollider; set => currentCollider = value; }

    public Transform WeaponRaycastStartPosition;

    public float attackDistance = 1f;

    private Camera mainCam;

    private Collider iUsableCollider;

    public Collider IUsableCollider
    {
        get { return iUsableCollider; }
        private set { iUsableCollider = value; }
    }

    [ColorUsageAttribute(false,true)]
    public Color usableEmissionColor;

    // Examines the objects in front of our character
    public Collider[] DetectObjectsInFront(Vector3 movementDirectionVector)
    {
        // Built-in function. Returns an array containing all the colliders in the "circle" around you
        return Physics.OverlapSphere(transform.position + movementDirectionVector, detectionRadius, objectDetectionMask);
    }

    private void Start() 
    {
        mainCam = Camera.main;
    }
    // Mey method
    public void PerformDetection(Vector3 movementDirectionVector)
    {   
        // The colliders that are detected
        var colliders = DetectObjectsInFront(movementDirectionVector);
        // If you have left the collider you have to delete it
        collidersList.Clear();
        bool isUsableFound = false;
        // We check if there are interactable items between the colliders.  
        foreach (var collider in colliders)
        {   
            // We need IPickable type
            var pickableItem = collider.GetComponent<IPickable>();
            if (pickableItem != null)
            {
                // If the idem can be added, it is added to the colliders
                collidersList.Add(collider);
            }
            
            var usable = collider.GetComponent<IUsable>();
            // Change the material of the ipickable item
            if(usable != null && isUsableFound == false)
            {
                if(IUsableCollider != null)
                {
                    MaterialHelper.DisableEmission(iUsableCollider.gameObject);
                }
                IUsableCollider = collider;
                MaterialHelper.EnableEmission(IUsableCollider.gameObject, usableEmissionColor);
                isUsableFound = true;
                if (currentCollider != null)
                {
                    MaterialHelper.SwapToOriginalMaterial(currentCollider.gameObject, currentColliderMaterialsList);
                    currentCollider = null;
                }
                return;
            }
            
        }
       
        if(isUsableFound == false)
        {
            if (IUsableCollider != null)
            {
                MaterialHelper.DisableEmission(iUsableCollider.gameObject);
            }
            IUsableCollider = null;
        }
        // If no item collider recently
        if (collidersList.Count == 0)
        {   
            //And there were  one or more item in the colliderlist
            if (currentCollider != null)
            {   
                //Swaping it to the original material (we didnt picked it up, so it we will change te pickable material)
                SwapToOriginalMaterial();
                currentCollider = null;
            }
            return;
        }
        
        if (currentCollider == null)
        {
            currentCollider = collidersList[0];
            SwapToSelectionMaterial();
        }
        // if it is not in the sphere right now
        else if (collidersList.Contains(currentCollider) == false)
        {
            SwapToOriginalMaterial();
            currentCollider = collidersList[0];
            SwapToSelectionMaterial();
        }
    } 

   // In a raycast it tetects the colliders
    public void DetectColliderInFront()
    {
        
        RaycastHit hit;
        if(Physics.SphereCast(WeaponRaycastStartPosition.position,0.3f, transform.forward, out hit, attackDistance))
        {
            OnAttackSuccessful?.Invoke(hit.collider, hit.point);
        }
        
    }

    // Swaps the materials between normal material and selected material (if is an interactible object)
    private void SwapToSelectionMaterial()
    {
        currentColliderMaterialsList.Clear();

        if(currentCollider.transform.childCount > 0)
        {
            foreach(Transform child in currentCollider.transform)
            {
            PrepareRendererToSwapMaterials();              
            }
        }
        else
        {
            PrepareRendererToSwapMaterials();              

        }
    }

    // Swaps the material
    private void SwapMaterials(Renderer renderer)
    {
        Material[] matArray = new Material[renderer.materials.Length];
        for ( int i = 0; i < matArray.Length;i++)
        {
            matArray[i] = selectionMaterial;
        }
        renderer.materials = matArray;
    }

    // Swaps the material in the renderer 
    private void PrepareRendererToSwapMaterials()
    {
        var renderer = currentCollider.GetComponent<Renderer>();
        currentColliderMaterialsList.Add(renderer.sharedMaterials);
        SwapMaterials(renderer);  
    }
    
    // Gives us the original material back and swaps it with the highlited material
    private void SwapToOriginalMaterial()
    {
        if(currentColliderMaterialsList.Count > 1)
        {
            for (int i = 0; i < currentColliderMaterialsList.Count; i++ )
            {
                var renderer = currentCollider.transform.GetChild(i).GetComponent<Renderer>();
                renderer.materials = currentColliderMaterialsList[i];               
            }
        }
        else
        {
            var renderer = currentCollider.GetComponent<Renderer>();
            renderer.materials = currentColliderMaterialsList[0];
        }
    }
}

