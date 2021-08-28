using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footsteps : MonoBehaviour
{

    public AudioSource audioSource;

    public AudioClip RunGroundNormalLegSound;
    public AudioClip RunGroundMetallicLegSound;

    public AudioClip SprintGroundFootstepSound;
    
    public AudioClip WallrunNormalLegSound;
    public AudioClip WallrunMetallicLegSound;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NormalFootstep()
    {
        audioSource.PlayOneShot(RunGroundNormalLegSound);
    }

    public void MetallicFootstep()
    {
        audioSource.PlayOneShot(RunGroundMetallicLegSound);
    }

    public void WallRunNormalFootstep()
    {
        audioSource.PlayOneShot(WallrunNormalLegSound);
    }

    public void WallRunMetallicFootstep()
    {
        audioSource.PlayOneShot(WallrunMetallicLegSound);
    }

    public void SprintFootstep()
    {
        audioSource.PlayOneShot(SprintGroundFootstepSound);
    }
}
