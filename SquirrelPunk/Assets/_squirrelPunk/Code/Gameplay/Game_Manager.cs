using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Manager : MonoBehaviour
{
    public static Game_Manager instance;
    public Checkpoint[] _checkpoints;

    [Header("Collectables")]
    public int _goldAcorns = 0;
    public int _normalAcorns = 0;

    // Start is called before the first frame update
    void Start()
    {
        _checkpoints = FindObjectsOfType<Checkpoint>();
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

    Checkpoint GetActiveCheckpoint()
    {
        foreach (Checkpoint cp in _checkpoints)
        {
            if (cp._active == true)
            {
                return cp;
            }
        }
        return null;
    }

    public void Respawn(playermovement player)
    {
        Checkpoint activeCheckpoint = GetActiveCheckpoint();

        player.gameObject.transform.position = activeCheckpoint.gameObject.transform.position + Vector3.forward;
    }

    public void SetActiveCheckpoint(Checkpoint newActiveCheckpoint)
    {
        foreach(Checkpoint cp in _checkpoints)
        {
            cp._active = false;
        }

        newActiveCheckpoint.SetActiveCheckpoint(true);
    }
}