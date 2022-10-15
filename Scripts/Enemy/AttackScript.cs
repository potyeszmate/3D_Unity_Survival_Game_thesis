using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackScript : MonoBehaviour {

    public float damage = 2f;
    public float radius = 1f;
    public LayerMask layerMask;
	
	void Update () {

        // Chechks the Attacking object's collider (if not a pistol of course)
        Collider[] hits = Physics.OverlapSphere(transform.position, radius, layerMask);

        if(hits.Length > 0) {
            
            // when an attack is happening it deals the certain damage to the health (as a player or an enemy)
            hits[0].gameObject.GetComponent<HealthScript>().ApplyDamage(damage);
            gameObject.SetActive(false);

        }

	}

} 




























