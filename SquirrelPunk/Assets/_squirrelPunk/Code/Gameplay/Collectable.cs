using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    [SerializeField] Type type;

    public delegate void collect();
    public event collect nutCollection;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<playermovement>())
        {
            Game_Manager.instance.CollectionListener(type);

            if (nutCollection != null)
            {
                nutCollection();
            }
            this.gameObject.SetActive(false);
        }
    }
}

public enum Type
{
    Normal,
    Gold
};