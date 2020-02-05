using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuControl : MonoBehaviour
{
    public GameObject InGameMenu,PauseMenu;

    void Start()
    {
        Screen.SetResolution(1280, 720, false);
    }
    public void OnSetButton()
    {
        InGameMenu.SetActive(false);
        PauseMenu.SetActive(true);
        Time.timeScale = 0;
    }
    public void OnRestart()
    {
        SceneManager.LoadScene("Jump A Jump");
        Time.timeScale = 1;
    }

    public void OnContinue()
    {
        InGameMenu.SetActive(true);
        PauseMenu.SetActive(false);
        Time.timeScale = 1;
    }
    public void OnMainMenu()
    {
        SceneManager.LoadScene("Start");
    }
    

}
