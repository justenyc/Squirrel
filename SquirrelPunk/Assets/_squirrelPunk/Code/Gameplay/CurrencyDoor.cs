using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CurrencyDoor : MonoBehaviour
{
    [SerializeField] TextMeshPro number;
    [SerializeField] SpriteRenderer typeSprite;

    [SerializeField] Currency currencyType;
    [SerializeField] int requiredAmount;

    [SerializeField] MeshRenderer[] mats;
    [SerializeField] float dissolveSpeed;
    [SerializeField] bool dissolve;

    // Start is called before the first frame update
    void Start()
    {
        number.text = requiredAmount.ToString();
        mats = this.GetComponentsInChildren<MeshRenderer>();
    }

    bool CheckCurrency()
    {
        switch(currencyType)
        {
            case Currency.Gold:
                return (Game_Manager.instance._goldAcorns >= requiredAmount);

            case Currency.Normal:
                return (Game_Manager.instance._normalAcorns >= requiredAmount);

            default:
                return false;
        }
    }

    IEnumerator Dissolve()
    {
        number.enabled = false;
        typeSprite.enabled = false;
        this.GetComponent<BoxCollider>().enabled = false;

        if (dissolve)
        {
            for (float i = 1; i > 0; i -= Time.deltaTime * dissolveSpeed)
            {
                mats[0].material.SetFloat("Dissolve", mats[0].material.GetFloat("Dissolve") - Time.deltaTime * dissolveSpeed);
                mats[1].material.SetFloat("Dissolve", mats[1].material.GetFloat("Dissolve") - Time.deltaTime * dissolveSpeed);
                yield return new WaitForSeconds(Time.deltaTime);
            }
        }
        else
        {
            for (float i = 0; i < 1; i += Time.deltaTime * dissolveSpeed)
            {
                mats[0].material.SetFloat("Dissolve", mats[0].material.GetFloat("Dissolve") + Time.deltaTime * dissolveSpeed);
                mats[1].material.SetFloat("Dissolve", mats[1].material.GetFloat("Dissolve") + Time.deltaTime * dissolveSpeed);
                yield return new WaitForSeconds(Time.deltaTime);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Game_Manager.instance == null)
        {
            Debug.LogError("Game Manager not found!");
            Debug.Break();
        }

        playermovement p = other.GetComponent<playermovement>();
        if (p == null)
        {
            Debug.Log("No playermovement script found on other object");
        }
        else
        {
            if(CheckCurrency())
            {
                StartCoroutine(Dissolve());
            }
        }
    }

    enum Currency
    {
        Normal,
        Gold
    };
}