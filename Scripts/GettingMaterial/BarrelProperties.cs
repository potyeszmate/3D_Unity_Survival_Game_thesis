using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelProperties : MonoBehaviour {

    public int durability = 8;
    GameObject thisBarrel;

    private Rigidbody rb;

    private new AudioSource audio;
    public AudioClip sfx;
    private bool isBreaking = false;
    public Transform apple;
    public Transform waterBottle;
    public Transform pill;
    public Transform cannedFood;
    public Transform battery;
    public Transform rope;
    public Transform tape;
    public Transform matchBox;

    // Use this for initialization
    void Start ()
    {
        audio = transform.GetComponent<AudioSource>();
        thisBarrel = transform.gameObject;
        rb = thisBarrel.GetComponent<Rigidbody>();
        rb.mass = 100;
        rb.isKinematic = true;
        rb.useGravity = false;;
	}
	
	// Update is called once per frame
	void Update ()
    {
        // Destroying the barell when it's health is smaller than 0
        if (durability <= 0 & isBreaking == false)
        {
            PlaybarellBreakSound();
            //turning off the rigidbody's kinematic and turning off the gravity
            rb.useGravity = true;
            rb.isKinematic = false;
            rb.AddForce(Vector3.forward, ForceMode.Impulse);
            StartCoroutine(DestroyBarell());
            isBreaking = true;
        }
	}

    IEnumerator DestroyBarell()
    {
        yield return new WaitForSeconds(0.50f);

        int minVal = 1;
        int maxVal = 4;
        

        int rand = Random.Range(minVal, maxVal);

        // Giving random item counts (between 1 to 4)
        for (int i = 0; i < rand; i++)
        {
            int minVal1 = 1;
            int maxVal2 = 101;

            int rand2 = Random.Range(minVal1, maxVal2);

            // Giving random items in certain percentages

            if( rand2 >= 1 && rand2 <= 30 )
                Instantiate(apple, thisBarrel.transform.position, thisBarrel.transform.rotation);
                
            else if( rand2 >= 31 && rand2 <= 41)
                Instantiate(waterBottle, thisBarrel.transform.position, thisBarrel.transform.rotation);

            else if( rand2 >= 42 && rand2 <= 52 )
                Instantiate(pill, thisBarrel.transform.position, thisBarrel.transform.rotation);

            else if( rand2 >= 53 && rand2 <= 60 )
                Instantiate(cannedFood, thisBarrel.transform.position, thisBarrel.transform.rotation);

            else if( rand2 >= 61 && rand2 <= 75 )
                Instantiate(rope, thisBarrel.transform.position, thisBarrel.transform.rotation);

            else if( rand2 >= 75 && rand2 <= 83 )
                Instantiate(tape, thisBarrel.transform.position, thisBarrel.transform.rotation);

            else if( rand2 >= 84 && rand2 <= 90 )
                Instantiate(matchBox, thisBarrel.transform.position, thisBarrel.transform.rotation);

            else if( rand2 >= 91 && rand2 <= 100 )
                Instantiate(battery, thisBarrel.transform.position, thisBarrel.transform.rotation);
                
        }
        
        // Inactivate the barell
        thisBarrel.SetActive(false);

    }

    // Play the barell breaking audio
    void PlaybarellBreakSound()
    {
        audio.clip = sfx;
        audio.PlayOneShot(audio.clip);
    }
}