using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{

    public string MainScene;

    public void PlayGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(MainScene);
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
