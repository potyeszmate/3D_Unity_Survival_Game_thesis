using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiInGameMenu : MonoBehaviour
{
    public Button saveBtn, exitBtn,exitBtn2;
    public GameObject gameMenuPanel, loadingPanel;

    public bool MenuVisible { get => gameMenuPanel.activeSelf; }

    private void Start()
    {
        gameMenuPanel.SetActive(false);
        GameManager manager = FindObjectOfType<GameManager>();
        saveBtn.onClick.AddListener(manager.SaveGame);
        exitBtn.onClick.AddListener(manager.ExitToMainMenu);
        
        
    }

    // Toggles the pause menu (it its active then set to inactive, if it inactive it sets to active)
    public void ToggleMenu()
    {
        gameMenuPanel.SetActive(!gameMenuPanel.activeSelf);
         
    }

    // Toggles the loading meu
    public void ToggleLoadingPanel()
    {
        loadingPanel.SetActive(!loadingPanel.activeSelf);
    }
   

}
