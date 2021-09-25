using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Manager : MonoBehaviour
{
    public static Game_Manager instance;
    public Checkpoint[] _checkpoints;
    public Cinemachine.CinemachineFreeLook vCam;

    [Header("Collectables")]
    public int _goldAcorns = 0;
    public int _normalAcorns = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (_checkpoints.Length == 0)
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
        switch (type)
        {
            case Type.Normal:
                _normalAcorns++;
                UIManager.instance.UpdateText("Nut_Display_Text", _normalAcorns);
                break;

            case Type.Gold:
                _goldAcorns++;
                UIManager.instance.UpdateText("Gold_Nut_Display_Text", _goldAcorns);
                break;

            case Type.Time:
                break;

            default:
                Debug.LogError("Incorrect Type");
                break;
        }
    }

    public void EnableClocks(bool b)
    {
        Clock[] _clocks = FindObjectsOfType<Clock>();

        foreach (Clock c in _clocks)
        {
            c.EnableInteractions(b);
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

        if (activeCheckpoint == null)
        {
            _checkpoints[0].SetActiveCheckpoint(true);
            activeCheckpoint = _checkpoints[0];
        }

        player.gameObject.transform.position = activeCheckpoint.gameObject.transform.position + Vector3.up * 10;
    }

    public void SetActiveCheckpoint(Checkpoint newActiveCheckpoint)
    {
        foreach (Checkpoint cp in _checkpoints)
        {
            if (cp != newActiveCheckpoint)
                cp.SetActiveCheckpoint(false);
        }

        newActiveCheckpoint.SetActiveCheckpoint(true);
    }

    public void LookAtTargetTemp(Transform target)
    {
        StartCoroutine(LookAtTargetTemporarily(target));
    }

    IEnumerator LookAtTargetTemporarily(Transform target)
    {
        vCam.LookAt = target;
        yield return new WaitForSecondsRealtime(2f);
        vCam.LookAt = FindObjectOfType<playermovement>().gameObject.transform;
    }
}