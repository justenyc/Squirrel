using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public bool _active = false;
    public float appearanceSpeed = 1;
    public GameObject beam;

    public MeshRenderer[] mats;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip ActivateSound;

    private void Start()
    {
        mats = GetComponentsInChildren<MeshRenderer>();
        StartCoroutine(AnimateAppearance(_active));
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

    IEnumerator AnimateAppearance(bool active)
    {
        if (active)
        {
            for (float i = mats[1].material.GetFloat("Appearance"); i < 1; i += Time.deltaTime * appearanceSpeed)
            {
                mats[1].material.SetFloat("Appearance", Mathf.Clamp01(mats[1].material.GetFloat("Appearance") + Time.deltaTime * appearanceSpeed * 10));
                mats[2].material.SetFloat("Appearance", Mathf.Clamp01(mats[2].material.GetFloat("Appearance") + Time.deltaTime * appearanceSpeed * 10));
                yield return new WaitForSeconds(Time.deltaTime);
            }
        }
        else
        {
            for (float i = mats[1].material.GetFloat("Appearance"); i > 0; i -= Time.deltaTime * appearanceSpeed)
            {
                mats[1].material.SetFloat("Appearance", Mathf.Clamp01(mats[1].material.GetFloat("Appearance") - Time.deltaTime * appearanceSpeed * 10));
                mats[2].material.SetFloat("Appearance", Mathf.Clamp01(mats[2].material.GetFloat("Appearance") - Time.deltaTime * appearanceSpeed * 10));
                yield return new WaitForSeconds(Time.deltaTime);
            }
        }
    }

    public void SetActiveCheckpoint(bool isActive)
    {
        _active = isActive;
        audioSource.PlayOneShot(ActivateSound);
        StartCoroutine(AnimateAppearance(_active));
    }
}
