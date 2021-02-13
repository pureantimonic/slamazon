using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DroneController : MonoBehaviour
{
    [SerializeField] private float MaxAccel = 1;
    [SerializeField] private float Speed = 1;
    [SerializeField] private float AngularSpeed = 1;
    [SerializeField] private Transform modelTransform;
    [SerializeField] private float MaxAngAccel = 1;
    [SerializeField] private float MaxAngle = 15;
    private Quaternion initialRotation;
    private Vector2 latInput;
    private Quaternion targetRotation;
    private float targetYVelociy;
    private float vertInput;
    

    private Rigidbody rbody;
    // Start is called before the first frame update
    void Start()
    {
        targetRotation = transform.rotation;
        initialRotation = modelTransform.localRotation;
        rbody = GetComponent<Rigidbody>();
    }



    public void OnLook(InputValue val)
    {
        Vector2 lookInput = val.Get<Vector2>();
        Vector3 newEulers = new Vector3(0, lookInput.x, 0);
        //Vector3 axis;
        targetRotation = Quaternion.Euler(newEulers * AngularSpeed) * targetRotation;
        //Quaternion deltaRot = transform.rotation * Quaternion.Inverse(targetRotation);
        //float angle;
        //deltaRot.ToAngleAxis(out angle, out axis);
        //deltaRot = Quaternion.AngleAxis(Mathf.Min(angle, MaxAngle), axis);
        //targetRotation = deltaRot * transform.rotation;
    }

    public void OnMove(InputValue val)
    {
        latInput = val.Get<Vector2>();
    }

    public void OnUpDown(InputValue val)
    {
        vertInput = val.Get<float>();
    }
    
    

    public void FixedUpdate()
    {
        Vector3 targetVel = (transform.forward * latInput.y + transform.right * latInput.x + Vector3.up * vertInput) * Speed;
        rbody.velocity = Vector3.MoveTowards(rbody.velocity, targetVel, MaxAccel);
        //rbody.velocity = new Vector3(rbody.velocity.x, targetYVelociy, rbody.velocity.z);

        Vector3 localVelocity = transform.InverseTransformDirection(rbody.velocity);
        modelTransform.localRotation = Quaternion.Euler(localVelocity.z * 2 , 0,  -localVelocity.x * 2) * initialRotation;

        //transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, MaxAngAccel);
        
    }

    public void Update()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, MaxAngAccel * Time.deltaTime);
    }
    
}
