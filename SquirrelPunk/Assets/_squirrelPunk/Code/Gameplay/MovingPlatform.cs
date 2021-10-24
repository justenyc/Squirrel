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
    [SerializeField] bool move = true;
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
    void FixedUpdate()
    {
        if (waypoints.Count > 1)
            Movement();
    }

    void BackAndForth()
    {
        Vector3 movementVector = Vector3.MoveTowards(transform.position, waypoints[iterator].position, Time.deltaTime * moveSpeed) - transform.position;

        if (Vector3.Distance(transform.position, waypoints[iterator].position) < 0.1f && move == true)
        {
            move = false;

            try
            {
                if (forward == true)
                    iterator++;
                else
                    iterator--;

                Transform t = waypoints[iterator];
                StartCoroutine(DelayPlatform(delayTime, forward));
            }
            catch
            {
                forward = !forward;

                if (forward == true)
                    iterator += 2;
                else
                    iterator -= 2;

                StartCoroutine(DelayPlatform(delayTime, forward));
            }
        }


        if (move == true)
        {
            transform.position += movementVector;
            MovePlayer(movementVector);
        }
    }


    IEnumerator DelayPlatform(float delay, bool direction)
    {
        yield return new WaitForSecondsRealtime(delay);
        move = true;
    }

    void Loop()
    {
        try
        {
            Vector3 movementVector = Vector3.MoveTowards(transform.position, waypoints[iterator].position, Time.deltaTime * moveSpeed) - transform.position;

            if (Vector3.Distance(transform.position, waypoints[iterator].position) < 0.1f && move == true)
            {
                move = false;
                iterator++;
                StartCoroutine(DelayPlatform(delayTime, forward));
            }

            if (move == true)
            {
                transform.position += movementVector;
                MovePlayer(movementVector);
            }

        }
        catch
        {
            iterator = 0;
            StartCoroutine(DelayPlatform(delayTime, forward));
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

    //Player childs itself to any object tagged as 'Grounded'
    void MovePlayer(Vector3 movementVector)
    {
        if (this.GetComponentInChildren<CharacterController>())
        {
            CharacterController temp = this.GetComponentInChildren<CharacterController>();
            temp.Move(movementVector);
        }
    }
}

public enum Platform_Type
{
    BackAndForth,
    Loop
};
