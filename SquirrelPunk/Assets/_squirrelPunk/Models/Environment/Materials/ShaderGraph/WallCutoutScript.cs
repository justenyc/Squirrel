using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCutoutScript : MonoBehaviour
{
    public static int PosID = Shader.PropertyToID("_playerPos");
    public static int SizeID = Shader.PropertyToID("_size");

    public Material WallMaterial;
    Material instanceMaterial;
    public Camera Camera;
    public LayerMask Mask;

    // Update is called once per frame
    void Update()
    {
        var dir = Camera.transform.position - transform.position;
        RaycastHit hit;

        if (Physics.Raycast(transform.position, dir, out hit, Vector3.Distance(transform.position, Camera.transform.position)))
        {
            if (hit.collider.tag == "wall")
            {
                if (instanceMaterial != null)
                    instanceMaterial.SetFloat(SizeID, 0);

                instanceMaterial = hit.collider.gameObject.GetComponent<MeshRenderer>().material;
                instanceMaterial.SetFloat(SizeID, 1);
            }
        }
        else
        {
            instanceMaterial.SetFloat(SizeID, 0);
        }

        Debug.DrawRay(transform.position, dir, Color.magenta);
        var view = Camera.WorldToViewportPoint(transform.position);
        WallMaterial.SetVector(PosID, view);
    }
}
