using System;
using UnityEngine;

public class PlayerBump : PlayerBase
{
    [SerializeField] private float ejectForce;
    [SerializeField] private float ejectionRadius;


    protected override void Interact()
    {
        base.Interact();

        foreach (PhysicsHandler obj in _detectedObjects)
        {
            obj.GetComponent<Rigidbody>().AddExplosionForce(ejectForce, transform.position, ejectionRadius, 0, ForceMode.Impulse);
            obj.GetComponent<Rigidbody>().AddForce((obj.transform.position - transform.position).normalized * (ejectForce / obj.bumpWeight));
        }
    }
}