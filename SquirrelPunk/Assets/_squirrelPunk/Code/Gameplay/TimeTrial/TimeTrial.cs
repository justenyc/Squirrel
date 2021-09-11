using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeTrial : MonoBehaviour
{
    //Starting from Clock script. Also sets listener
    //references UIManager script 

    [Header("Setup Objects")]
    [SerializeField] GameObject _clock;
    [SerializeField] GameObject _objectsToSpawn;
    [SerializeField] GameObject _victoryAcorn;
    [SerializeField] GameObject _victoryAcornAppearanceEffect;
    Transform[] _objects;

    [Header("Trial")]
    [SerializeField] bool _startCountdown;
    [Tooltip("Time in Seconds")] [SerializeField] float _allotedTime;
    [SerializeField] float _countdown;
    [Tooltip("The total number of collectables to be collected")]
    [SerializeField] int _numberToCollect;
    [Tooltip("The current number of collectables that the player has collected during the time trial. Resets if timer reaches zero")]
    [SerializeField] int _numberCollected;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioSource extraAudioSource;
    public AudioClip StartSound;
    public AudioClip ClockSound;
    public AudioClip FailSound;
    public AudioClip VictorySound;

    // Start is called before the first frame update
    void Start()
    {
        playermovement p = FindObjectOfType<playermovement>();
        p.died += PlayerDeathListener;

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
                TrialCollectablesSubscriber(true);
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

        if (_victoryAcornAppearanceEffect == null)
        {
            Debug.LogError("Victory Acorn Appearance effect is not assigned in the inspector");
        }
    }

    private void FixedUpdate()
    {
        if (_startCountdown == true && _countdown > 0)
        {
            _countdown -= Time.deltaTime;
            UIManager.instance.UpdateText("Time_Trial_Text", Mathf.Ceil(_countdown));
        }
        else if (_startCountdown == true && _countdown <= 0)
        {
            _startCountdown = false;
            Reset();
        }
    }

    void TrialCollectablesSubscriber(bool subscribe)
    {
        if (_objects.Length > 0)
        {
            foreach(Transform T in _objects)
            {
                Collectable newCollectable = T.GetComponent<Collectable>();
                if (newCollectable != null)
                {
                    if (subscribe == true)
                        newCollectable.nutCollection += NutCollectionListener;
                    else
                        newCollectable.nutCollection -= NutCollectionListener;
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
        Camera.main.GetComponent<BackgroundAudio>().PlayNormalBGM();
        audioSource.PlayOneShot(FailSound);
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
        audioSource.PlayOneShot(StartSound);
        extraAudioSource.PlayOneShot(ClockSound);
        Camera.main.GetComponent<BackgroundAudio>().PlayTimeTrial();
        SpawnObjects(true);
        _startCountdown = true;
        UIManager.instance.SetTimeTrialTextActive(true);
    }

    void TimeTrialVictory()
    {
        audioSource.PlayOneShot(VictorySound);
        _victoryAcorn.SetActive(true);
        Instantiate(_victoryAcornAppearanceEffect, _victoryAcorn.transform.position, _victoryAcornAppearanceEffect.transform.rotation);
        _victoryAcorn.transform.parent = null;

        TrialCollectablesSubscriber(false);
        UIManager.instance.SetTimeTrialTextActive(false);
        Camera.main.GetComponent<BackgroundAudio>().PlayNormalBGM();
        Destroy(this.gameObject);
    }

    void PlayerDeathListener()
    {
        Reset();
    }
}
