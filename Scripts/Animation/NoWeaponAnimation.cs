using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoWeaponAnimation : MonoBehaviour
{

    float TimeT = 0;
    public Animator animator;

    public GameObject itemPanel,pausePanel,sleepPanel;
    public PlayerStat playerStat;

    [SerializeField]
    private AudioSource audioSource;

    public AudioClip runSfx;

    [SerializeField]
    private AudioClip woosh_Sound;
    public AudioClip[] attackSfx;

    public GameObject isGroundedHelper;
    public bool canRunAnimation = true;

    // Update is called once per frame - Nothing In Hand animations
    void Update()
    {
        if(playerStat.stamina < 1)
            canRunAnimation = false;
        else
            canRunAnimation = true;

        TimeT += Time.deltaTime;
        bool isInventoryOn = false;

        if(itemPanel.activeSelf || sleepPanel.activeSelf || pausePanel.activeSelf)
        {
            isInventoryOn = true;
        }

        if(itemPanel.activeSelf || pausePanel.activeSelf || sleepPanel.activeSelf)
        {
            animator.GetComponent<Animator>().enabled = false;
        }
        else
        {
            animator.GetComponent<Animator>().enabled = true;
        }

        if(Input.GetKeyDown(KeyCode.Mouse0) && TimeT > 1 && !isInventoryOn)
        { 

            Attack();
            TimeT = 0;
        }

        
        if ((Input.GetKey(KeyCode.W) || (Input.GetKey(KeyCode.S) || (Input.GetKey(KeyCode.A) || (Input.GetKey(KeyCode.D))))) && Input.GetKey(KeyCode.LeftShift))
        { 
            if(canRunAnimation)
                Run();
            else
                Walk();
            
        }
            
        else if((Input.GetKey(KeyCode.W) || (Input.GetKey(KeyCode.S) || (Input.GetKey(KeyCode.A) || (Input.GetKey(KeyCode.D))))))   // moveDirection != Vector3.zero && 
        {
            Walk();   
        }
        else
        {
            Idle();
        }
        
    }

    // Idle anim setting
    private void Idle()
    {
        animator.SetBool("Idle",true);
        animator.SetBool("Walk",false);
        animator.SetBool("Run",false);
    }

    // walk anim setting
    private void Walk()
    {
        animator.SetBool("Walk",true);
        animator.SetBool("Idle",false);
        animator.SetBool("Run",false);
    }

    // Run anim setting
    private void Run()
    {
        animator.SetBool("Run",true);
        animator.SetBool("Idle",false);
        animator.SetBool("Walk",false);
    }

    // Attack anim setting
    private void Attack()
    {
        animator.SetTrigger("Attack");
    }

    // Idle anim setting
    IEnumerator RunAnimation()
    {
        yield return new WaitForSeconds(0.3f);

        if(isGroundedHelper.activeSelf == true)
        {
            if(canRunAnimation)
                Run();
            else
                Walk();
        }    

    }

    // Idle anim setting
    IEnumerator WalkAnimation()
    {
        yield return new WaitForSeconds(0.3f);

        if(isGroundedHelper.activeSelf == true)
            Walk();
        
    }

    // Using in animation tab - Plays hand punching sound
    private void PlayHandWhoshSound()
    {
        audioSource.clip = woosh_Sound;
        audioSource.Play();

    }

    // Using in animation tab - Plays player punching sound effect
    private void PlayHitSound()
    {
        audioSource.clip = attackSfx[Random.Range(0, attackSfx.Length)];
        audioSource.PlayOneShot(audioSource.clip);
    }

    
}
