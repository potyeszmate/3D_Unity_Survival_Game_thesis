using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureOnOff :  Structure, IUsable
{
    [SerializeField]
    bool isUsable = true;
    public bool IsUsable => isUsable;
    public GameObject[] objectsToToggle;
    
    
   

    // We toggling on and off the prefab (if we use it than on if we dont than off)
    public void Use()
    {

        foreach (var objectToToggle in objectsToToggle)
        {
            objectToToggle.SetActive(!objectToToggle.activeSelf);
        }

        FindObjectOfType<Cookingsystem>().ToggleCraftingUI();


    }
}
