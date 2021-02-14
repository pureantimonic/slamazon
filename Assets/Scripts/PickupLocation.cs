using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupLocation : MonoBehaviour
{
    private Package currentPackage;
    private bool hasPackage;
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
        return !hasPackage;
    }

   

    public void OnPackagePickedUp()
    {
        hasPackage = false;
        currentPackage = null;
        om.OnOpenLocation(this);
        //StartCoroutine(WillSpawnPackage(3));
    }

    private IEnumerator DoSpawnPackageInTime(float t, OrderManager.Order ord)
    {
        yield return new WaitForSeconds(t);
        SpawnPackage(ord);
    }
    public void DoSpawnPackage(OrderManager.Order ord)
    {
        hasPackage = true;   
        StartCoroutine(DoSpawnPackageInTime(2, ord));
    }


    public void SpawnPackage(OrderManager.Order ord)
    {
        GameObject newPackage = GameObject.Instantiate(Global.Instance.GetRandomPackage());
        newPackage.transform.position = spawnPoint.position;
        newPackage.GetComponent<Package>().pl = this;
        newPackage.GetComponent<Package>().SetOrder(ord);
        ord.package = newPackage;
        currentPackage = newPackage.GetComponent<Package>();
    }
    
}
