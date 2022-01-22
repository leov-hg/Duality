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
           obj.ApplyBumpForce(obj.transform.position - transform.position, ejectForce);
        }
    }
}