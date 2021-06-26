using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    [SerializeField] Type type;

    [SerializeField] bool _spin = true;
    [SerializeField] bool _bob = true;
    [SerializeField] float _bobTime = 3f;
    [SerializeField] float _bobCountdown = 0;
    [SerializeField] float _bobSpeed = 0.1f;
    [SerializeField] float _rotationSpeed = 5f;

    Vector3 direction = Vector3.up;

    public delegate void collect();
    public event collect nutCollection;

    private void Start()
    {
        _bobCountdown = _bobTime;
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

    void Spin()
    {
        Vector3 temp = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(new Vector3(temp.x, temp.y + _rotationSpeed * Time.deltaTime, temp.z));
    }
    
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