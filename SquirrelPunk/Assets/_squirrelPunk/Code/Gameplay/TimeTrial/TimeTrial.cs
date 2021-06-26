using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeTrial : MonoBehaviour
{
    [Header("Setup Objects")]
    [SerializeField] GameObject _clock;
    [SerializeField] GameObject _objectsToSpawn;
    [SerializeField] GameObject _victoryAcorn;
    Transform[] _objects;

    [Header("Trial")]
    [SerializeField] bool _startCountdown;
    [Tooltip("Time in Seconds")] [SerializeField] float _allotedTime;
    [SerializeField] float _countdown;
    [Tooltip("The total number of collectables to be collected")]
    [SerializeField] int _numberToCollect;
    [Tooltip("The current number of collectables that the player has collected during the time trial. Resets if timer reaches zero")]
    [SerializeField] int _numberCollected;

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

            if (_objects.Length > 1)
            {
                _numberToCollect = _objectsToSpawn.GetComponentsInChildren<Transform>().Length - 1;
                _objectsToSpawn.SetActive(false);
                InitializeTrialCollectableListener();
            }
            else
            {
                Debug.LogError("There are no objects set as a child to Objects to Spawn");
            }
        }

        if (_victoryAcorn == null)
        {
            Debug.LogError("The Victory Acorn is not assigned in the inspector");
        }
        else
        {
            _victoryAcorn.SetActive(false);
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

    void InitializeTrialCollectableListener()
    {
        if (_objects.Length > 0)
        {
            foreach(Transform T in _objects)
            {
                Collectable newCollectable = T.GetComponent<Collectable>();
                if (newCollectable != null)
                {
                    newCollectable.nutCollection += NutCollectionListener;
                }
            }
        }
    }

    void NutCollectionListener()
    {
        _numberCollected++;

        if(_numberCollected == _numberToCollect)
        {
            TimeTrialVictory();
        }
    }

    private void Reset()
    {
        _startCountdown = false;
        _countdown = _allotedTime;
        _clock.GetComponent<Clock>().EnableInteractions(true);
        _objectsToSpawn.SetActive(false);
        _numberCollected = 0;
        UIManager.instance.SetTimeTrialTextActive(false);
    }

    void SpawnObjects(bool b)
    {
        _objectsToSpawn.SetActive(true);
        if (_objects.Length > 0)
        {
            foreach (Transform t in _objects)
            {
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

    void TimeTrialVictory()
    {
        _victoryAcorn.SetActive(true);
    }
}
