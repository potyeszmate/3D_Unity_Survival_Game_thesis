//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    //Important part of the project

    public AgentController playerController;
    private PlayerStat playerStat;

    public AudioSource audioSource;
    public AudioClip[] eatfx;
    public AudioClip[] drinkfx;

    void Start()
    {
    //playerStat = GetComponent<PlayerStat>();
    playerStat = GameObject.Find("Player").GetComponent<PlayerStat>();  
    }

    // Use the certain type of item 
    public bool UseItem(ItemSO itemData)
    {
        var itemType = itemData.GetItemType();
        switch (itemType)
        {
            case ItemType.None:
                throw new System.Exception("Item can't have itemtype of NONO");
            //If is a food type then add the bonuses that the certain food gives to the player stat
            case ItemType.Food:
                FoodItemSO foodData = (FoodItemSO)itemData;
                playerStat.hunger += foodData.hungerBonus;
                playerStat.thirst += foodData.thirstBonus;
                playerStat.health += foodData.healthBoost;
                PlayEatSound();
                return true;
            //If is a water type then add the bonuses that the certain drink gives to the player stat
            case ItemType.Water:
                WaterItemSO waterData = (WaterItemSO)itemData;
                playerStat.hunger += waterData.hungerBonus;
                playerStat.thirst += waterData.thirstBonus;
                playerStat.health += waterData.healthBoost;
                playerStat.energy += waterData.energyBonus;
                PlayDrinkSound();
                return true;
            // Here I just show that we have the weapon, but right now I dont equip the weapon(or tool) here
            case ItemType.Weapon:
                //WeaponItemSO weapon = (WeaponItemSO)itemData;
                //Debug.Log("Equiping weapon");
                return false;
            default:
                Debug.Log("Cant use an item that is a type: " + itemType.ToString());
                break;
        }
        return false;
    }

    internal bool EquipItem(ItemSO itemData)
    {   
        
        var itemType = itemData.GetItemType();
        switch (itemType)
        {
            case ItemType.None:
                //throw new Exception("NO item");
            //Here I set the Item types to be equipable ( weapons, which are tools and weapons as well)
            case ItemType.Weapon:
                return true;
            default:
                break;
        }
        return false;
    }

    // Playing the eating sound when use Food item (used animation tab)
    private void PlayEatSound()
    {
        audioSource.clip = eatfx[Random.Range(0, eatfx.Length)];
        
        audioSource.PlayOneShot(audioSource.clip);  
        
    }

    // Play drinking sound when use water type (used in animation tab)
    private void PlayDrinkSound()
    {
        audioSource.clip = drinkfx[Random.Range(0, eatfx.Length)];
        audioSource.PlayOneShot(audioSource.clip);  
       
    }

}
