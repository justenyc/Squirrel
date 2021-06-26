using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [SerializeField] Image _timeTrialText;
    // Start is called before the first frame update
    private void Start()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);
    }

    public void SetTimeTrialTextActive(bool b)
    {
        _timeTrialText.gameObject.SetActive(b);
    }

    public void UpdateTimeTrialText(float value)
    {
        Text myText = _timeTrialText.GetComponentInChildren<Text>();
        myText.text = Mathf.Ceil(value).ToString();
    }
}