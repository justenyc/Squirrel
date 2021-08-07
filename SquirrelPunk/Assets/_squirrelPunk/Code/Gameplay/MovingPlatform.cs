using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] Platform_Type type;
    [SerializeField] float moveSpeed;
    [SerializeField] List<Transform> waypoints;

    bool forward;
    int iterator = 0;

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
            if (Vector3.Distance(transform.position, waypoints[iterator].position) < 0.1f)
            {
                if (forward)
                {
                    iterator++;
                }
                else
                {
                    iterator--;
                }
            }
            transform.position = Vector3.MoveTowards(transform.position, waypoints[iterator].position, Time.deltaTime * moveSpeed);
        }
        catch
        {
            forward = !forward;

            if (forward)
            {
                iterator++;
            }
            else
            {
                iterator--;
            }
        }
    }

    void Loop()
    {
        try
        {
            if (Vector3.Distance(transform.position, waypoints[iterator].position) < 0.1f)
            {
                iterator++;
            }
            transform.position = Vector3.MoveTowards(transform.position, waypoints[iterator].position, Time.deltaTime * moveSpeed);
        }
        catch
        {
            iterator = 0;
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
