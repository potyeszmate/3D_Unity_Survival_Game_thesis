using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineStone_Ore : MonoBehaviour
{
   public int minDamage = 1;
    public int maxDamage = 3;

    private AudioSource audioSource;
    public AudioClip mineSfx;
    public AudioClip barrelSfx;

    public ParticleSystem hitStoneVfx;

	public float timer = 0;

    void Start()
    {
        audioSource = transform.parent.GetComponent<AudioSource>();  
        hitStoneVfx.Stop();  
    }

    // Update is called once per frame
    void Update () 
    {   
        // Interacticg with stone when left clicking
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
			if(timer <= 0)
            	GetInteract();
        }
		if (timer > 0)
			timer -= Time.deltaTime;
	}


    void GetInteract()
    {
        Collider hitc;  

        // Stone interaction. Damaging the stone
        if(GetInteractOut(2f, out hitc))
        {
            
            if (hitc.tag == "Stone")
            {

                var damage = UnityEngine.Random.Range(minDamage, maxDamage);
                StoneProperty stoneprop = hitc.gameObject.GetComponent<StoneProperty>();

                stoneprop.durability -= damage;
				timer = 1F;

            }
        }

        // Barell interaction. Damaging the barell
        if(GetInteractOut(2f, out hitc))
        {
            if (hitc.tag == "Barell")
            {

                var damage = UnityEngine.Random.Range(minDamage, maxDamage);
                BarrelProperties barrelprop = hitc.gameObject.GetComponent<BarrelProperties>();

                barrelprop.durability -= damage;
				timer = 1.20F;

            }
        }

        // Ore interaction. Damaging the Ore
        if(GetInteractOut(2f, out hitc))
        {
            
            if (hitc.tag == "Ore")
            {
                
                var damage = UnityEngine.Random.Range(minDamage, maxDamage);
                StoneProperty stoneprop = hitc.gameObject.GetComponent<StoneProperty>();

                stoneprop.durability -= damage;
				timer = 1F;

            }
        }

         // Other Ore interaction. Damaging the Ore
        if(GetInteractOut(2f, out hitc))
        {
            
            if (hitc.tag == "Gold_Silver_Ore")
            {

                var damage = UnityEngine.Random.Range(minDamage, maxDamage);
                StoneProperty stoneprop = hitc.gameObject.GetComponent<StoneProperty>();

                stoneprop.durability -= damage;
				timer = 1F;

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

    // Plays the stone sound when we hit a stone - Using in animation tab
    private void PlayMineSound()
    {
        Collider hitc;
        
        if(GetInteractOut(2f, out hitc))
        {
            if (hitc.tag == "Stone" || hitc.tag == "Ore" || hitc.tag == "Gold_Silver_Ore")
            {
                audioSource.clip = mineSfx;
                audioSource.PlayOneShot(audioSource.clip); 
            }
        }

    }

    // Plays the barellsound when we hit a barell - Using in animation tab
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

    // Turning on the VFX when we hit a stone - Using in animation tab
    private void PlayHitStoneVfx()
    {
        Collider hitc;
        
        if(GetInteractOut(2f, out hitc))
        {
            if (hitc.tag == "Stone" || hitc.tag == "Ore" || hitc.tag == "Gold_Silver_Ore")
            {
                
                hitStoneVfx.Play();
                
            }
        }
        
    }

    // Turning off the VFX - Using in the animation tab
    private void SetOFStoneVfx()
    {
        hitStoneVfx.Stop();
    }
}
