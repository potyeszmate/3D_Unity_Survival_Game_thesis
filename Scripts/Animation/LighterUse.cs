using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LighterUse : MonoBehaviour
{
    float TimeT = 0;
    public Animator animator;
    public GameObject flameGameObject;

    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private AudioClip lighterOn;

    [SerializeField]
    private AudioClip lighterOff;
    private PlayerStat playerStat;
    public bool canRunAnimation = true;


    private void Start() 
    {
        playerStat = GameObject.Find("Player").GetComponent<PlayerStat>();
    }

    // Update is called once per frame - Lighter Animations
    void Update()
    {
        if(playerStat.stamina < 1)
            canRunAnimation = false;
        else
            canRunAnimation = true;


        TimeT += Time.deltaTime;
        

        if ((Input.GetKey(KeyCode.W) || (Input.GetKey(KeyCode.S) || (Input.GetKey(KeyCode.A) || (Input.GetKey(KeyCode.D))))) && Input.GetKey(KeyCode.LeftShift))
        { 
            
            //Run();
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

        // Right click - switching on and off the lighter (turning on and off)
        if (Input.GetKeyDown(KeyCode.Mouse1) && TimeT > 2)
        {
            if(flameGameObject.activeSelf == true)
            {
                SwitchOff();
            }
            else
            {
                SwitchOn();
            }

        }
    }

    private void Idle()
    {
        animator.SetBool("LighterIdle",true);
        animator.SetBool("LighterWalk",false);
        animator.SetBool("LighterRun",false);

    }

    private void Walk()
    {
        animator.SetBool("LighterWalk",true);
        animator.SetBool("LighterIdle",false);
        animator.SetBool("LighterRun",false);
    }

    private void Run()
    {
        animator.SetBool("LighterRun",true);
        animator.SetBool("LighterIdle",false);
        animator.SetBool("LighterWalk",false);
    }

    
    // Swithcin off animation to the lighter
    private void SwitchOff()
    {
        animator.SetTrigger("LighterOff");
    }

    // Swithcin on animation to the lighter
    private void SwitchOn()
    {
        animator.SetTrigger("LighterOn");
    }

    // Turning on the flame in the lighter - Using in animation tab
    public void SetFlameOn()
    {
        flameGameObject.SetActive(true);  
    }

    // Turning off the flame in the lighter - Using in animation tab
    public void SetFlameOff()
    {
        flameGameObject.SetActive(false);  
    }

    // Lighter turning on audio - Using in animation tab
    void PlayLighterOn() {
        audioSource.clip = lighterOn;
        audioSource.Play();
    }
    
    // Lighter turning off audio - Using in animation tab
    void PlayLighterOff() {
        audioSource.clip = lighterOff;
        audioSource.Play();
    }
}
