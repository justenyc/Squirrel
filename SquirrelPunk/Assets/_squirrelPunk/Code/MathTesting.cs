using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathTesting : MonoBehaviour
{
    public bool shootCast = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position, transform.position + transform.right * 5f, Color.red);
        Debug.DrawRay(transform.position, transform.position + transform.forward * 5f, Color.blue);
        if (shootCast)
        {
            shootCast = false;
            ShootRay();
        }
    }

    void ShootRay()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.right, out hit, 10f))
        {
            Debug.Log(hit.transform.name);
            //https://answers.unity.com/questions/1333667/perpendicular-to-a-3d-direction-vector.html
            Vector3 hitRot = hit.transform.rotation.eulerAngles;
            Vector3 currentRot = transform.rotation.eulerAngles;
            float angle = Vector3.Angle(transform.forward, new Vector3(hit.normal.z, 0, -hit.normal.x));
            //transform.rotation = Quaternion.Euler(new Vector3(currentRot.x, angle, currentRot.z));
            transform.rotation = Quaternion.FromToRotation(-Vector3.right, hit.normal);

        }
    }
}
