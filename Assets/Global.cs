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

    public TextMeshProUGUI ScoreText;

    public TextMeshProUGUI PackageText;

    public List<Destination> destinations;

    public GameObject[] Packages;
    // Start is called before the first frame update
    void Awake()
    {
        if (!Instance)
            Instance = this;

        destinations = new List<Destination>();
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
        Score += Mathf.Max(0,score);
    }

    public void AddPackage()
    {
        PackageDeliverd++;
    }

    public GameObject GetRandomPackage()
    {
        return Packages[Random.Range(0, Packages.Length)];
    }
    
    // Update is called once per frame
    void Update()
    {
        ScoreText.text = Score.ToString("F0");
        PackageText.text = PackageDeliverd.ToString();
    }
}
