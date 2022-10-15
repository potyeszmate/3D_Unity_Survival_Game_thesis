using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class DeathManager : MonoBehaviour
{
    public Button newBtn, exitBtn;
    

    private void Start()
    {
        GameManager manager = FindObjectOfType<GameManager>();

        Debug.Log("Death scene");
        Time.timeScale = 1;
        // Dont lock the cursor
        Cursor.lockState = CursorLockMode.Confined;
        // Set the cursor visible
        Cursor.visible = true;
        // On click we play a new game
        //newBtn.onClick.AddListener(NewGame);

        //newBtn.onClick.AddListener(manager.StartNextScene);
        newBtn.onClick.AddListener(NewGame);

        // On click we exit to main menu
        //exitBtn.onClick.AddListener(manager.ExitToMainMenuDeathMenu);
        exitBtn.onClick.AddListener(Exit);

        
        
    }

    // Loads a new game scene
    internal void NewGame()
    {
        //manager.LoadnewGame();
        PlayerPrefs.SetInt("LoadSavedData", 0);
        SceneManager.LoadScene(1);
        

        //PlayerPrefs.SetInt("LoadSavedData", 0);
        Time.timeScale = 1;
    }

    // Exits to the main menu
    internal void Exit()
    {
        PlayerPrefs.SetInt("LoadSavedData", 0);
        SceneManager.LoadScene(2);
        
        //SceneManager.LoadScene(2);
        Time.timeScale = 1;
    }

   
}
