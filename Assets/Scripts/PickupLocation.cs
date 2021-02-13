using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupLocation : MonoBehaviour
{
    private Package currentPackage;
    
    [SerializeField] private Transform spawnPoint;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WillSpawnPackage(1));
    }

    public IEnumerator WillSpawnPackage(float t)
    {
        yield return new WaitForSeconds(t);
        SpawnPackage();
    }

    public void OnPackagePickedUp()
    {
        StartCoroutine(WillSpawnPackage(3));
    }
    
    void SpawnPackage()
    {
        GameObject newPackage = GameObject.Instantiate(Global.Instance.GetRandomPackage());
        newPackage.transform.position = spawnPoint.position;
        newPackage.GetComponent<Package>().pl = this;
        newPackage.GetComponent<Package>().destination = Global.Instance.GetRandomDestination().destinationPoint.position;

    }
    
}
