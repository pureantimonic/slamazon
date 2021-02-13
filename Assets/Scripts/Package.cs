using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Package : MonoBehaviour
{
    [SerializeField] private GameObject iconCanvas;
    [SerializeField] private GameObject boxIcon;
    [SerializeField] private float iconMargin;
    [SerializeField] private float minVelocity = 0.1f;
    private GameObject m_BoxIcon;
    private Rigidbody rbody;
    private bool waitingForRest;
    
    public void Start()
    {
        rbody = GetComponent<Rigidbody>();
        iconCanvas = Global.Instance.iconCanvas;
        m_BoxIcon = Instantiate(boxIcon, iconCanvas.transform, false) as GameObject;
        m_BoxIcon.GetComponent<Image>().color = UnityEngine.Random.ColorHSV(0f, 1f, 0.8f, 1.0f, 0.5f, 1.0f);
    }
    
    public void Update()
    {
        var screenPoint = Camera.main.WorldToScreenPoint(this.transform.position);
        var canvasTransform = iconCanvas.transform as RectTransform;
        

        var width = canvasTransform.rect.width;

        if (screenPoint.z < 0)
        {
            screenPoint.x = Mathf.Sign(screenPoint.x) * Mathf.NegativeInfinity;
        }

        m_BoxIcon.transform.position = new Vector3(
            Mathf.Clamp(screenPoint.x, width * iconMargin, width * (1f - iconMargin)),
            canvasTransform.rect.height * 0.9f
        );
        
        
        
    }

    public void OnDestroy()
    {
      //  Destroy(m_BoxIcon);
    }

    [SerializeField] public Vector3 anchorPoint;


    public void OnPickedUp()
    {
        m_BoxIcon.SetActive(false);
        waitingForRest = false;
        if (pl)
        {
            pl.OnPackagePickedUp();
            pl = null;
        }
    }

    public void OnRelease()
    {
        m_BoxIcon.SetActive(true);
        waitingForRest = true;
    }

    public void OnRest()
    {
        Debug.Log("Package Delivered");
        Destroy(m_BoxIcon);
        Global.Instance.AddScore(10 - Vector3.Distance(transform.position, destination));
        Global.Instance.AddPackage();
        Destroy(gameObject);
    }

    private void FixedUpdate()
    {
        if (waitingForRest)
        {
            if (rbody.velocity.magnitude < minVelocity)
            {
                waitingForRest = false;
                OnRest();
            }
        }
    }

    public PickupLocation pl;
    public Vector3 destination;
    public MeshRenderer mainMesh;

}
