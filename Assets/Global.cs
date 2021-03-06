using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Global : MonoBehaviour
{
    public static Global Instance;
    public GameObject Player;

    public GameObject iconCanvas;

    public float Score = 0;

    public int PackageDeliverd = 0;

    public List<Destination> destinations;

    public GameObject[] Packages;
    public GameObject[] People;

    public GUIScript GUICanvas;

    public List<PickupLocation> depots;
    
    // Start is called before the first frame update
    void Awake()
    {
        if (!Instance)
            Instance = this;

        destinations = new List<Destination>();
        depots = new List<PickupLocation>();
    }

    public void RegisterDepot(PickupLocation pl)
    {
        depots.Add(pl);
    }

    public void RegisterDestination(Destination dest)
    {
        destinations.Add(dest);
    }

    public List<Destination> GetDestinations()
    {
        return destinations;
    }

    public Destination GetRandomDestination()
    {
        return destinations[Random.Range(0, destinations.Count)];
    }

    public void AddScore(float score)
    {
        Debug.Log("GET SCORE:  " + score);
        Score += Mathf.Max(0,score);
        GUICanvas.UpdateGUI((int)Score, PackageDeliverd);
    }

    public void AddPackage()
    {
        PackageDeliverd++;
        GUICanvas.UpdateGUI((int)Score, PackageDeliverd);
    }

    public GameObject GetRandomPackage()
    {
        return Packages[Random.Range(0, Packages.Length)];
    }

    public GameObject GetRandomPerson()
    {
        return People[Random.Range(0, People.Length)];
    }
    
    
    // Update is called once per frame
    void Update()
    {

    }
}
