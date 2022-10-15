using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAxeWooshSound : MonoBehaviour {

    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private AudioClip[] woosh_Sounds;

    [SerializeField]
    private AudioClip[] attackSfx;

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
