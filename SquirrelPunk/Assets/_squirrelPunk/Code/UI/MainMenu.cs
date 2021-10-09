using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject creditsBackButton;
    public GameObject playButton;
    public string MainScene;

    public void PlayGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    public void SetCreditsMenu()
    {
        EventSystem.current.SetSelectedGameObject(null);
        Invoke("SetCreditsCursor", .1f);

    }
    public void SetCreditsCursor()
    {
        EventSystem.current.SetSelectedGameObject(creditsBackButton);
    }

    public void SetPlayMenu()
    {
        EventSystem.current.SetSelectedGameObject(null);
        Invoke("SetPlayButtonCursor", .1f);
    }

    public void SetPlayButtonCursor()
    {
        EventSystem.current.SetSelectedGameObject(playButton);
    }
}
