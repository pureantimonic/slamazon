using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Package : MonoBehaviour
{
    private GameObject packageIcon;
    private GameObject destinationIcon;

    [SerializeField] private float iconMargin;
    [SerializeField] private float minVelocity = 0.1f;
    [SerializeField] private GameObject deliveredEffect;
    [SerializeField] private LayerMask groundLayer;
    private GameObject m_BoxIcon;
    private Rigidbody rbody;
    private bool waitingForRest;
    private bool waitingForContact;
    
    

    [SerializeField] private float velocityThreshold;
    [SerializeField] private float baseProbability = 1f;
    [SerializeField] private float specialChance = 0.1f;
    private int packageType;
    private AudioSource audioSource;
    private AudioSource baseAudioSource;
    
    public void Start()
    {
        rbody = GetComponent<Rigidbody>();
        var audioSources = GetComponents<AudioSource>();
        audioSource = audioSources[0];
        baseAudioSource = audioSources[1];
        
        iconCanvas = Global.Instance.iconCanvas;

        packageIcon = gameObject.transform.Find("PackageIcon").gameObject;
        destinationIcon = gameObject.transform.Find("DestinationIcon").gameObject;
        
        packageIcon.transform.SetParent(iconCanvas.transform, false);
        destinationIcon.transform.SetParent(iconCanvas.transform, false);

        Color color = UnityEngine.Random.ColorHSV(0f, 1f, 0.8f, 0.8f, 0.8f, 0.8f);
        packageIcon.GetComponent<Image>().color = color;
        destinationIcon.GetComponent<Image>().color = color;

        if (Random.value < specialChance)
        {
            packageType = Random.Range(0, Global.Instance.collisionClips.Count);
        }
        else
        {
            packageType = -1;
        }
    }

    public void UpdateIcon(Vector3 position, GameObject obj)
    {
        var screenPoint = Camera.main.WorldToScreenPoint(position);
        var canvasTransform = iconCanvas.transform as RectTransform;

        var w = canvasTransform.rect.width;
        var h = canvasTransform.rect.height;

        if (screenPoint.z < 0)
        {
            screenPoint.x = Mathf.Sign(screenPoint.x) * Mathf.NegativeInfinity;
        }

        obj.transform.position = new Vector3(
            Mathf.Clamp(screenPoint.x, w * iconMargin, w * (1f - iconMargin)),
            h * 0.9f);
    }

    public void Update()
    {
        UpdateIcon(this.transform.position, packageIcon);
        UpdateIcon(this.destination, destinationIcon);
    }

    public void OnDestroy()
    {
        Destroy(packageIcon);
        Destroy(destinationIcon);
    }

    [SerializeField] public Vector3 anchorPoint;


    public void OnPickedUp()
    {
        waitingForContact = false;

        waitingForRest = false;
        if (pl)
        {
            pl.OnPackagePickedUp();
            pl = null;
        }
    }

    public void OnRelease()
    {
        //waitingForRest = true;
        waitingForContact = true;
        waitingForRest = true;
    }

    public void OnRest()
    {
        float distanceToDest = Vector3.Distance(transform.position, destination);
        if (distanceToDest > 20)
        {
            return;
        }
        Destroy(m_BoxIcon);
        Global.Instance.AddScore((20 - distanceToDest) / (10/20));
        Global.Instance.AddPackage();
        Destroy(GameObject.Instantiate(deliveredEffect, transform.position, Quaternion.identity), 2);
        Destroy(gameObject);
    }

    private IEnumerator WaitForMotion(float t)
    {
        float _t = 0;
        while (_t < t)
        {
            _t += Time.fixedDeltaTime;
            if (rbody.velocity.magnitude > minVelocity)
            {
                waitingForRest = true;
                break;
            }
            yield return new WaitForFixedUpdate();
        }
        if (!waitingForRest)
        {
            OnRest();
        }
        
    }

    private void FixedUpdate()
    {
        if (waitingForRest)
        {
            if (rbody.velocity.magnitude < minVelocity)
            {
                waitingForRest = false;
                StartCoroutine(WaitForMotion(0.5f));
            }
        }
    }
    
    public void OnCollisionEnter(Collision other)
    {
        if (waitingForContact && (other.collider.gameObject.layer== 9))
        {
            OnRest();
        }
        if (packageType > -1 && other.relativeVelocity.magnitude > velocityThreshold && !audioSource.isPlaying)
        {
            var choices = Global.Instance.collisionClips[packageType].innerList;
            audioSource.clip = choices[Random.Range(0, choices.Count)];
            audioSource.Play();
        }
        
        // regular box noise
        if (Random.value < baseProbability)
        {
            baseAudioSource.pitch = Random.Range(0.6f, 1.4f);
            baseAudioSource.PlayOneShot(
                Global.Instance.baseCollisionClips[Random.Range(0, Global.Instance.baseCollisionClips.Count)],
                0.5f
            );
        }
        
    }

    public PickupLocation pl;
    public Vector3 destination;
    public MeshRenderer mainMesh;
    public GameObject iconCanvas;

}
