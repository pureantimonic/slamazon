using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Package : MonoBehaviour
{
    [SerializeField] public Vector3 anchorPoint;


    public void OnPickedUp()
    {
        if (pl)
        {
            pl.OnPackagePickedUp();
            pl = null;
        }
    }
    
    public PickupLocation pl;
    public Vector3 Destination;
    public MeshRenderer mainMesh;

}
