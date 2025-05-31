using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string gauntlet;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NewGame()
    {
        SceneManager.LoadScene(gauntlet);
    }

    public void Continue()
    {
        SceneManager.LoadScene(gauntlet);
    }

    //Quits the game if the quit button is pressed
    public void QuitGame()
    {
        Application.Quit();
    }
}
