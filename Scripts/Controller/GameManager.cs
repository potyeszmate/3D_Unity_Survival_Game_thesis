using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public SaveSystem saveSystem;
    public string mainMenuSceneName;
    //public AgentController agentController;

    public UiInGameMenu gameMenu;
    //public UiMainMenu gameMainMenu;

     private bool pointerConfined = false;

    private bool timeAlreadyStopped = false;

    public Objects objects;

    //public Transform objectsTransform;

    public GameObject[] stones;

    // Start is called before the first frame update
    void Start()
    {
        objects = new Objects();
        // Loading the Saved files before before start of the game
        if (PlayerPrefs.GetInt("LoadSavedData") == 1)
        {
            // Turning on the Loading panel while loading
            Time.timeScale = 0;
            gameMenu.ToggleLoadingPanel();

            saveSystem.LoadSavedSunSpawnpointCoroutine();
            saveSystem.LoadSavedMoonSpawnpointCoroutine();
            saveSystem.LoadSavedTimeCoroutine();
            saveSystem.LoadSavedStatstCoroutine();
            saveSystem.LoadSavedSpawnpointCoroutine();
            StartCoroutine(saveSystem.LoadSavedDataCoroutine(DoneLoading));
            LoadGameObject();   
            
            PlayerPrefs.SetInt("LoadSavedData", 0);

        }
    }

   
    // Exiting to main menu scene
    public void ExitToMainMenu()
    {
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Time.timeScale = 1;


    }

    public void ExitToMainMenuDeathMenu()
    {
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        Time.timeScale = 1;


    }

    // Starting a new game scene
    internal void StartNewGame()
    {
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 2);
        Time.timeScale = 1;

    }

    // Opening hte next scene by index - New Game
    internal void StartNextScene()
    {
        Time.timeScale = 1;
        
        SceneManager.LoadScene(1); //  1

    }

    // Loading the saved game (continue game)  - Continue Game
    internal void LoadSavedGame()
    {
        PlayerPrefs.SetInt("LoadSavedData", 1);
        StartNextScene();  
    }



    // Check if saved files exist
    internal bool CheckSavedGameExists()
    {
        return saveSystem.CheckSavedDataExists();        
    }

    // After loading this method inactivates the loading panel and starts the game 
    public void DoneLoading()
    {
        gameMenu.ToggleLoadingPanel();
        Time.timeScale = 1;
   }

    // Saving the game objects
    internal void SaveGame()
    {
        saveSystem.SaveObjects();
        saveSystem.SaveSpawnPostition();
        saveSystem.SavePlayerStat();
        saveSystem.SaveTimeStat();
        saveSystem.SaveSunSpawnPostition();
        saveSystem.SaveMoonSpawnPostition();
        SaveGameObject();
    }

    // Toggle the menu panel in the game
    public void ToggleGameMenu()
    {
        if(gameMenu.MenuVisible == false)
        {
            
            // cursor unlocking and set the visibility
            if(Cursor.lockState == CursorLockMode.Confined)
            {
                pointerConfined = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true;
            }
            // Time stopped
            if(Time.timeScale == 0)
            {
                timeAlreadyStopped = true;
            }
            else
            {
                Time.timeScale = 0;
            }
        }
        else
        {
            // Time stopped
            if(timeAlreadyStopped)
            {
                timeAlreadyStopped = false;
            }
            else
            {
                Time.timeScale = 1;
            }
            if(pointerConfined == true)
            {
                pointerConfined = false;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }

        }

        gameMenu.ToggleMenu();
    }

    // Save the game objects activity ( if its active in the map or not)
    private void SaveGameObject()
    {
        //objects.objectPosition = objectsTransform.position;
        bool[] stonesActive = new bool[stones.Length];
        
        for (int i = 0; i < stones.Length; i++ )
        {
            stonesActive[i] = stones[i].activeInHierarchy;
        }
        objects.stonesActive = stonesActive;

        ObjectsSaverLoader.SaveGameObjectInMap(objects);
    }

    public bool CheckPositionsExists()
    {
      return System.IO.File.Exists(ObjectsSaverLoader.path);
    }

    // Load the game objects in the map (if inactive, then not)
    private void LoadGameObject()
    {
        
        if (CheckPositionsExists())
        {
            objects = ObjectsSaverLoader.LoadGameObjectInMap(objects.ObjectName);
            //objectsTransform.position = objects.objectPosition;
            for( int i = 0; i < stones.Length; i++)
            {
                stones[i].gameObject.SetActive(objects.stonesActive[i]);
            }
        }
    }


}
