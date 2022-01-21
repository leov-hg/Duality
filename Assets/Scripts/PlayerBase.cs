using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InputType{GetKey, GetKeyDown}

public class PlayerBase : MonoBehaviour
{
    [Separator("References", true)]
    [SerializeField] private Rigidbody rb;

    [Separator("Settings", true)]
    [SerializeField] private float speed = 1;
    [SerializeField] private InputType inputType;

    private Vector3 _direction;


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
        }
    }

    private void FixedUpdate()
    {
        rb.AddForce(_direction * speed, ForceMode.Acceleration);
    }

    virtual protected void Interact()
    {
        
    }
}