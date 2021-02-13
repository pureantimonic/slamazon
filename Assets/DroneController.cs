using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DroneController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float MaxAccel = 1;
    [SerializeField] private float Speed = 1;
    [SerializeField] private float AngularSpeed = 1;
    [SerializeField] private Transform modelTransform;
    [SerializeField] private float MaxAngAccel = 1;
    [SerializeField] private float MaxAngle = 15;

    [Header("Gameplay")] [SerializeField] private float pickupRange = 4;
    [SerializeField] private LayerMask packageLayer;
    [SerializeField] private Rigidbody attachBody;
    [SerializeField] private Transform camView;
    [SerializeField] private GameObject destinationEffect;
    private GameObject DestinationEffect;

    [Header("UI")] [SerializeField] private Image pickupHighlight;
    
    private Quaternion initialRotation;
    private Vector2 latInput;
    private Quaternion targetRotation;
    private float targetYVelociy;
    private float vertInput;
    private Package targetPackage;
    private Package currentPackage;
    private bool readyToFire;
    private float startFireTime;
    private float lookAngleY;

    private Rigidbody rbody;
    // Start is called before the first frame update
    void Start()
    {
        destinationEffect.SetActive(false);
        targetRotation = transform.rotation;
        initialRotation = modelTransform.localRotation;
        rbody = GetComponent<Rigidbody>();
        pickupHighlight.enabled = false;
    }


    

    public void OnLook(InputValue val)
    {
        Vector2 lookInput = val.Get<Vector2>();
        Vector3 newEulers = new Vector3(0, lookInput.x, 0);
        lookAngleY -= lookInput.y * AngularSpeed;
        lookAngleY = Mathf.Clamp(lookAngleY, -90, 90);
        targetRotation = Quaternion.Euler(newEulers * AngularSpeed) * targetRotation;
        
    }

    public void OnMove(InputValue val)
    {
        latInput = val.Get<Vector2>();
    }

    public void OnUpDown(InputValue val)
    {
        vertInput = val.Get<float>();
    }

    public void OnPickUp(InputValue val)
    {
        if (val.Get<float>() < 1)
            return;
        
        
        if (targetPackage && !currentPackage)
        {
            PickupPackage(targetPackage);
        }else if (currentPackage)
        {
            ReleasePackage();
            
        }
    }

    private void OnPrepFire(InputValue val)
    {
        readyToFire = val.Get<float>() > 0;
        if (readyToFire)
        {
            startFireTime = Time.time;
        }
        
    }

    private GameObject ReleasePackage()
    {
        Rigidbody packBody = currentPackage.GetComponent<Rigidbody>();
        packBody.drag = 0;
        packBody.angularDrag = 0.05f;
        Destroy(currentPackage.gameObject.GetComponent<ConfigurableJoint>());
        currentPackage = null;
        destinationEffect.SetActive(false);
        return packBody.gameObject;
    }

    private void OnFire(InputValue val)
    {
        if (!readyToFire)
            return;
        float power = (Time.time - startFireTime);
        GameObject package = ReleasePackage();
        Rigidbody packBody = package.GetComponent<Rigidbody>();
        packBody.AddForce(transform.forward * power * 10, ForceMode.Impulse);


    }

    private void PickupPackage(Package package)
    {
        destinationEffect.SetActive(true);
        destinationEffect.transform.position = package.Destination;
        currentPackage = package;
        //package.transform.position = attachBody.transform.position;
        ConfigurableJoint cfj = package.gameObject.AddComponent<ConfigurableJoint>();
        cfj.anchor = package.anchorPoint;
        cfj.connectedBody = attachBody;
        cfj.autoConfigureConnectedAnchor = false;
        cfj.connectedAnchor = Vector3.zero;
        cfj.xMotion = ConfigurableJointMotion.Limited;
        cfj.yMotion = ConfigurableJointMotion.Limited;
        cfj.zMotion = ConfigurableJointMotion.Limited;
        cfj.linearLimit =  new SoftJointLimit {limit = 0.4f};
        Rigidbody packBody = package.GetComponent<Rigidbody>();
        packBody.drag = 5;
        packBody.angularDrag = 0.1f;
        
    }
    
    
    // from jokigenki on stackoverflow : )
    /// <summary>
    /// Returns a rectangle on this canvas that fully encloses the given GameObject.
    /// </summary>
    /// <param name="canvas">The canvas on which to base the rectangle</param>
    /// <param name="go">The game object to enclose</param>
    /// <returns>A Rect that encloses the GameObject</returns>
    public static Rect GUIRect( UnityEngine.Canvas canvas, GameObject go)
    {
        var canvasRT = canvas.transform as RectTransform;
        var camera = canvas.worldCamera;
        if (camera == null) camera = Camera.main;
        var renderer = go.GetComponent<Renderer>();
        if (camera == null || canvasRT == null || renderer == null) return Rect.zero;
        var bounds = renderer.bounds;
        var cen = bounds.center;
        var ext = bounds.extents;
        var extMin = cen - ext;
        var extMax = cen + ext;
        var extentPoints = new[]
        {
            new Vector3(extMax.x, extMin.y, extMin.z),
            new Vector3(extMin.x, extMin.y, extMax.z),
            new Vector3(extMax.x, extMin.y, extMax.z),
            new Vector3(extMin.x, extMax.y, extMin.z),
            new Vector3(extMax.x, extMax.y, extMin.z),
            new Vector3(extMin.x, extMax.y, extMax.z),
            extMax
        };
        var min = camera.WorldToScreenPoint(extMin);
        var max = min;
        foreach (var v3 in extentPoints)
        {
            var v = camera.WorldToScreenPoint(v3);
            min = Vector2.Min(min, v);
            max = Vector2.Max(max, v);
        }
        
        var sizeDelta = canvasRT.sizeDelta / 2f;
        return new Rect(min.x - sizeDelta.x, min.y - sizeDelta.y, max.x - min.x, max.y - min.y);
    }

    public void FixedUpdate()
    {
        Vector3 targetVel = (transform.forward * latInput.y + transform.right * latInput.x + Vector3.up * vertInput) * Speed;
        rbody.velocity = Vector3.MoveTowards(rbody.velocity, targetVel, MaxAccel);
        Vector3 localVelocity = transform.InverseTransformDirection(rbody.velocity);
        modelTransform.localRotation = Quaternion.Euler(localVelocity.z * 2 , 0,  -localVelocity.x * 2) * initialRotation;
    }

    public void Update()
    {
        camView.localRotation = Quaternion.Euler(lookAngleY, 0, 0);
        Collider[] colls = new Collider[5];
        if (!currentPackage)
        {
            int count = Physics.OverlapSphereNonAlloc(modelTransform.position, pickupRange, colls, packageLayer);
            if (count > 0)
            {
                Rect guiRect = GUIRect(pickupHighlight.canvas, colls[0].GetComponent<Package>().mainMesh.gameObject);
                pickupHighlight.rectTransform.localPosition = guiRect.center;
                pickupHighlight.rectTransform.sizeDelta = new Vector2(guiRect.width, guiRect.height);
                pickupHighlight.enabled = true;
                targetPackage = colls[0].gameObject.GetComponent<Package>();
            }
            else if(pickupHighlight.enabled)
            {
                pickupHighlight.enabled = false;
                targetPackage = null;
            }
        }else if(pickupHighlight.enabled)
        {
            pickupHighlight.enabled = false;
            targetPackage = null;
        }
        

        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, MaxAngAccel * Time.deltaTime);
    }
    
}
