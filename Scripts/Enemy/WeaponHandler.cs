using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : MonoBehaviour {

    private Animator anim;

    public GameObject attack_Point;

    void Awake() 
    {
       anim = GetComponent<Animator>();
    }
    
    // Turning on the attack point of the tool or weapon (animation tab)
    void Turn_On_AttackPoint() 
    {
        attack_Point.SetActive(true);
    }

    // Turning off the attack point of the tool or weapon (animation tab)
    void Turn_Off_AttackPoint() 
    {
        if(attack_Point.activeInHierarchy) 
        {
            attack_Point.SetActive(false);
        }
    }
    

} 

