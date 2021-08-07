using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] Platform_Type type;
    [SerializeField] float moveSpeed;
    [SerializeField] float delayTime;
    [SerializeField] List<Transform> waypoints;

    [SerializeField] bool forward = true;
    [SerializeField] bool delayBuffer = false;
    [SerializeField] int iterator = 0;

    // Start is called before the first frame update
    void Start()
    {
        waypoints = new List<Transform>(GetComponentsInChildren<Transform>());

        if (waypoints.Count > 2)
            waypoints.RemoveAt(0);

        if (waypoints.Count > 1)
        {
            foreach (Transform t in waypoints)
            {
                t.parent = null;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (waypoints.Count > 1)
            Movement();
    }

    void BackAndForth()
    {
        try
        {
            if (Vector3.Distance(transform.position, waypoints[iterator].position) < 0.1f && delayBuffer == false)
            {
                delayBuffer = true;
                StartCoroutine(DelayPlatform(delayTime, forward));
            }
            if (this.GetComponentInChildren<CharacterController>())
            {
                CharacterController temp = this.GetComponentInChildren<CharacterController>();
                temp.Move(Vector3.MoveTowards(transform.position, waypoints[iterator].position, Time.deltaTime * moveSpeed));
            }
            transform.position = Vector3.MoveTowards(transform.position, waypoints[iterator].position, Time.deltaTime * moveSpeed);
        }
        catch
        {
            forward = !forward;

            StartCoroutine(DelayPlatform(delayTime, forward));
        }
    }


    IEnumerator DelayPlatform(float delay, bool direction)
    {
        yield return new WaitForSeconds(delay);
        if (direction)
        {
            iterator++;
        }
        else
        {
            iterator--;
        }
        yield return new WaitForSeconds(0.25f);
        delayBuffer = false;
    }

    IEnumerator DelayPlatform(float delay, bool direction, int iteratorForce)
    {
        yield return new WaitForSeconds(delay);
        iterator = iteratorForce;
        yield return new WaitForSeconds(0.25f);
        delayBuffer = false;
    }

    void Loop()
    {
        try
        {
            if (Vector3.Distance(transform.position, waypoints[iterator].position) < 0.1f && delayBuffer == false)
            {
                delayBuffer = true;
                StartCoroutine(DelayPlatform(delayTime, forward));
            }
            if (this.GetComponentInChildren<CharacterController>())
            {
                CharacterController temp = this.GetComponentInChildren<CharacterController>();
                //temp.Move(Vector3.MoveTowards(transform.position, waypoints[iterator].position, Time.deltaTime * moveSpeed));
            }
            transform.position = Vector3.MoveTowards(transform.position, waypoints[iterator].position, Time.deltaTime * moveSpeed);
        }
        catch
        {
            delayBuffer = true;
            StartCoroutine(DelayPlatform(delayTime, forward, 0));
        }
    }

    void Movement()
    {
        switch (type)
        {
            case Platform_Type.BackAndForth:
                BackAndForth();
                break;

            case Platform_Type.Loop:
                Loop();
                break;
        }
    }
}

public enum Platform_Type
{
    BackAndForth,
    Loop
};
