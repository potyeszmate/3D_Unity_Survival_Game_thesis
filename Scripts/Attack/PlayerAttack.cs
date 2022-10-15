
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour {

    public float damage = 20f;

    public Animator zoomCameraAnim;
    private bool zoomed;

    private Camera mainCam;

    private GameObject crosshair;

    private bool is_Aiming;

    float TimeT = 0;

    void Awake() 
    {
        crosshair = GameObject.FindWithTag(Tags.CROSSHAIR);
        mainCam = Camera.main;

    }

	// Update is called once per frame - Shooting with the Pistol
	void Update () 
    {
         WeaponShoot();
    }

    // 
    void WeaponShoot() 
    {
        TimeT += Time.deltaTime;
        // if we have a pistol and shooting with left click (can shoot every 1.6 sec)
        if(Input.GetKeyDown(KeyCode.Mouse0) && TimeT > 1.6f ) 
        {
            StartCoroutine(Fire());
            TimeT = 0;
        } 

    }

    // Checks if the bullet hits the enemy with a Raycast method
    void BulletFired() 
    {
        
        RaycastHit hit;

        // Raycast that checks if we hit something in the middle of the screen
        if(Physics.Raycast(mainCam.transform.position, mainCam.transform.forward, out hit)) 
        {   
            // If that is an object with the enemy tag, then aplly the dmg to the enemy
            if(hit.transform.tag == Tags.ENEMY_TAG) 
            {
                hit.transform.GetComponent<HealthScript>().ApplyDamage(damage);
            }

        }

    }

    // Using a delay in the BulletFired, when we shoot, after 0.4 sec the enemy gets the dmg 
    IEnumerator Fire() 
    {
        yield return new WaitForSeconds(0.4f);
        BulletFired();

    }

}
































