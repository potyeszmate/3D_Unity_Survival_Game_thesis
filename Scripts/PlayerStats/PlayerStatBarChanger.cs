using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;



public class PlayerStatBarChanger : MonoBehaviour
{
    private PlayerStat playerStat;
    private HealthScript healthScript;

    public GameObject health1;
    public GameObject health2;
    public GameObject health3;
    public GameObject health4;

    public GameObject hunger1;
    public GameObject hunger2;
    public GameObject hunger3;
    public GameObject hunger4;
    public GameObject hunger5;
    public GameObject hunger6;
    public GameObject hunger7;


    public GameObject thirst1;
    public GameObject thirst2;
    public GameObject thirst3;
    public GameObject thirst4;
    public GameObject thirst5;
    public GameObject thirst6;    
    public GameObject thirst7;    

    public GameObject energy1;
    public GameObject energy2;
    public GameObject energy3;
    public GameObject energy4;
    public GameObject energy5;  

    void Start()
    {          
        playerStat = GameObject.Find("Player").GetComponent<PlayerStat>();
        healthScript = GameObject.Find("Player").GetComponent<HealthScript>();
 
    }

    //The code in my project I'm not proud of but it works. Checks if the 3 stat is between certain values and sets it to the certain image
    // Could have been done with switch as well of course

