using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAnimationSwitch : MonoBehaviour
{

    float TimeT = 0;
    public Animator animator;
    public bool canRunAnimation = true;
    private PlayerStat playerStat;

    private void Start() 
    {
        playerStat = GameObject.Find("Player").GetComponent<PlayerStat>();
    }

    // Update is called once per frame - Axe animation
    void Update()
    {
        
        if(playerStat.stamina < 1)
            canRunAnimation = false;
        else
            canRunAnimation = true;
        
        TimeT += Time.deltaTime;
        
        if(Input.GetKeyDown(KeyCode.Mouse0) && TimeT > 1)
        {
            Attack();
            TimeT = 0;
        }

        if ((Input.GetKey(KeyCode.W) || (Input.GetKey(KeyCode.S) || (Input.GetKey(KeyCode.A) || (Input.GetKey(KeyCode.D))))) && Input.GetKey(KeyCode.LeftShift))
        { 
            //Run();
            
            if(canRunAnimation)
                Run();
            else
                Walk();
            
        }
            
        else if((Input.GetKey(KeyCode.W) || (Input.GetKey(KeyCode.S) || (Input.GetKey(KeyCode.A) || (Input.GetKey(KeyCode.D))))))  
        {
            Walk();
        }
        else
        {
            Idle();
        }

    }


    private void Idle()
    {
        animator.SetBool("AxeIdle",true);
        animator.SetBool("AxeWalk",false);
        animator.SetBool("AxeRun",false);

    }

    private void Walk()
    {
        animator.SetBool("AxeWalk",true);
        animator.SetBool("AxeIdle",false);
        animator.SetBool("AxeRun",false);

    }

    private void Run()
    {
        animator.SetBool("AxeRun",true);
        animator.SetBool("AxeIdle",false);
        animator.SetBool("AxeWalk",false);
    }

    private void Attack()
    {
        animator.SetTrigger("AxeAttack"); 
    }

}
