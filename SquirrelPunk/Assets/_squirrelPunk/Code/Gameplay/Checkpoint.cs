using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public bool _active = false;
    public GameObject beam;

    Material myMat;

    private void Start()
    {
        myMat = this.GetComponent<MeshRenderer>().material;
        myMat.EnableKeyword("_EmissionColor");
        SetEmissionColor(_active);
    }

    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<playermovement>())
        {
            if (Game_Manager.instance != null)
            {
                Game_Manager.instance.SetActiveCheckpoint(this);
            }
        }
    }

    public void SetActiveCheckpoint(bool isActive)
    {
        _active = isActive;
        SetEmissionColor(_active);
    }

    void SetEmissionColor(bool isActive)
    {
        if (isActive == false)
        {
            beam.SetActive(false);//Temporary because EmissionColor seems broken in unity during editor runtime
            //myMat.SetColor("_EmissionColor", Color.black);
        }
        else
        {
            beam.SetActive(true);
            //myMat.SetColor("_EmissionColor", new Color(0,0,2,1));
        }
    }
}
