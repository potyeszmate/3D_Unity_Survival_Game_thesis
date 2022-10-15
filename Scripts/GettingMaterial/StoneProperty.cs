using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneProperty : MonoBehaviour {

    public int durability = 10;
    // A fa amit vágunk
    GameObject thisStone;

    private Rigidbody rb;

    private new AudioSource audio;
    public AudioClip sfx;
    private bool isBreaking = false;
    // A kő amit kapunk
    public Transform stone;


    

    // Use this for initialization
    void Start ()
    {
        audio = transform.GetComponent<AudioSource>();
        thisStone = transform.gameObject;
        rb = thisStone.GetComponent<Rigidbody>();
        rb.mass = 80;
        rb.isKinematic = true;
        rb.useGravity = false;;
	}
	
	// Update is called once per frame - Destroying the tree - same as the barells
	void Update ()
    {
        if (durability <= 0 & isBreaking == false)
        {
            PlayStoneBreakSound();
            rb.useGravity = true;
            rb.isKinematic = false;
            rb.AddForce(Vector3.forward, ForceMode.Impulse);
            StartCoroutine(DestroyStone());
            isBreaking = true;
        }
	}

    IEnumerator DestroyStone()
    {
        yield return new WaitForSeconds(2);

        int minVal = 1;
        int maxVal = 5;

        int rand = Random.Range(minVal, maxVal);

        // Giving random amount of stones, ore the given ores (between the amounts)
        for (int i = 0; i < rand; i++)
        {
            Instantiate(stone, thisStone.transform.position, thisStone.transform.rotation);
        }
        //Destroy(thisStone);
        thisStone.SetActive(false);
    }

    // Stone breaking effect
    void PlayStoneBreakSound()
    {
        audio.clip = sfx;
        audio.PlayOneShot(audio.clip);
    }
}