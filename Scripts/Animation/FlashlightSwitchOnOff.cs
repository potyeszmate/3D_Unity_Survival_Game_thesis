using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightSwitchOnOff : MonoBehaviour
{
    float TimeT = 0;
    public Animator animator;
    public GameObject lightGameObject;

    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private AudioClip flashlightswitch;

    [SerializeField]
    private AudioClip[] attackSfx;
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
    
        if(Input.GetKeyDown(KeyCode.Mouse1) && !lightGameObject.activeSelf )
        {
            lightGameObject.SetActive(true);
            PlayFlashlightSwitch();
        }
        else if(Input.GetKeyDown(KeyCode.Mouse1) && lightGameObject.activeSelf)
        {
            lightGameObject.SetActive(false);
            PlayFlashlightSwitch();
        }


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
            
        else if((Input.GetKey(KeyCode.W) || (Input.GetKey(KeyCode.S) || (Input.GetKey(KeyCode.A) || (Input.GetKey(KeyCode.D))))))   // moveDirection != Vector3.zero && 
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
        animator.SetBool("FlashlightIdle",true);
        animator.SetBool("FlashlightWalk",false);
        animator.SetBool("FlashlightRun",false);
    }

    private void Walk()
    {
        animator.SetBool("FlashlightWalk",true);
        animator.SetBool("FlashlightIdle",false);
        animator.SetBool("FlashlightRun",false);
    }

    private void Run()
    {
        animator.SetBool("FlashlightRun",true);
        animator.SetBool("FlashlightIdle",false);
        animator.SetBool("FlashlightWalk",false);

    }

    private void Attack()
    {
        animator.SetTrigger("FlashlightAttack");
    }

    void PlayFlashlightSwitch() 
    {
        audioSource.clip = flashlightswitch;
        audioSource.Play();
    }

    private void PlayHitSound()
    {
        audioSource.clip = attackSfx[Random.Range(0, attackSfx.Length)];
        audioSource.PlayOneShot(audioSource.clip);
    }
}
