using System;
using MyBox;
using UnityEngine;

public class PhysicsHandler : MonoBehaviour
{
    public float bumpWeight = 1;
    public float vacuumWeight = 1;
    public float bumpUpliftModifier;

    private Rigidbody _rb;
    public Rigidbody Rb => _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void ApplyBumpForce(Vector3 forceDirection, float ejectForce)
    {
        _rb.AddForce(forceDirection.SetY(bumpUpliftModifier).normalized * (ejectForce / bumpWeight), ForceMode.VelocityChange);
    }

    public void ApplyVacuumForce(Vector3 forceDirection, float vacuumforce)
    {
        _rb.AddForce(forceDirection.normalized * (vacuumforce / vacuumWeight), ForceMode.Acceleration);
    }
}