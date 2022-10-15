//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentMovement : MonoBehaviour
{
    public CharacterController characterController;
    private Animator animator;
    public GameObject itemPanel;
    public GameObject HandWithNoWeapon;

    public bool playerGrounded;

    public GameObject isGroundedHelper;
    public AudioSource audioSource;
    public AudioClip[] jumpSfx;
    public AudioClip runSfx;

    public float gravity = 14f;
    [SerializeField] public float jumpHeight = 5f;

    [SerializeField] public float walkingSpeed = 2.5f;

    [SerializeField] public float runningSpeed = 3.8f;

    public bool canPlayerRun = true;

    public Vector3 moveDirection = Vector3.zero;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();

    }

    // Moving player
    private void Update()
    {
        MovePlayer();
        CheckIsGrounded();    
        
    }
    
    //Moves the player and sets its speed
    private void MovePlayer()
    {

        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");
        
        if(characterController.isGrounded)
        {
            
            bool isWalking = true;

            if ((Input.GetKey(KeyCode.W) || (Input.GetKey(KeyCode.S) 
            || (Input.GetKey(KeyCode.A) || (Input.GetKey(KeyCode.D))))) 
            && moveDirection != Vector3.zero && Input.GetKey(KeyCode.LeftShift))
            {
                isWalking = false;
                
            }
            else if((Input.GetKey(KeyCode.W) || (Input.GetKey(KeyCode.S) || 
            (Input.GetKey(KeyCode.A) || (Input.GetKey(KeyCode.D))))) 
            && moveDirection != Vector3.zero)  
            {
                isWalking = true;  
                
            }

            // Character speed
            float currentSpeed = isWalking ? walkingSpeed : canPlayerRun ? runningSpeed : walkingSpeed;

            // Character movement
            moveDirection = transform.forward * vertical + transform.right * horizontal;
            moveDirection *= currentSpeed;

            Jump();

        }
        
        // Real time movement
        characterController.Move(moveDirection * Time.deltaTime);
        moveDirection.y -= gravity * Time.deltaTime;
            
        
    }

    // Jump if space pressed
    private void Jump()
    {
        if(isGroundedHelper.activeSelf)
        {
            if(Input.GetButton("Jump"))
            {   
                moveDirection.y = jumpHeight;
                StartCoroutine(PlayJumpSound());
                
            }
        }
        
    }

    // Sets the players position in the maps
    public void TeleportPlayerTo(Vector3 position)
    {
        characterController.enabled = false;
        transform.position = position;
        characterController.enabled = true;
    }

    // Not using riht now, check if its grounded, avilable from everywhere in the project
    private void CheckIsGrounded()
    {
        if(moveDirection.y <= 0 && moveDirection.y >= -0.8f)
        {
            //Debug.Log("Grounded");
            isGroundedHelper.SetActive(true);
        }  
        else
        {
            //Debug.Log("Not grounded");
            isGroundedHelper.SetActive(false);
        }
            

    }

    // Jump Sound
    IEnumerator PlayJumpSound() 
    {
        yield return new WaitForSeconds(0.1f);
        
        audioSource.clip = jumpSfx[Random.Range(0, jumpSfx.Length)];
        audioSource.Play();
    }
    
}
