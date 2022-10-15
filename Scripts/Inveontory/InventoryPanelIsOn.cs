using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryPanelIsOn : MonoBehaviour
{
    public GameObject itemPanel;

    public GameObject pausePanel;
    
    public GameObject sleepPanel;

    public GameObject noWeaponhand;

    public Animator animator;
    public GameObject isGroundedHelper;

    // Start is called before the first frame update
    void Start()
    {
        Transform[] children = this.transform.GetComponentsInChildren<Transform>(true);

        // We have to set the animator if we have an item in the item slot
        if(children.Length > 1)
        {
            animator = GetComponentInChildren<Animator>();
        }
        
         
    }

    // Update is called once per frame
    void Update()
    {
        Transform[] children = this.transform.GetComponentsInChildren<Transform>(true);

        // Checks that we have an item in the item slot. IF we dont have item, then we set the notWeaponHand to active
        if(children.Length > 1)
        {
            // Activate the weapon
            noWeaponhand.SetActive(false);
            animator = GetComponentInChildren<Animator>();
            // If a panel is active than we have to disable the animator
            if(itemPanel.activeSelf || pausePanel.activeSelf || sleepPanel.activeSelf)
            {
                animator.GetComponent<Animator>().enabled = false;
            }
            else
            {
                animator.GetComponent<Animator>().enabled = true;
            }

        }
        else
        {
            noWeaponhand.SetActive(true);
        }
       
        
    }
}
