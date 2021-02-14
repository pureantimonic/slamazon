using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneSound : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody rigid;
    private AudioSource source;
    private Vector3 lastVelocity = Vector3.zero;
    private float pitch = 0.0f;
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        source = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        pitch = rigid.velocity.magnitude / 10f + 0.5f;

    }

    private void Update()
    {
        source.pitch = AudioManager.Instance.globalPitch * pitch;
    }
}
