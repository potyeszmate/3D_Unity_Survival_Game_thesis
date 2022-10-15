using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(AudioSource))]
public class BoxProperties : MonoBehaviour {

    public int durability = 8;
    // A fa amit vágunk
    GameObject thisBox;

    private Rigidbody rb;

    private new AudioSource audio;
    public AudioClip boxSfx;
    private bool isBreaking = false;
    // A kő amit kapunk
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
        thisBox = transform.gameObject;
        rb = thisBox.GetComponent<Rigidbody>();
        rb.mass = 100;
        rb.isKinematic = true;
        rb.useGravity = false;;
	}
	
	// Update is called once per frame
	void Update ()
    {
        // if the box's health is 0, than we have to break it (turning off the rigit body kinematic and destroy the object)
        if (durability <= 0 & isBreaking == false)
        {
            PlayBoxBreakSound();
            rb.useGravity = true;
            rb.isKinematic = false;
            rb.AddForce(Vector3.forward, ForceMode.Impulse);
            // Using the same methods as for the barell
            StartCoroutine(DestroyBarell());
            isBreaking = true;
        }
	}

    // Does the same thing as the barell
    IEnumerator DestroyBarell()
    {
        yield return new WaitForSeconds(0.50f);

        int minVal = 1;
        int maxVal = 4;
        

        int rand = Random.Range(minVal, maxVal);

        for (int i = 0; i < rand; i++)
        {
            int minVal1 = 1;
            int maxVal2 = 101;

            int rand2 = Random.Range(minVal1, maxVal2);

            if( rand2 >= 1 && rand2 <= 30 )
                Instantiate(apple, thisBox.transform.position, thisBox.transform.rotation);
                
            else if( rand2 >= 31 && rand2 <= 41)
                Instantiate(waterBottle, thisBox.transform.position, thisBox.transform.rotation);

            else if( rand2 >= 42 && rand2 <= 52 )
                Instantiate(pill, thisBox.transform.position, thisBox.transform.rotation);

            else if( rand2 >= 53 && rand2 <= 60 )
                Instantiate(cannedFood, thisBox.transform.position, thisBox.transform.rotation);

            else if( rand2 >= 61 && rand2 <= 75 )
                Instantiate(rope, thisBox.transform.position, thisBox.transform.rotation);

            else if( rand2 >= 75 && rand2 <= 83 )
                Instantiate(tape, thisBox.transform.position, thisBox.transform.rotation);

            else if( rand2 >= 84 && rand2 <= 90 )
                Instantiate(matchBox, thisBox.transform.position, thisBox.transform.rotation);

            else if( rand2 >= 91 && rand2 <= 100 )
                Instantiate(battery, thisBox.transform.position, thisBox.transform.rotation);
                
        }

        thisBox.SetActive(false);

    }

    void PlayBoxBreakSound()
    {
        audio.clip = boxSfx;
        audio.PlayOneShot(audio.clip);
    }
}