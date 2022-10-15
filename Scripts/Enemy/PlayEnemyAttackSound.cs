using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayEnemyAttackSound : MonoBehaviour
{
    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private AudioClip[] enemyAttackSounds;

    // Plays the enemy's attack sound
    void Play_EnemyAttackSound() 
    {
        audioSource.clip = enemyAttackSounds[Random.Range(0, enemyAttackSounds.Length)];
        audioSource.Play();
    }
}
