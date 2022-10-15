using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlacementHelper : MonoBehaviour
{
    Transform playerTransform;
    BoxCollider boxCollider;
    Rigidbody rb;
    public List<Collider> collisions = new List<Collider>();

    List<Material[]> objectMaterials = new List<Material[]>();

    private float raycastMaxDistance = 5;
    private float maxheightDifference = .3f;

    LayerMask layerMask;

    Material m_material;

    float lowestYHeight = 0;

    public bool CorrectLocation { get; private set; }
    private bool stopMovement = false;

    private void Start()
    {
        // We need to place the object to the ground
        layerMask.value = 1 << LayerMask.NameToLayer("Ground");
    }

    public void Initialize(Transform transform)
    {
        playerTransform = transform;
    }
    

    //  The box collider, rigidbody and the renderer will be used in this system
    public void PrepareForMovement()
    {
        boxCollider = GetComponent<BoxCollider>();
        rb = GetComponent<Rigidbody>();
        m_material = GetComponent<Renderer>().material;
    }

    // We dont need a trigger for the collider and a rigid body (because we will move the item and place it)
    public Structure PrepareForPlacement()
    {
        stopMovement = true;
        Destroy(rb);
        boxCollider.isTrigger = false;
        var structureComponent = GetComponent<Structure>();
        if(structureComponent == null)
        {
            structureComponent = gameObject.AddComponent<Structure>();
        }
        return structureComponent;
    }

    // We check if we can place the object or not
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer != LayerMask.NameToLayer("Pickable") && other.gameObject.layer != LayerMask.NameToLayer("Player") && other.gameObject.layer != LayerMask.NameToLayer("Ground"))
        {
            if(collisions.Contains(other)!= true)
            {
                collisions.Add(other);
                ChangeMaterialColor(Color.red);
            }
        }
    }

    // In exit we change the material to green and remove the collision (because its placable)
    private void OnTriggerExit(Collider other)
    {
        collisions.Remove(other);
        if(collisions.Count == 0)
        {
            ChangeMaterialColor(Color.green);
        }
    }
    
    
    private void FixedUpdate()
    {
        if(stopMovement == false && playerTransform != null)
        {
            var positionToMove = playerTransform.position + playerTransform.forward;
            rb.position = positionToMove;
            rb.MoveRotation(Quaternion.LookRotation(playerTransform.forward));
            if(collisions.Count == 0)
            {   
                
                // Find corners of box collider
                Vector3 bottomCenter = new Vector3(boxCollider.center.x, boxCollider.center.y - boxCollider.size.y / 2f,boxCollider.center.z);
                Vector3 topLeftCorner = bottomCenter + new Vector3(-boxCollider.size.x / 2f, 0, boxCollider.size.z / 2f);
                Vector3 topRightCorner = bottomCenter + new Vector3(+boxCollider.size.x / 2f, 0, boxCollider.size.z / 2f);
                Vector3 bottomLeftCorner = bottomCenter + new Vector3(-boxCollider.size.x / 2f, 0, -boxCollider.size.z / 2f);
                Vector3 bottomRightCorner = bottomCenter + new Vector3(boxCollider.size.x / 2f, 0, -boxCollider.size.z / 2f);

                // Shoot rays from thos points
                Debug.DrawRay(transform.TransformPoint(topLeftCorner) + Vector3.up, Vector3.down * raycastMaxDistance, Color.magenta);
                Debug.DrawRay(transform.TransformPoint(topRightCorner) + Vector3.up, Vector3.down * raycastMaxDistance, Color.magenta);
                Debug.DrawRay(transform.TransformPoint(bottomLeftCorner) + Vector3.up, Vector3.down * raycastMaxDistance, Color.magenta);
                Debug.DrawRay(transform.TransformPoint(bottomRightCorner) + Vector3.up, Vector3.down * raycastMaxDistance, Color.magenta);

                // Gives the Raycast to the object by the corners of the prefab
                RaycastHit hit1, hit2, hit3, hit4;
                bool result1 = Physics.Raycast(transform.TransformPoint(topLeftCorner) + Vector3.up, Vector3.down, out hit1, raycastMaxDistance, layerMask);
                bool result2 = Physics.Raycast(transform.TransformPoint(topRightCorner) + Vector3.up, Vector3.down, out hit2, raycastMaxDistance, layerMask);
                bool result3 = Physics.Raycast(transform.TransformPoint(bottomLeftCorner) + Vector3.up, Vector3.down, out hit3, raycastMaxDistance, layerMask);
                bool result4 = Physics.Raycast(transform.TransformPoint(bottomRightCorner) + Vector3.up, Vector3.down, out hit4, raycastMaxDistance, layerMask);

                // If we have the raycast
                if(result1 && result2 && result3 && result4)
                {   
                    // These are the  values  of the raycast
                    float[] heightValuesList = { hit1.point.y, hit2.point.y , hit3.point.y , hit4.point.y };
                    // The min and the max value
                    var min = heightValuesList.Min();
                    var max = heightValuesList.Max();
                    // If the min is smaller than 0 than it cant be placed (dont palce it under the ground)
                    if(min < lowestYHeight)
                    {
                        ChangeMaterialColor(Color.red);
                        Debug.Log("Cant place that low");
                        CorrectLocation = false;
                    }
                    // checks the difference of the max and the min and if it is too high than we cant place it (optional value)
                    else if(max-min > maxheightDifference)
                    {
                        Debug.Log("Too bigh height difference");
                        ChangeMaterialColor(Color.red);
                        CorrectLocation = false;
                    }
                    // else we place it to the selected groun(! -> just to the baked ground orobjects)
                    else
                    {   
                        Debug.Log("Placement position correct");
                        //ChangeMaterialColor(Color.green);
                        rb.position = new Vector3(positionToMove.x, (max + min) / 2f, positionToMove.z);
                        CorrectLocation = true;
                    }

                }
                
            }
        }
    }

    // We can change the color of the material if its wrong or not in placement 
    private void ChangeMaterialColor(Color color)
    {
        //m_material.SetColor("Color_Shader", color);

    }

    // Destroy the gameobject when esc pressed
    internal void DestroyStructure()
    {
        Destroy(gameObject);
    }
}
