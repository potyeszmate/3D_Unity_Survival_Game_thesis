using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeProperties : MonoBehaviour {

    public int durability = 10;
    // Tree object
    GameObject thisTree;

    private Rigidbody rb;

    private new AudioSource audio;
    public AudioClip sfx;
    private bool isFalling = false;
    
    public Transform log;


    

    // Use this for initialization
    void Start ()
    {
        audio = transform.parent.GetComponent<AudioSource>();
        thisTree = transform.parent.gameObject;
        rb = thisTree.GetComponent<Rigidbody>();
        rb.mass = 80;
        rb.isKinematic = true;
        rb.useGravity = false;;
	}
	
	// Update is called once per frame - Destroying the tree - same as the barells
	void Update ()
    {
        if (durability <= 0 & isFalling == false)
        {
            PlayFallingSound();
            rb.useGravity = true;
            rb.isKinematic = false;
            rb.AddForce(Vector3.forward, ForceMode.Impulse);
            StartCoroutine(DestroTree());
            isFalling = true;
        }
	}

    IEnumerator DestroTree()
    {
        yield return new WaitForSeconds(8);

        int minVal = 1;
        int maxVal = 5;

        int rand = Random.Range(minVal, maxVal);

        // Giving random amount of trees (between 1 nad 5)
        for (int i = 0; i < rand; i++)
        {
            Instantiate(log, thisTree.transform.position, thisTree.transform.rotation);
        }

        thisTree.SetActive(false);
    }

    // Play the barell breaking audio
    void PlayFallingSound()
    {
        audio.clip = sfx;
        audio.PlayOneShot(audio.clip);
    }
}