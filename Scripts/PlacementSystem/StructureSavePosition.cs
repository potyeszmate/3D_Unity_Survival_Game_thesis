using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureSavePosition : Structure, IUsable
{
    [SerializeField]
    bool isUsable = true;
    public bool IsUsable => isUsable;

    public GameObject[] objectsToToggle;

    // If we use the structe that has this code attached to it than we show the sleep UI
    public void Use()
    {
        
        FindObjectOfType<StructuresInteractionManager>().ShowSLeepUI();

    }
}