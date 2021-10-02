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

    [SerializeField] TextMeshProUGUI totalNut;
    [SerializeField] TextMeshProUGUI goldNut;


    Dictionary<string, TextMeshProUGUI> d = new Dictionary<string, TextMeshProUGUI>();

    PlayerInput playerInput;

    bool paused;

    [SerializeField] Animator fadeAnimator;


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

        totalNut.text = Game_Manager.instance._normalAcorns.ToString();
        goldNut.text = Game_Manager.instance._goldAcorns.ToString();
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

    public void StartFade()
    {
        fadeAnimator.SetTrigger("FadeStart");
    }

    public void EndFade()
    {
        fadeAnimator.SetTrigger("FadeEnd");
    }

}