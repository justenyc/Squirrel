using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignPost : MonoBehaviour
{
    [TextArea]
    public string SignText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<playermovement>())
        {
            UIManager.instance.InstantDialogue(SignText);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<playermovement>())
        {
            UIManager.instance.ClearDialogue();
        }
    }

}
