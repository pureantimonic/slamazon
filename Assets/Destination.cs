using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destination : MonoBehaviour
{
    public Transform destinationPoint;

    private void Start()
    {
        Global.Instance.RegisterDestination(this);
    }
}
