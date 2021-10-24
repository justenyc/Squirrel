using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundAudio : MonoBehaviour
{
    public AudioClip[] audioClips;
    public AudioListener audioListener;

    public AudioSource BackgroundMusicSource;
    public AudioClip TimeTrialMusic;
    public AudioClip BackgroundMusic;

    public float PlayTimeVariation = 2;
    public float PlayTimeInterval = 12;

    private float NextPlay;
    private float TimeCount;
    private int CurrentMusic;

    //Audio source for random ambience sound clips
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        NextPlay = Random.Range(PlayTimeInterval - PlayTimeVariation, PlayTimeInterval + PlayTimeVariation);
        PlayNormalBGM();
    }

    void Update()
    {
        if (TimeCount > NextPlay)
        {
            NextPlay = Random.Range(PlayTimeInterval - PlayTimeVariation, PlayTimeInterval + PlayTimeVariation);
            TimeCount = 0.0f;
            CurrentMusic = 0; //Random.next(music.Length);
            //audioSource.clip = audioClips[Random.Range(0, audioClips.Length)];
            //audioSource.Play();
        }

        TimeCount += Time.deltaTime;
    }

    public void PlayTimeTrial()
    {
        BackgroundMusicSource.Stop();
        BackgroundMusicSource.loop = true;
        BackgroundMusicSource.PlayOneShot(TimeTrialMusic);
        BackgroundMusicSource.pitch = 1.3f;
    }

    public void PlayNormalBGM()
    {
        BackgroundMusicSource.Stop();
        BackgroundMusicSource.loop = true;
        BackgroundMusicSource.pitch = 1.0f;
        audioSource.clip = BackgroundMusic;
        BackgroundMusicSource.Play();
    }
}



