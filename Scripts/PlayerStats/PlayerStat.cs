using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;



public class PlayerStat : MonoBehaviour
{
    
    protected AgentMovement AgentMovement;

    public float health = 100;
    public float stamina = 10;
    public float hunger = 5;
    public float thirst = 5;
    public float energy = 10;


    private float healthNormal;
    private float staminaNormal;
    private float hungerNormal;
    private float thirstNormal;
    private float energyNormal;

    // Statok csökkentése
    private float staminaFallRate = 1.8f;
    private float hungerFallRate = 0.1f;
    private float thirstFallRate = 0.1f;
    private float energyFallRate = 0.01f;


    public Text healthText;
    
    public Text hungerText;
    public Text thirstText;
    public Text energyText;

    public Image staminaProgressUI = null;
    public CanvasGroup sliderCanvasGroup = null;
    
    public new AudioSource audio;
    public AudioClip playerTired;


    
    // Start is called before the first frame update
    void Start()
    {  
        
        // Setting Max values we can reach
        healthNormal = health;
        staminaNormal = stamina;
        hungerNormal = hunger;
        thirstNormal = thirst;
        energyNormal = energy;
        
        AgentMovement = GetComponent<AgentMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKey(KeyCode.LeftShift) && (Input.GetKey(KeyCode.W) || (Input.GetKey(KeyCode.S) || (Input.GetKey(KeyCode.A) || (Input.GetKey(KeyCode.D))))))
        {
            stamina -= Time.deltaTime * staminaFallRate;
            hunger -= Time.deltaTime * hungerFallRate * 1.3f;
            thirst -= Time.deltaTime * thirstFallRate * 1.3f;
            energy -= Time.deltaTime * energyFallRate * 1.2f;
            
            // When we run, the stamina goes down
            staminaProgressUI.fillAmount -= staminaFallRate / 10 * Time.deltaTime;
            

        }
        else
        {
            // Giving back the stamine to the player if he isnt running
            if (stamina <= staminaNormal)
            {
                stamina += Time.deltaTime * (staminaFallRate / 1.1f);
                UpdateStamina(1);
            }
            
        }

        // Stamina should not fall below 0 
        if (stamina < 0)
        {
            AgentMovement.canPlayerRun = false;
            stamina = 0;
            sliderCanvasGroup.alpha = 1;
        }
        else
        {
            AgentMovement.canPlayerRun = true;
            
        }

        // Do not go further than Max
        if (stamina >= staminaNormal)
        {
            stamina = staminaNormal;
            sliderCanvasGroup.alpha = 1;
        }

        // Play audio if we dont have stamina (tired sound)
        if(stamina == 0)
        {
            StartCoroutine(PlayTired());

        }
            
        // Same as above, dont need more then 0 stats, and  less then 0 stat.

        if (hunger > 0)
            hunger -= Time.deltaTime * hungerFallRate;

        if (hunger >= hungerNormal)
            hunger = hungerNormal;

        if (hunger <= 0)
            hunger = 0;

        if (thirst > 0)
            thirst -= Time.deltaTime * thirstFallRate;

        if (thirst >= thirstNormal)
            thirst = thirstNormal;

        if (thirst <= 0)
            thirst = 0;

        if (energy > 0)
            energy -= Time.deltaTime * energyFallRate;

        if (energy >= energyNormal)
            energy = energyNormal;

        if (energy <= 0)
            energy = 0;

        hungerText.text = Mathf.FloorToInt(hunger).ToString();
        thirstText.text = Mathf.FloorToInt(thirst).ToString();
        energyText.text = Mathf.FloorToInt(energy).ToString();

    
    } 

    // The method updates the stamine and reduces it by a given value (if we running) 
    void UpdateStamina(int value)
    {
        staminaProgressUI.fillAmount = stamina / staminaNormal;
        sliderCanvasGroup.alpha = 1;
        
    }

    // Displaying our Health to the UI
    public void Display_HealthStats(float healthValue) {

        healthText.text = Mathf.FloorToInt(healthValue).ToString();

    }

    // The PlayTired audio we need to play
    IEnumerator PlayTired()
    {
        yield return new WaitForSeconds(0.5f);
        audio.clip = playerTired;
        audio.Play();
    }

    
}
