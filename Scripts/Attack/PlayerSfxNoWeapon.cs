using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSfxNoWeapon : MonoBehaviour
{
    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private AudioClip runningSound;

    // Player Hit sound if we have no weapon (need an other code, because it an other object attached to an other parent)
    void PlayWooshSound() {
        audioSource.clip = runningSound;
        audioSource.Play();
    }
}
