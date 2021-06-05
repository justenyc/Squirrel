using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterModel : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        transform.localPosition = new Vector3(0, 0.5f, 0);
    }
}
