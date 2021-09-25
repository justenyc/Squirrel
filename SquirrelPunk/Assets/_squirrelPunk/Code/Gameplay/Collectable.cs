using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] string _name;
    [SerializeField] Type type;
    [SerializeField] Area area;
    [SerializeField] GameObject _collectionEffect;

    [Header("Animation Parameters")]
    [SerializeField] bool _bob = true;
    [SerializeField] float _bobSpeed = 0.1f;
    [SerializeField] float _bobTime = 3f;
    float _bobCountdown = 0;

    [SerializeField] bool _spin = true;
    [SerializeField] float _rotationSpeed = 5f;

    Vector3 direction = Vector3.up;

    [Header("Pickup sound")]
    public AudioSource audioSource;
    public AudioClip[] PickupSound;

    public delegate void collect();
    public event collect nutCollection;

    private void Start()
    {
        _bobCountdown = _bobTime;

        if (_collectionEffect == null)
            Debug.LogError("Collection Effect is not assigned in the inspector!");
    }

    private void FixedUpdate()
    {
        if (_spin == true)
            Spin();

        if (_bob == true)
        {
            transform.position += new Vector3(direction.x, direction.y * _bobSpeed, direction.z);

            if (_bobCountdown > 0)
            {
                _bobCountdown -= Time.deltaTime;
            }
            else
            {
                direction *= -1;
                _bobCountdown = _bobTime;
            }
        }
    }
    public Type GetTypeEnum()
    {
        return type;
    }

    public Area GetAreaEnum()
    {
        return area;
    }

    void Spin()
    {
        Vector3 temp = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(new Vector3(temp.x, temp.y + _rotationSpeed * Time.deltaTime, temp.z));
    }

    private void OnTriggerEnter(Collider other)
    {
        //TODO: Should only get playermovement once here
        if (other.GetComponent<playermovement>())
        {
            foreach (AudioClip clip in PickupSound)
            {
                other.GetComponent<playermovement>().audioSource.PlayOneShot(clip);
            }

            if (Game_Manager.instance != null)
                Game_Manager.instance.CollectionListener(this);
            else
                Debug.LogError("Game Manager not found!");

            if (nutCollection != null)
            {
                nutCollection();
                Instantiate(_collectionEffect, this.transform.position, _collectionEffect.transform.rotation);
            }
            else
            {
                Instantiate(_collectionEffect, this.transform.position, _collectionEffect.transform.rotation);
            }
            this.gameObject.SetActive(false);
        }
    }
}

public enum Type
{
    Normal,
    Gold,
    Time
};

public enum Area
{
    one,
    two,
    three,
    four
};

