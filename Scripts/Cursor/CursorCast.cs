using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorCast : MonoBehaviour
{
    public static float distanceFromTarget;
    public float toTarget;

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;

        // Simple Raycast - Uses when we hit something (tree, stone, box, barell)
        if ( Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit))
        {
            toTarget = hit.distance;
            distanceFromTarget = toTarget;
        }
    }
}
