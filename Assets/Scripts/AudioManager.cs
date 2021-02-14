using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    
    public static AudioManager Instance;
    public GameObject temporarySound;

    public AudioClip slowDownSound;
    public AudioClip speedUpSound;
    public AudioClip heartbeatSound;
    public float timeSlowVolume = 1.0f;

    public List<AudioClip> baseCollisionClips;
    public List<ListWrapper<AudioClip>> collisionClips;
    public float globalPitch = 1f;
    public AudioSource source;
    private bool timeSlowed = false;

    public BackgroundMusic bgm;

    void Awake()
    {
        if (!Instance)
            Instance = this;

        source = GetComponent<AudioSource>();

    }

    void Update()
    {
        var idealPitch = Time.deltaTime / Time.unscaledDeltaTime;
        globalPitch += (idealPitch - globalPitch) * 10f * Time.unscaledDeltaTime;
        if (timeSlowed && !source.isPlaying)
        {
            source.clip = heartbeatSound;
            source.Play();
        }
    }

    public void TimeSlow(bool starting)
    {
        if (timeSlowed == starting) return;
        bgm.TimeSlow(starting);
        timeSlowed = starting;
        source.clip = starting ? slowDownSound : speedUpSound;
        source.volume = timeSlowVolume;
        source.Play();
    }

    public void PlaySound(
        Vector3 position,
        AudioClip clip,
        float _pitch = 1.0f,
        float volume = 1.0f)
    {
        var newSound = Instantiate(temporarySound);
        newSound.transform.position = position;
        var newSoundScript = newSound.GetComponent<TemporarySound>();

        newSoundScript.clip = clip;
        newSoundScript.pitch = _pitch;
        newSoundScript.volume = volume;

    }
    
    public void PlaySound(
        Transform _transform,
        AudioClip clip,
        float _pitch = 1.0f,
        float volume = 1.0f)
    {
        var newSound = Instantiate(temporarySound, _transform, false);
        var newSoundScript = newSound.GetComponent<TemporarySound>();

        newSoundScript.clip = clip;
        newSoundScript.pitch = _pitch;
        newSoundScript.volume = volume;

    }
}

