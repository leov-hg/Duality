using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InputType{GetKey, GetKeyDown}

public class PlayerBase : MonoBehaviour
{
    [Separator("References", true)]
    [SerializeField] private PlayerRef playerRef;
    [SerializeField] protected Rigidbody rb;

    [Separator("Settings", true)]
    [SerializeField] private float rotationSmoothSpeed = 1;
    [SerializeField] private float speed = 1;
    [SerializeField] private InputType inputType;
    [Space(10)]
    [SerializeField] private LayerMask groundLayer;

    protected float _currentSpeed = 0;

    private Vector3 _direction;
    private Vector3 _targetView;
    private Camera _mainCam;
    private Ray _camRay;

    private void Awake()
    {
        _mainCam = Camera.main;
        playerRef.playerBase = this;

        _currentSpeed = speed;
    }
    protected List<Rigidbody> _detectedObjects = new List<Rigidbody>();
    protected List<Collider> _detectedCollider = new List<Collider>();


    private void Update()
    {
        _direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;

        if (inputType == InputType.GetKeyDown && Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
        else if (inputType == InputType.GetKey && Input.GetKey(KeyCode.E))
        {
            Interact();
            _camRay = _mainCam.ScreenPointToRay(Input.mousePosition + Vector3.forward);
            if (Physics.Raycast(_camRay, out RaycastHit hitInfo, 100, groundLayer))
            {
                _targetView = hitInfo.point;
            }

            if (rb.velocity.magnitude > 0)
            {
                rb.rotation = Quaternion.Lerp(rb.rotation,
                    Quaternion.LookRotation((_targetView - rb.position).normalized),
                    Time.deltaTime * rotationSmoothSpeed);
            }
        }
    }

    private void FixedUpdate()
    {
        rb.AddForce(_direction * speed, ForceMode.Acceleration);
    }

    virtual protected void Interact()
    {
    }

    virtual protected void ScanForObjects()
    {
        
    }
}
