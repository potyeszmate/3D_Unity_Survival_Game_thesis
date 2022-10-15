using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {

    public static EnemyManager instance;

    [SerializeField]
    private GameObject cannibal_Prefab, bear_Prefab, boar_Prefab, wolf_Prefab, maleDeer_Prefab, femaleDeer_Prefab, chicken_Prefab, bunny_Prefab;

    public Transform[] cannibal_SpawnPoints, bear_SpawnPoints,  boar_SpawnPoints,  wolf_SpawnPoints, maleDeer_SpawnPoints, femaleDeer_SpawnPoints, chicken_SpawnPoints, bunny_SpawnPoints;

    [SerializeField]
    private int cannibal_Enemy_Count, bear_Enemy_Count, boar_Enemy_Count, wolf_Enemy_Count, maleDeer_Enemy_Count, femaleDeer_Enemy_Count, chicken_Enemy_Count, bunny_Enemy_Count;

    private int initial_Cannibal_Count, initial_Bear_Count, initial_Boar_Count, initial_Wolf_Count,  initial_MaleDeer_Count, initial_FemaleDeer_Count, initial_Chicken_Count, initial_Bunny_Count;

    public float wait_Before_Spawn_Enemies_Time = 10f;


    // Initialization
    void Awake () 
    {
        MakeInstance();
	}

    // In the start it spawns the enemies
    void Start() {
        initial_Cannibal_Count = cannibal_Enemy_Count;
        initial_Bear_Count = bear_Enemy_Count;
        initial_Boar_Count = boar_Enemy_Count;
        initial_Wolf_Count = wolf_Enemy_Count;
        initial_MaleDeer_Count = maleDeer_Enemy_Count;
        initial_FemaleDeer_Count = femaleDeer_Enemy_Count;
        initial_Chicken_Count = chicken_Enemy_Count;
        initial_Bunny_Count = bunny_Enemy_Count;
        

        SpawnEnemies();

        StartCoroutine("CheckToSpawnEnemies");
    }

    void MakeInstance() 
    {
        if(instance == null) 
        {
            instance = this;
        }
    }

    // Spawning the enemies
    void SpawnEnemies() 
    {
        SpawnCannibals();
        SpawnBears();
        SpawnBoars();
        SpawnWolfes();
        SpawnMaleDeer();
        SpawnFemaleDeer();
        SpawnChicken();
        SpawnBunny();
        
        
    }

    //Spawning canibals
    void SpawnCannibals() 
    {

        int index = 0;

        // Instantiating the cannibals (checking the numbers of the canibals if we need to spawn it)
        for (int i = 0; i < cannibal_Enemy_Count; i++) 
        {

            if (index >= cannibal_SpawnPoints.Length) 
            {
                index = 0;
            }

            Instantiate(cannibal_Prefab, cannibal_SpawnPoints[index].position, Quaternion.identity);

            index++;

        }

        cannibal_Enemy_Count = 0;

    }

    // Instantiating the boars (checking the numbers of the boars if we need to spawn it)
    void SpawnBoars() 
    {

        int index = 0;

        for (int i = 0; i < boar_Enemy_Count; i++) {

            if (index >= boar_SpawnPoints.Length)
            {
                index = 0;
            }

            Instantiate(boar_Prefab, boar_SpawnPoints[index].position, Quaternion.identity);

            index++;

        }

        boar_Enemy_Count = 0;

    }

    // Instantiating the bears (checking the numbers of the bears if we need to spawn it)
    void SpawnBears() 
    {

        int index = 0;

        for (int i = 0; i < bear_Enemy_Count; i++) {

            if (index >= bear_SpawnPoints.Length)
            {
                index = 0;
            }

            Instantiate(bear_Prefab, bear_SpawnPoints[index].position, Quaternion.identity);

            index++;

        }

        bear_Enemy_Count = 0;

    }

    // Instantiating the wolfes (checking the numbers of the wolfes if we need to spawn it)
    void SpawnWolfes() 
    {

        int index = 0;

        for (int i = 0; i < wolf_Enemy_Count; i++) {

            if (index >= wolf_SpawnPoints.Length)
            {
                index = 0;
            }

            Instantiate(wolf_Prefab, wolf_SpawnPoints[index].position, Quaternion.identity);

            index++;

        }

        wolf_Enemy_Count = 0;

    }

    // Instantiating the Male Deer (checking the numbers of the wolfes if we need to spawn it)
    void SpawnMaleDeer() 
    {

        int index = 0;

        for (int i = 0; i < maleDeer_Enemy_Count; i++) {

            if (index >= maleDeer_SpawnPoints.Length)
            {
                index = 0;
            }

            Instantiate(maleDeer_Prefab, maleDeer_SpawnPoints[index].position, Quaternion.identity);

            index++;

        }

        maleDeer_Enemy_Count = 0;

    }

    // Instantiating the Female Deer (checking the numbers of the wolfes if we need to spawn it)
    void SpawnFemaleDeer() 
    {

        int index = 0;

        for (int i = 0; i < femaleDeer_Enemy_Count; i++) {

            if (index >= femaleDeer_SpawnPoints.Length)
            {
                index = 0;
            }

            Instantiate(femaleDeer_Prefab, femaleDeer_SpawnPoints[index].position, Quaternion.identity);

            index++;

        }

        femaleDeer_Enemy_Count = 0;

    }

    // Instantiating the Chicken (checking the numbers of the wolfes if we need to spawn it)
    void SpawnChicken() 
    {

        int index = 0;

        for (int i = 0; i < chicken_Enemy_Count; i++) {

            if (index >= chicken_SpawnPoints.Length)
            {
                index = 0;
            }

            Instantiate(chicken_Prefab, chicken_SpawnPoints[index].position, Quaternion.identity);

            index++;

        }

        chicken_Enemy_Count = 0;

    }

    // Instantiating the Bunny (checking the numbers of the wolfes if we need to spawn it)
    void SpawnBunny() 
    {

        int index = 0;

        for (int i = 0; i < bunny_Enemy_Count; i++) {

            if (index >= bunny_SpawnPoints.Length)
            {
                index = 0;
            }

            Instantiate(bunny_Prefab, bunny_SpawnPoints[index].position, Quaternion.identity);

            index++;

        }

        bunny_Enemy_Count = 0;

    }

    // Checking the enemies to spawn
    IEnumerator CheckToSpawnEnemies() {
        yield return new WaitForSeconds(wait_Before_Spawn_Enemies_Time);

        SpawnCannibals();
        SpawnBears();
        SpawnBoars();
        SpawnWolfes();
        SpawnMaleDeer();
        SpawnFemaleDeer();
        SpawnChicken();
        SpawnBunny();

        StartCoroutine("CheckToSpawnEnemies");

    }

    // If the enemy dies we need to add an enemy to spawn
    public void EnemyDied(string enemy) 
    {

        // Canibal check
        if(enemy == "Canibal") 
        {

            cannibal_Enemy_Count++;
            
            if(cannibal_Enemy_Count > initial_Cannibal_Count) 
            {
                cannibal_Enemy_Count = initial_Cannibal_Count;
            }

        }
        // Bear check
        else if(enemy == "Bear") 
        {

            bear_Enemy_Count++;

            if(bear_Enemy_Count > initial_Bear_Count) {
                bear_Enemy_Count = initial_Bear_Count;
            }

        }
        // Boar check
        else if(enemy == "Boar") 
        {

            boar_Enemy_Count++;

            if(boar_Enemy_Count > initial_Boar_Count) {
                boar_Enemy_Count = initial_Boar_Count;
            }

        }
        // Wolf check
        else if(enemy == "Wolf") 
        {

            wolf_Enemy_Count++;

            if(wolf_Enemy_Count > initial_Wolf_Count) {
                wolf_Enemy_Count = initial_Wolf_Count;
            }
        }
        // MaleDeer check
        else if(enemy == "MaleDeer") 
        {

            maleDeer_Enemy_Count++;

            if(maleDeer_Enemy_Count > initial_MaleDeer_Count) {
                maleDeer_Enemy_Count = initial_MaleDeer_Count;
            }
        }
        // FemaleDeer check
        else if(enemy == "FemaleDeer") 
        {

            femaleDeer_Enemy_Count++;

            if(femaleDeer_Enemy_Count > initial_FemaleDeer_Count) {
                femaleDeer_Enemy_Count = initial_FemaleDeer_Count;
            }
        }
        // Chicken check
        else if(enemy == "Chicken") 
        {

            chicken_Enemy_Count++;

            if(chicken_Enemy_Count > initial_Chicken_Count) {
                chicken_Enemy_Count = initial_Chicken_Count;
            }
        }
        // Bunny check
        else if(enemy == "Bunny") 
        {

            bunny_Enemy_Count++;

            if(bunny_Enemy_Count > initial_Bunny_Count) {
                bunny_Enemy_Count = initial_Bunny_Count;
            }
        }


    }

    // Stops the spawning of an enemy type
    public void StopSpawning() 
    {
        StopCoroutine("CheckToSpawnEnemies");
    }

} // class
