using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FootstepsSound : MonoBehaviour
{
    public List<AudioClip> walkingFootsteps = new List<AudioClip>();
    public List<AudioClip> jumpingSounds = new List<AudioClip>();
    public List<AudioClip> runningFootsteps = new List<AudioClip>();
    public List<AudioClip> LandingSounds = new List<AudioClip>();

    AudioSource audioSource;
   
    bool nextStep;
    float footstepInterval;
    
    int delay = 700;


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        nextStep = false;   
    }

    
    void Update()
    {   
        bool isRunning = Input.GetKey(KeyCode.LeftShift);

        // Currently not using this because we use the stepound in the animation.
        if(footstepInterval > delay)
        {
            footstepInterval = 0;
            nextStep = true;
        }
            
    }

    // Plays the walking step sounds - Using it in the leg animation tab when the leg hits the floor
    void PlayWalkingSounds()
    {
        nextStep = false;
        int surfaceIndex = TerrainSurface.GetMainTexture(transform.position);
        audioSource.clip = walkingFootsteps[surfaceIndex];
        audioSource.PlayOneShot(audioSource.clip);
    }

    // Plays the running step sounds - Using it in the leg animation tab when the leg hits the floor
    void RunningStepsSound()
    {   
        nextStep = false;
        int surfaceIndex = TerrainSurface.GetMainTexture(transform.position);
        audioSource.clip = runningFootsteps[surfaceIndex];
        audioSource.PlayOneShot(audioSource.clip);      
    }

}
