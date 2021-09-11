using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clock : MonoBehaviour
{
    [SerializeField] float _rotationSpeed = 0;
    TimeTrial _timeTrail;
    GameObject child;
    // Start is called before the first frame update
    void Start()
    {
        _timeTrail = transform.parent.GetComponent<TimeTrial>();
        child = transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        Rotate();
    }

    public void EnableInteractions(bool b)
    {
        this.GetComponent<MeshRenderer>().enabled = b;
        this.GetComponent<SphereCollider>().enabled = b;
        child.SetActive(b);
    }

    void Rotate()
    {
        Vector3 temp = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(new Vector3(temp.x, temp.y + _rotationSpeed * Time.deltaTime, temp.z));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out playermovement p))
        {
            _timeTrail.StartTimeTrial();
            EnableInteractions(false);
        }
    }
}