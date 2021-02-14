using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackageAudio : MonoBehaviour
{
    // Start is called before the first frame update
    private AudioSource baseSource;
    private AudioSource source;
    private float basePitch = 1.0f;
    private float pitch = 1.0f;

    [SerializeField] private int velocityThreshold = 5;
    [SerializeField] private float baseProbability = 0.9f;
    [SerializeField] private float specialChance = 0.2f;

    private int packageType;
    
    void Start()
    {
        var audioSources = GetComponents<AudioSource>();
        baseSource = audioSources[0];
        source = audioSources[1];
        pitch = Random.Range(0.8f, 1.2f);
        
        if (Random.value < specialChance)
        {
            packageType = Random.Range(0, AudioManager.Instance.collisionClips.Count);
        }
        else
        {
            packageType = -1;
        }

    }
    
    public void OnCollisionEnter(Collision other)
    {
        if (packageType > -1 && other.relativeVelocity.magnitude > velocityThreshold && !source.isPlaying)
        {
            var choices = AudioManager.Instance.collisionClips[packageType].innerList;
            source.clip = choices[Random.Range(0, choices.Count)];
            if (source.enabled)
            {
                source.Play();
            }
        }
        
        // regular box noise
        if (Random.value < baseProbability)
        {
            basePitch = Random.Range(0.6f, 1.4f);
            if (baseSource.enabled)
            {
                baseSource.PlayOneShot(
                    AudioManager.Instance.baseCollisionClips[Random.Range(0, AudioManager.Instance.baseCollisionClips.Count)],
                    0.5f
                );
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        baseSource.pitch = basePitch * AudioManager.Instance.globalPitch;
        source.pitch = pitch * AudioManager.Instance.globalPitch;


    }
}
