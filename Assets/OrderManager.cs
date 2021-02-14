using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class OrderManager : MonoBehaviour
{
    [SerializeField] private float orderTime;
    [SerializeField] private GameObject orderUIPrefab;
    [SerializeField] private Transform orderUIParent;
    
    public class Order
    {
        public OrderManager om;
        public Destination destination;
        public Color color;
        public float startTime;
        public OrderUI UI;
        public bool spawned;
        public GameObject person;
        public GameObject package;
    }

    public List<Order> ongoingOrders;
    // Start is called before the first frame update
    void Start()
    {
        ongoingOrders = new List<Order>();
        StartCoroutine(StartOrderInTime(1));
        StartCoroutine(StartOrderInTime(2));
        StartCoroutine(ContinuousOrders());
    }

    public IEnumerator StartOrderInTime(float t)
    {
        yield return new WaitForSeconds(t);
        StartRandomOrder();
        
    }

    public IEnumerator ContinuousOrders()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(10,15));
            if (ongoingOrders.Count < 5)
            {
                StartRandomOrder();
            }
        }
    }

    public void CompleteOrder(Order ord)
    {
        Destroy(ord.person);
        ord.UI.OnComplete();
        ongoingOrders.Remove(ord);
    }

    public void StartRandomOrder()
    {
        Order newOrder = new Order();
        newOrder.om = this;
        newOrder.spawned = false;
        newOrder.destination = Global.Instance.GetRandomDestination();
        newOrder.startTime = Time.time;
        newOrder.color = UnityEngine.Random.ColorHSV(0f, 1f, 0.8f, 1, 0.9f, 0.9f);
        newOrder.person = GameObject.Instantiate(Global.Instance.GetRandomPerson());
        newOrder.person.transform.position = newOrder.destination.destinationPoint.position;
        newOrder.person.transform.rotation = newOrder.destination.destinationPoint.rotation;
        GameObject uiOrder = GameObject.Instantiate(orderUIPrefab, orderUIParent);
        newOrder.UI = uiOrder.GetComponent<OrderUI>();
        newOrder.UI.SetColor(newOrder.color);
        foreach (PickupLocation pl in Global.Instance.depots)
        {
            if (pl.CanSpawnPackage())
            {
                newOrder.spawned = true;
                pl.SetOrderManager(this);
                pl.DoSpawnPackage(newOrder);
                break;
            }
        }
        ongoingOrders.Add(newOrder);
    }

    public void OnOpenLocation(PickupLocation pl)
    {
        foreach (Order ord in ongoingOrders)
        {
            if (!ord.spawned)
            {
                pl.SetOrderManager(this);
                pl.DoSpawnPackage(ord);
                break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < ongoingOrders.Count;i++)
        {
            Order ord = ongoingOrders[i];
            float timeRemainingNorm = (orderTime - (Time.time - ord.startTime)) / orderTime;
            ord.UI.SetTimeRemaining(timeRemainingNorm);
            if (timeRemainingNorm < 0)
            {
                if (ord.package)
                {
                    ord.package.GetComponent<Package>().DestroyPackage();
                }
                Destroy(ord.person);
                ord.UI.OnFail();
                ongoingOrders.RemoveAt(i);
            }
            
        }
    }
}
