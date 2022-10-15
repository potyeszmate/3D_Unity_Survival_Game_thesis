using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingRod : MonoBehaviour
{
    float TimeT = 0;
    public Animator animator;

    [SerializeField]
    private AudioSource audioSource;
    
    [SerializeField]
    private AudioClip[] woosh_Sounds;
    private PlayerStat playerStat;
    public bool canRunAnimation = true;


    
    [SerializeField]
    private AudioClip[] attackSfx;

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
        if(Input.GetKeyDown(KeyCode.Mouse1) && TimeT > 1)
        {
            Use();
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
        animator.SetBool("FishingIdle",true);
        animator.SetBool("FishingWalk",false);
        animator.SetBool("FishingRun",false);
    }

    // Setting the right animation - Walk
    private void Walk()
    {
        animator.SetBool("FishingWalk",true);
        animator.SetBool("FishingIdle",false);
        animator.SetBool("FishingRun",false);
    }

    // Setting the right animation - Run
    private void Run()
    {
        animator.SetBool("FishingRun",true);
        animator.SetBool("FishingIdle",false);
        animator.SetBool("FishingWalk",false);
    }

    // Setting the right animation - Attack
    private void Use()
    {
        animator.SetTrigger("FishingUse");
    }

    // sounds

     // Axe woosh sound when we attack (or hit a tree or box) - Using in the animation tab
    void PlayWooshSound() 
    {
        audioSource.clip = woosh_Sounds[Random.Range(0, woosh_Sounds.Length)];
        audioSource.Play();
    }

    // Player attack sound - Using in the animation tab
    private void PlayHitSound()
    {
        audioSource.clip = attackSfx[Random.Range(0, attackSfx.Length)];
        audioSource.PlayOneShot(audioSource.clip);
    }

}
