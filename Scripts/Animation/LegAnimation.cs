using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegAnimation : MonoBehaviour
{
    float TimeT = 0;
    public Animator animator;
    public GameObject itemPanel;
    public PlayerStat playerStat;

    [SerializeField]
    private AudioSource audioSource;

    public AudioClip runSfx;

    [SerializeField]
    private AudioClip woosh_Sound;
    public AudioClip[] attackSfx;

    public GameObject isGroundedHelper;
    public bool canRunAnimation = true;

    // Update is called once per frame - Leg animations
    void Update()
    {
        if(playerStat.stamina < 1)
            canRunAnimation = false;
        else
            canRunAnimation = true;


        TimeT += Time.deltaTime;
        bool isInventoryOn = false;

        if(itemPanel.activeSelf)
        {
            isInventoryOn = true;
        }

        // Kicking animation - Can kick in every 1.7 sec by pressing q button
        if(Input.GetKeyDown(KeyCode.Q) && TimeT > 1.70f && !isInventoryOn)
        { 

            Attack();
            TimeT = 0;
        }

        // Running animation
        if ((Input.GetKey(KeyCode.W) || (Input.GetKey(KeyCode.S) || (Input.GetKey(KeyCode.A) || (Input.GetKey(KeyCode.D))))) 
            && Input.GetKey(KeyCode.LeftShift))
        { 
            // If the stamina is 0 then we cant run, just walk
            if(canRunAnimation)
                Run();
            else
                Walk();
        }

        //Walking animation    
        else if((Input.GetKey(KeyCode.W) || (Input.GetKey(KeyCode.S) || (Input.GetKey(KeyCode.A) || (Input.GetKey(KeyCode.D))))))   // moveDirection != Vector3.zero && 
        {
            Walk();
        }

        //Dont Move
        else
        {
            Idle();
        }
        
    }

    //Not moving
    private void Idle()
    {
        animator.SetBool("LegIdle",true);
        animator.SetBool("LegWalk",false);
        animator.SetBool("LegRun",false);

    }
    //Walking
    private void Walk()
    {
        animator.SetBool("LegWalk",true);
        animator.SetBool("LegIdle",false);
        animator.SetBool("LegRun",false);
    }
    //Running
    private void Run()
    {
        animator.SetBool("LegRun",true);
        animator.SetBool("LegIdle",false);
        animator.SetBool("LegWalk",false);
    }
    //Kicking
    private void Attack()
    {
        animator.SetTrigger("LegKick");
    }
    
}
