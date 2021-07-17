using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Manager : MonoBehaviour
{
    public static Game_Manager instance;
    public int _goldAcorns = 0;
    public int _normalAcorns = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void CollectionListener(Type type)
    {
        switch(type)
        {
            case Type.Normal:
                _normalAcorns++;
                break;

            case Type.Gold:
                _goldAcorns++;
                break;

            case Type.Time:
                break;

            default:
                Debug.LogError("Incorrect Type");
                break;
        }
    }
}