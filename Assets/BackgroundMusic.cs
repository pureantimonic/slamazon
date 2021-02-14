using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    private AudioSource source;

    private AudioLowPassFilter lp;

    private float lowCutoff = Mathf.Log(130f);
    private float highCutoff = Mathf.Log(22000f);

    [SerializeField] private float speed;

    private float idealCutoff;
    private float realCutoff;
    
    
    // Start is called before the first frame update
    void Start()
    {
        idealCutoff = highCutoff;
        realCutoff = lowCutoff;
        
        source = GetComponent<AudioSource>();
        lp = GetComponent<AudioLowPassFilter>();
        
    }

    // Update is called once per frame
    void Update()
    {
        realCutoff += (idealCutoff - realCutoff) * speed * Time.unscaledDeltaTime;
        lp.cutoffFrequency = Mathf.Clamp(Mathf.Exp(realCutoff), 10f, 22000f);
        source.pitch = AudioManager.Instance.globalPitch;
    }

    public void TimeSlow(bool starting)
    {
        idealCutoff = starting ? lowCutoff : highCutoff;
    }
}
