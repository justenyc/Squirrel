using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{
    public GameObject explosionEffect;
    public MeshRenderer mr;

    private void Start()
    {
        mr = this.GetComponent<MeshRenderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<playermovement>() != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }
        StartCoroutine(DeRespawn());
    }

    IEnumerator DeRespawn()
    {
        mr.enabled = false;
        yield return new WaitForSeconds(0.5f);
        mr.enabled = true;
    }
}
