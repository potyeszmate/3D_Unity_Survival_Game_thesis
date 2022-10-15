using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HasChildrenWeapon : MonoBehaviour
{
    public GameObject playerHandsWithoutWaepon;


    // Update is called once per frame - Currently not using ( checked the children of the item handler, but we have the method for it in other class)
    void Update()
    {
       Transform[] children = this.transform.GetComponentsInChildren<Transform>(true);

        if (children.Length <= 1)
        {
            playerHandsWithoutWaepon.SetActive(true);
        }
        else
        {
            playerHandsWithoutWaepon.SetActive(false);
        }
    }
}
