using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class UiMainMenu : MonoBehaviour
{
    public Button newGameBtn, resumBtn, exitBtn;
    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetInt("LoadSavedData", 0);

        GameManager manager = FindObjectOfType<GameManager>();
        //On click we start a new game
        //newGameBtn.onClick.AddListener(manager.StartNextScene);
        newGameBtn.onClick.AddListener(NewGame);

        //On click we continue the saved game
        //resumBtn.onClick.AddListener(manager.LoadSavedGame);
        resumBtn.onClick.AddListener(ContinueGame);
        // If we dont have any saved file, we cant press continue
        resumBtn.interactable = false;

        if (manager.CheckSavedGameExists())
        {
            resumBtn.interactable = true;
        }
    }

    // The Exit quits the whole application
    public void Exit() 
    {
        Application.Quit();
        Debug.Log("Quit");
    }

    internal void NewGame()
    {
        //manager.LoadnewGame();
        SceneManager.LoadScene(1);
        //PlayerPrefs.SetInt("LoadSavedData", 0);
        Time.timeScale = 1;
    }

    // Exits to the main menu
    internal void ContinueGame()
    {
        SceneManager.LoadScene(1);
        PlayerPrefs.SetInt("LoadSavedData", 1);
        //SceneManager.LoadScene(2);
        Time.timeScale = 1;
    }



    
}
