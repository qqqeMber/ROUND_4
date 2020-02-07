using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuControl2 : MonoBehaviour
{
    public GameObject InGameMenu, PauseMenu;
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = (true);
        }
    }

    public void OnSetButton()
    {

        InGameMenu.SetActive(false);
        PauseMenu.SetActive(true);
        
    }
    public void OnRestart()
    {
        SceneManager.LoadScene("iceworld");
        Time.timeScale = 1;
    }

    public void OnContinue()
    {
        InGameMenu.SetActive(true);
        PauseMenu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = (false);
        Time.timeScale = 1;
    }
    public void OnMainMenu()
    {
        SceneManager.LoadScene("Start");
    }
}
