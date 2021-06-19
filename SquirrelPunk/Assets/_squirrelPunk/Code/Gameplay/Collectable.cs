using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    [SerializeField] Type type;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<playermovement>())
        {
            Game_Manager.instance.CollectionListener(type);
            Destroy(this.gameObject);
        }
    }
}

public enum Type
{
    Normal,
    Gold
};