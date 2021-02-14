using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporarySound : MonoBehaviour
{

    public AudioClip clip;

    public float volume;

    public float pitch;

    private AudioSource source;
    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
        source.clip = clip;
        source.volume = volume;
        source.pitch = pitch;
        source.Play();
    }

    // Update is called once per frame
    void Update()
    {
        source.pitch = pitch * AudioManager.Instance.globalPitch;
        if (!source.isPlaying)
        {
            Destroy(gameObject);
        }
    }
}
