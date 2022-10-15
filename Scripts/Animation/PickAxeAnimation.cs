using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickAxeAnimation : MonoBehaviour
{

    float TimeT = 0;
    public Animator animator;

    private bool isInventoryOn;



    [SerializeField]
    private AudioClip[] attackSfx;

    [SerializeField]
    private AudioSource audioSource;

     private PlayerStat playerStat;
    public bool canRunAnimation = true;

    private void Start() 
    {
        playerStat = GameObject.Find("Player").GetComponent<PlayerStat>();
    }

    // Update is called once per frame - PickAxe animations
    void Update()
    {
        if(playerStat.stamina < 1)
            canRunAnimation = false;
        else
            canRunAnimation = true;

        TimeT += Time.deltaTime;
        
        if(Input.GetKeyDown(KeyCode.Mouse0) && TimeT > 1 && !isInventoryOn)
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
        
        animator.SetBool("PickAxeIdle",true);
        animator.SetBool("PickAxeWalk",false);
        animator.SetBool("PickAxeRun",false);

    }

    private void Walk()
    {
        animator.SetBool("PickAxeWalk",true);
        animator.SetBool("PickAxeIdle",false);
        animator.SetBool("PickAxeRun",false);

    }

    private void Run()
    {
        animator.SetBool("PickAxeRun",true);
        animator.SetBool("PickAxeIdle",false);
        animator.SetBool("PickAxeWalk",false);

    }

    private void Attack()
    {
        animator.SetTrigger("PickAxeAttack");
    }
    
}
