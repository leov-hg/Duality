using System;
using UnityEngine;

public class PlayerBump : PlayerBase
{
    [SerializeField] private float ejectForce;


    protected override void Interact()
    {
        base.Interact();

        foreach (PhysicsHandler obj in _detectedObjects)
        {
            obj.GetComponent<Rigidbody>().AddForce((obj.transform.position - transform.position).normalized * (ejectForce / obj.bumpWeight), ForceMode.VelocityChange);
        }
    }
}