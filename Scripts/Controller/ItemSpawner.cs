using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    // Here we can spawn items. Currently not using, beacuse it wont work with my saving system.
    //It works just if I manually place items to the map and then drag it to the game manager

    public ItemSO itemToSpawn;
    [Range(1,10)]
    public int count = 1;
    public bool singleObject = false;
    
    [Range(0.1f, 50f)]
    public float radius = 1;
    public bool showGizmo = true;
    public Color gizmoColor = Color.green;

    [SerializeField]
    private bool respawnable = false;

    public bool Respawnable
    {
        get { return respawnable; }
    }

    // Just a helper that show us the spawnpoints in editor
    public void OnDrawGizmos()
    {
        if(showGizmo && radius > 0)
        {
            // Draw the gizmos
            Gizmos.color = gizmoColor;
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }
}
