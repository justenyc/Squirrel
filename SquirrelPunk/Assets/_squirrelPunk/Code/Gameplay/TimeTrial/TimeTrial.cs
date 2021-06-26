using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeTrial : MonoBehaviour
{
    [Header("Setup Objects")]
    [SerializeField] GameObject _clock;
    [SerializeField] GameObject _objectsToSpawn;
    Transform[] _objects;

    [Header("Time")]
    [SerializeField] bool _startCountdown;
    [Tooltip("Time in Seconds")] [SerializeField] float _allotedTime;
    [SerializeField] float _countdown;
    
    // Start is called before the first frame update
    void Start()
    {
        if (_clock == null)
        {
            Debug.LogError("Clock is not assigned in inspector");
        }

        if (_objectsToSpawn == null)
        {
            Debug.LogError("Objects to spawn is not assigned in the inspector");
        }
        else
        {
            _objects = _objectsToSpawn.GetComponentsInChildren<Transform>();
            _objectsToSpawn.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        if (_startCountdown == true && _countdown > 0)
        {
            _countdown -= Time.deltaTime;
            UIManager.instance.UpdateTimeTrialText(_countdown);
        }
        else
        {
            _startCountdown = false;
            Reset();
        }
    }

    private void Reset()
    {
        _startCountdown = false;
        _countdown = _allotedTime;
        _clock.GetComponent<Clock>().EnableInteractions(true);
        _objectsToSpawn.SetActive(false);
        UIManager.instance.SetTimeTrialTextActive(false);
    }

    void SpawnObjects(bool b)
    {
        _objectsToSpawn.SetActive(true);
        if (_objects.Length > 0)
        {
            foreach (Transform t in _objects)
            {
                Debug.Log(t.name);
                t.gameObject.SetActive(b);
            }
        }
    }

    public void StartTimeTrial()
    {
        SpawnObjects(true);
        _startCountdown = true;
        UIManager.instance.SetTimeTrialTextActive(true);
    }
}