    void Update()
    {       

        if(playerStat.thirst >= 15.0)
        {
            thirst1.SetActive(true);
            thirst2.SetActive(false);
            thirst3.SetActive(false);
            thirst4.SetActive(false);
            thirst5.SetActive(false);
            thirst6.SetActive(false);
            thirst7.SetActive(false);
        }
        else if(playerStat.thirst >= 12.0 && playerStat.thirst <= 15.0)
        {   
            Debug.Log("2.");
            thirst1.SetActive(false);
            thirst2.SetActive(true);
            thirst3.SetActive(false);
            thirst4.SetActive(false);
            thirst5.SetActive(false);
            thirst6.SetActive(false);
            thirst7.SetActive(false);
        }        
        else if(playerStat.thirst >= 10.0 && playerStat.thirst <= 12.0)
        {
            Debug.Log("3.");
            thirst1.SetActive(false);
            thirst2.SetActive(false);
            thirst3.SetActive(true);
            thirst4.SetActive(false);
            thirst5.SetActive(false);
            thirst6.SetActive(false);
            thirst7.SetActive(false);
        }
        else if(playerStat.thirst >= 8.0 && playerStat.thirst <= 10.0)
        {
            thirst1.SetActive(false);
            thirst2.SetActive(false);
            thirst3.SetActive(false);
            thirst4.SetActive(true);
            thirst5.SetActive(false);
            thirst6.SetActive(false);
            thirst7.SetActive(false);
        }
        else if(playerStat.thirst >= 6.0 && playerStat.thirst <= 8.0)
        {
            thirst1.SetActive(false);
            thirst2.SetActive(false);
            thirst3.SetActive(false);
            thirst4.SetActive(false);
            thirst5.SetActive(true);
            thirst6.SetActive(false);
            thirst7.SetActive(false);
        }
        else if(playerStat.thirst >= 4.0 && playerStat.thirst <= 6.0)
        {
            thirst1.SetActive(false);
            thirst2.SetActive(false);
            thirst3.SetActive(false);
            thirst4.SetActive(false);
            thirst5.SetActive(false);
            thirst6.SetActive(true);
            thirst7.SetActive(false);
        }
        else if(playerStat.thirst >= 0.0 && playerStat.thirst <= 4.0)
        {
            thirst1.SetActive(false);
            thirst2.SetActive(false);
            thirst3.SetActive(false);
            thirst4.SetActive(false);
            thirst5.SetActive(false);
            thirst6.SetActive(false);
            thirst7.SetActive(true);
        }

//////////////////////////////////////////////////////////

        if(playerStat.hunger >= 15.0)
        {
            hunger1.SetActive(true);
            hunger2.SetActive(false);
            hunger3.SetActive(false);
            hunger4.SetActive(false);
            hunger5.SetActive(false);
            hunger6.SetActive(false);
            hunger7.SetActive(false);
        }
        else if(playerStat.hunger >= 12.0 && playerStat.hunger <= 15.0)
        {   
            hunger1.SetActive(false);
            hunger2.SetActive(true);
            hunger3.SetActive(false);
            hunger4.SetActive(false);
            hunger5.SetActive(false);
            hunger6.SetActive(false);
            hunger7.SetActive(false);
        }        
        else if(playerStat.hunger >= 10.0 && playerStat.hunger <= 12.0)
        {
            hunger1.SetActive(false);
            hunger2.SetActive(false);
            hunger3.SetActive(true);
            hunger4.SetActive(false);
            hunger5.SetActive(false);
            hunger6.SetActive(false);
            hunger7.SetActive(false);
        }
        else if(playerStat.hunger >= 8.0 && playerStat.hunger <= 10.0)
        {
            hunger1.SetActive(false);
            hunger2.SetActive(false);
            hunger3.SetActive(false);
            hunger4.SetActive(true);
            hunger5.SetActive(false);
            hunger6.SetActive(false);
            hunger7.SetActive(false);
        }
        else if(playerStat.hunger >= 6.0 && playerStat.hunger <= 8.0)
        {
            hunger1.SetActive(false);
            hunger2.SetActive(false);
            hunger3.SetActive(false);
            hunger4.SetActive(false);
            hunger5.SetActive(true);
            hunger6.SetActive(false);
            hunger7.SetActive(false);
        }
        else if(playerStat.hunger >= 4.0 && playerStat.hunger <= 6.0)
        {
            hunger1.SetActive(false);
            hunger2.SetActive(false);
            hunger3.SetActive(false);
            hunger4.SetActive(false);
            hunger5.SetActive(false);
            hunger6.SetActive(true);
            hunger7.SetActive(false);
        }
        else if(playerStat.hunger >= 0.0 && playerStat.hunger <= 4.0)
        {
            hunger1.SetActive(false);
            hunger2.SetActive(false);
            hunger3.SetActive(false);
            hunger4.SetActive(false);
            hunger5.SetActive(false);
            hunger6.SetActive(false);
            hunger7.SetActive(true);
        }

//////////////////////////////////////////////////////////

        if(healthScript.health >= 73.0 && healthScript.health <= 100.0)
        {
            health1.SetActive(true);
            health2.SetActive(false);
            health3.SetActive(false);
            health4.SetActive(false);
        }
        else if(healthScript.health >= 42.0 && healthScript.health <= 72.0)
        {
            health1.SetActive(false);
            health2.SetActive(true);
            health3.SetActive(false);
            health4.SetActive(false);
        }
        else if(healthScript.health >= 21.0 && healthScript.health <= 41.0)
        {
            health1.SetActive(false);
            health2.SetActive(false);
            health3.SetActive(true);
            health4.SetActive(false);
        }
        else if(healthScript.health >= 0.0 && healthScript.health <= 20.0)
        {
            health1.SetActive(false);
            health2.SetActive(false);
            health3.SetActive(false);
            health4.SetActive(true);
        }

//////////////////////////////////////////////////////////

        if(playerStat.energy >= 8.0 && playerStat.energy <= 11.0)
        {
            energy1.SetActive(true);
            energy3.SetActive(false);
            energy3.SetActive(false);
            energy4.SetActive(false);
            energy5.SetActive(false);

        }
        else if(playerStat.energy >= 6.0 && playerStat.energy < 8.0)
        {
            energy1.SetActive(false);
            energy2.SetActive(true);
            energy3.SetActive(false);
            energy4.SetActive(false);
            energy5.SetActive(false);

        }
        else if(playerStat.energy >= 5.0 && playerStat.energy < 8.0)
        {
            energy1.SetActive(false);
            energy2.SetActive(false);
            energy3.SetActive(true);
            energy4.SetActive(false);
            energy5.SetActive(false);

        }
        else if(playerStat.energy >= 3.0 && playerStat.energy < 5.0)
        {
            energy1.SetActive(false);
            energy2.SetActive(false);
            energy3.SetActive(false);
            energy4.SetActive(true);
            energy5.SetActive(false);

        }

        else if(playerStat.energy >= 0.0 && playerStat.energy < 3.0)
        {
            energy1.SetActive(false);
            energy2.SetActive(false);
            energy3.SetActive(false);
            energy4.SetActive(false);
            energy5.SetActive(true);

        }

    }



 
}
