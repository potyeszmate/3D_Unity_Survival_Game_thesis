using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeWeaponState : MonoBehaviour
{
    float TimeT = 0;
    public Animator animator;
    private PlayerStat playerStat;
    public bool canRunAnimation = true;

    
    private void Start() 
    {
        playerStat = GameObject.Find("Player").GetComponent<PlayerStat>();
    }

    // Update is called once per frame
    void Update()
    {
        if(playerStat.stamina < 1)
            canRunAnimation = false;
        else
            canRunAnimation = true;

        TimeT += Time.deltaTime;
        
        // Attack animation by left click, can use it every 1 sec
        if(Input.GetKeyDown(KeyCode.Mouse0) && TimeT > 1)
        {
            Attack();
            TimeT = 0;
        }
        
        // Running animation by pressing w,a,s,d and lshift
        if ((Input.GetKey(KeyCode.W) || (Input.GetKey(KeyCode.S) || (Input.GetKey(KeyCode.A) || (Input.GetKey(KeyCode.D))))) && Input.GetKey(KeyCode.LeftShift))
        { 
            //Run(); 
            if(canRunAnimation)
                Run();
            else
                Walk();
        }

        // Walking animation by pressing w,a,s,d   
        else if((Input.GetKey(KeyCode.W) || (Input.GetKey(KeyCode.S) || (Input.GetKey(KeyCode.A) || (Input.GetKey(KeyCode.D))))))   // moveDirection != Vector3.zero && 
        {
            Walk();
        }

        // Idle animation (not doing anything)
        else
        {
            Idle();
        }
        
    }

    // Setting the right animation - Idle
    private void Idle()
    {
        animator.SetBool("KnifeIdle",true);
        animator.SetBool("KnifeWalk",false);
        animator.SetBool("KnifeRun",false);
    }

    // Setting the right animation - Walk
    private void Walk()
    {
        animator.SetBool("KnifeWalk",true);
        animator.SetBool("KnifeIdle",false);
        animator.SetBool("KnifeRun",false);
    }

    // Setting the right animation - Run
    private void Run()
    {
        animator.SetBool("KnifeRun",true);
        animator.SetBool("KnifeIdle",false);
        animator.SetBool("KnifeWalk",false);
    }

    // Setting the right animation - Attack
    private void Attack()
    {
        animator.SetTrigger("KnifeAttack");
    }

}
