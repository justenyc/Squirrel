using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [SerializeField] TextMeshProUGUI dialogueBox;
    [SerializeField] TextMeshProUGUI[] tmpArray;
    [SerializeField] GameObject pauseMenu;
    Dictionary<string, TextMeshProUGUI> d = new Dictionary<string, TextMeshProUGUI>();

    PlayerInput playerInput;

    bool paused;


    // Start is called before the first frame update
    private void Start()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);

        foreach (TextMeshProUGUI tmp in tmpArray)
        {
            d.Add(tmp.name, tmp);
        }

        ClearDialogue();

        
    }

    private void Update()
    {
        Gamepad gp = InputSystem.GetDevice<Gamepad>();
        if (gp.startButton.wasPressedThisFrame)
        {
            OnPause();
        }
    }

    public void SetTimeTrialTextActive(bool b)
    {
        d["Time_Trial_Text"].gameObject.transform.parent.gameObject.SetActive(b);
    }

    public void UpdateText(string textName, float value)
    {
        d[textName].text = value.ToString();
    }

    public void InstantDialogue(string dialogue)
    {
        dialogueBox.gameObject.transform.parent.gameObject.SetActive(true);
        dialogueBox.text = dialogue;
    }

    public void ClearDialogue()
    {
        dialogueBox.text = string.Empty;
        dialogueBox.gameObject.transform.parent.gameObject.SetActive(false);
    }

    public void OnPause()
    {
        paused = !paused;
        FindObjectOfType<playermovement>().enabled = !paused;
        if (paused)
        {
            pauseMenu.gameObject.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            pauseMenu.gameObject.SetActive(false);
            Time.timeScale = 1;
        }
    }

}