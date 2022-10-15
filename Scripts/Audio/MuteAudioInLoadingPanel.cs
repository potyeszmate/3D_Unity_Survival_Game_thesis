using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuteAudioInLoadingPanel : MonoBehaviour
{
    AudioSource audioSource;    
    public GameObject loadingPanel;   


    // Start is called before the first frame update
    void Start()
    {
    audioSource = GetComponent<AudioSource>();  
    }


    // Mutes the audio when we are in the loading state
    void Update()
    {
        if (loadingPanel.activeSelf)
            audioSource.mute = true;
        else
            audioSource.mute = false;
    }
}
