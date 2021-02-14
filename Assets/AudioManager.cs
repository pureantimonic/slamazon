using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioClip slowDownSound;
    public AudioClip speedUpSound;
    public AudioClip heartbeatSound;
    public float timeSlowVolume = 1.0f;

    public List<AudioClip> baseCollisionClips;
    public List<ListWrapper<AudioClip>> collisionClips;
    public float globalPitch;
    public AudioSource source;
    private bool timeSlowed = false;

    void Awake()
    {
        if (!Instance)
            Instance = this;

        source = GetComponent<AudioSource>();

    }

    void Update()
    {
        globalPitch = Time.deltaTime / Time.unscaledDeltaTime;
        if (timeSlowed && !source.isPlaying)
        {
            source.clip = heartbeatSound;
            source.Play();
        }
    }

    public void TimeSlow(bool starting)
    {
        if (timeSlowed == starting) return;
        timeSlowed = starting;
        source.clip = starting ? slowDownSound : speedUpSound;
        source.volume = timeSlowVolume;
        source.Play();
    }

}

