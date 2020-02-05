using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Mainmenu : MonoBehaviour
{
    public GameObject MapChoose,MainMenu;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OntoJump()
    {
        Global.loadName = "Jump A Jump";
        SceneManager.LoadScene("Loading");
        Time.timeScale = 1;
    }
    public void OnMapChoose()
    {
        MainMenu.SetActive(false);
        MapChoose.SetActive(true);
    }
    public void OnMapChooseReturn()
    {
        MainMenu.SetActive(true);
        MapChoose.SetActive(false);
    }
    public void OnLoadMap1()
    {
        Global.loadName = "iceworld";
        SceneManager.LoadScene("Loading");
        Time.timeScale = 1;
    }
    public void OnApplicationQuit()
    {
        Application.Quit();
    }
}
