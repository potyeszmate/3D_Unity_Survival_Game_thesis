using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAudio : MonoBehaviour {

    private AudioSource audioSource;

    [SerializeField]
    private AudioClip scream_Clip, die_Clip;

    [SerializeField]
    private AudioClip[] attack_Clips;

    // Initialization
    void Awake () {
        audioSource = GetComponent<AudioSource>();
	}

    // Using these Audio plays in the enemy's animation tab 

    // When the enemy runs away or attacks us (he screams)
    public void Play_ScreamSound() {
        audioSource.clip = scream_Clip;
        audioSource.Play();
    }

    // When the enemy attacks us
    public void Play_AttackSound() {
        audioSource.clip = attack_Clips[Random.Range(0, attack_Clips.Length)];
        audioSource.Play();
    }

    // When the enemy dies
    public void Play_DeadSound() {
        audioSource.clip = die_Clip;
        audioSource.Play();
    }

}


































