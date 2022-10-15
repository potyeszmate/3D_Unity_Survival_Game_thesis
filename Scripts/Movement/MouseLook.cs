using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public GameObject inventoryGeneralPanel;
    public GameObject gameMenuPanel, loadingPanel,sleepingPanel,Dot,cookingPanel;
    public Transform playerTransform;
    float xRotation = 0f;

  
    void Update()
    {
        if (cookingPanel.activeSelf == false && inventoryGeneralPanel.activeSelf == false && gameMenuPanel.activeSelf == false && loadingPanel.activeSelf == false && sleepingPanel.activeSelf == false)
        {
            // Mouse position
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            // Between -50 és 55 degree we can look around
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -50f, 55);

            // Camera rotation
            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            playerTransform.Rotate(Vector3.up * mouseX);
        }

        // The crosshairs is active when we are not in any panels/menu
        if(inventoryGeneralPanel.activeSelf == true || gameMenuPanel.activeSelf == true || loadingPanel.activeSelf == true || sleepingPanel.activeSelf == true)
        {
            if(Dot.activeSelf == true)
                Dot.SetActive(false);
        }
        else
        {
            if(Dot.activeSelf == false)
                Dot.SetActive(true);
        }
        

    }
}
