using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChopTree : MonoBehaviour
{
    public int minDamage = 1;
    public int maxDamage = 3;

    [SerializeField]
    private AudioSource audioSource;
    public AudioClip[] chopSfx;
    public AudioClip barrelSfx;
    public AudioClip boxSfx;

    public ParticleSystem hitTreeVfx;
    public GameObject helper;

	public float timer = 0;

    void Start()
    {
        audioSource = transform.parent.GetComponent<AudioSource>();
        hitTreeVfx.Stop();

    }

    // Update is called once per frame
    void Update () 
    {
        // Interact after left click
        if( Input.GetKeyDown(KeyCode.Mouse0))
        {
			if(timer <= 0)
            	GetInteract();
        }
		if (timer > 0)
			timer -= Time.deltaTime;
	}

    // Interact system to the Tree and Barell and box
    void GetInteract()
    {
        Collider hitc;
        // Tree interaction. Damaging the tree
        if(GetInteractOut(2f, out hitc))
        {
            if (hitc.tag == "Tree")
            {
                var damage = UnityEngine.Random.Range(minDamage, maxDamage);
                TreeProperties treeprop = hitc.gameObject.GetComponent<TreeProperties>();

                treeprop.durability -= damage;
				timer = 1.13f;
            }
        }

        // Barell interaction. Damaging the barell
        if(GetInteractOut(2f, out hitc))
        {
            //Debug.Log(hitc.tag);
            if (hitc.tag == "Barell")
            {
                var damage = UnityEngine.Random.Range(minDamage, maxDamage);
                BarrelProperties barrelprop = hitc.gameObject.GetComponent<BarrelProperties>();

                barrelprop.durability -= damage;
				timer = 1.20F;

            }
        }

        // Box interaction. Damaging the box
        if(GetInteractOut(2f, out hitc))
        {
            //Debug.Log(hitc.tag);
            if (hitc.tag == "Box")
            {
                var damage = UnityEngine.Random.Range(minDamage, maxDamage);
                BoxProperties boxprop = hitc.gameObject.GetComponent<BoxProperties>();

                boxprop.durability -= damage;
				timer = 1.20F;

            }
        }

        
        
    }

    // Checks the raycast distance
    private Collider GetInteract(float distance)
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, distance))
        {
            return hit.collider;
        }
        return null;
    }

    // Checks that something hits the raycast
    static bool GetInteractOut(float distance, out Collider hitcollider)
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, distance))
        {
            hitcollider = hit.collider;
            return true;
        }
        hitcollider = null;
        return false;
    }

    // Plays the chopsounds when we hit a tree - Using in animation tab
    private void PlayChopSound()
    {
        Collider hitc;
        
        if(GetInteractOut(2f, out hitc))
        {
            if (hitc.tag == "Tree")
            {
                audioSource.clip = chopSfx[Random.Range(0, chopSfx.Length)];
                audioSource.PlayOneShot(audioSource.clip); 
            }
        }
         
    }

    // Plays the barrelsound when we hit a barell - Using in animation tab
    private void PlayBarrelSound()
    {
        Collider hitc;
        
        if(GetInteractOut(2f, out hitc))
        {
            if (hitc.tag == "Barell")
            {
                audioSource.clip = barrelSfx;
                audioSource.PlayOneShot(audioSource.clip);
            }
        }
        
    }

    // Plays the boxsound when we hit a box - Using in animation tab
    private void PlayBoxSound()
    {
        Collider hitc;
        
        if(GetInteractOut(2f, out hitc))
        {
            if (hitc.tag == "Box")
            {
                audioSource.clip = boxSfx;
                audioSource.PlayOneShot(audioSource.clip);
            }
        }
        
    }

    // Turning on the VFX when we hit a tree or a box - Using in animation tab
    private void PlayHitTreeVfx()
    {
        Collider hitc;

        if(GetInteractOut(2f, out hitc))
        {
            if (hitc.tag == "Tree")
            {
                hitTreeVfx.Play();
            }
        }
        // Box
        if(GetInteractOut(2f, out hitc))
        {
            if (hitc.tag == "Box")
            {
                hitTreeVfx.Play();
                
            }
        }
        
    }

    // Turning off the VFX - Using in the animation tab
    private void SetOFVfx()
    {
        hitTreeVfx.Stop();
    }


}
