using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public bool _active = false;
    public float appearanceSpeed = 1;
    public GameObject beam;

    public MeshRenderer[] mats;

    private void Start()
    {
        mats = GetComponentsInChildren<MeshRenderer>();
        //StartCoroutine(AnimateAppearance());
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
        StartCoroutine(AnimateAppearance(_active));
    }

    IEnumerator AnimateAppearance(bool active)
    {
        if (active)
        {
            for (float i = 0; i < 1; i += Time.deltaTime * appearanceSpeed)
            {
                mats[1].material.SetFloat("Appearance", mats[1].material.GetFloat("Appearance") + Time.deltaTime * appearanceSpeed);
                mats[2].material.SetFloat("Appearance", mats[2].material.GetFloat("Appearance") + Time.deltaTime * appearanceSpeed);
                yield return new WaitForSeconds(Time.deltaTime);
            }
        }
        else
        {
            for (float i = 1; i > 0; i -= Time.deltaTime * appearanceSpeed)
            {
                mats[1].material.SetFloat("Appearance", mats[1].material.GetFloat("Appearance") - Time.deltaTime * appearanceSpeed);
                mats[2].material.SetFloat("Appearance", mats[2].material.GetFloat("Appearance") - Time.deltaTime * appearanceSpeed);
                yield return new WaitForSeconds(Time.deltaTime);
            }
        }
    }

    public void SetActiveCheckpoint(bool isActive)
    {
        _active = isActive;
    }
}
