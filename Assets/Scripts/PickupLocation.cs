using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupLocation : MonoBehaviour
{
    private Package currentPackage;
    private OrderManager om;
    [SerializeField] private Transform spawnPoint;
    // Start is called before the first frame update

    public void SetOrderManager(OrderManager _om)
    {
        om = _om;
    }
    void Start()
    {
        Global.Instance.RegisterDepot(this);
    }

    public bool CanSpawnPackage()
    {
        return currentPackage == null;
    }

   

    public void OnPackagePickedUp()
    {
        currentPackage = null;
        om.OnOpenLocation(this);
        //StartCoroutine(WillSpawnPackage(3));
    }


    
    
    public void SpawnPackage(OrderManager.Order ord)
    {
        GameObject newPackage = GameObject.Instantiate(Global.Instance.GetRandomPackage());
        newPackage.transform.position = spawnPoint.position;
        newPackage.GetComponent<Package>().pl = this;
        newPackage.GetComponent<Package>().SetOrder(ord);
        currentPackage = newPackage.GetComponent<Package>();
    }
    
}
