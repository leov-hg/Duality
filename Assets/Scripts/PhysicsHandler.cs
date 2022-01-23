using System;
using UnityEngine;

public class PhysicsHandler : MonoBehaviour
{
    public float bumpWeight = 1;
    public float vacuumWeight = 1;

    public bool active = true;

    private Rigidbody _rb;
    public Rigidbody Rb => _rb;

    public event Action onBumped;
    public event Action onVacuumed;
    public event Action<Vector3, float> onExploded;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void ApplyBumpForce(Vector3 forceDirection, float ejectForce)
    {
        if (!active) return;

        _rb.AddForce(forceDirection.normalized * (ejectForce / bumpWeight), ForceMode.VelocityChange);
        onBumped?.Invoke();
    }

    public void ApplyExplosionForce(Vector3 forceDirection, float ejectForce)
    {
        if (!active) return;

        //_rb.AddForce(forceDirection.normalized * (ejectForce / bumpWeight), ForceMode.VelocityChange);
        onExploded?.Invoke(forceDirection, ejectForce);
    }

    public void ApplyVacuumForce(Vector3 forceDirection, float vacuumforce)
    {
        if (!active) return;

        _rb.AddForce(forceDirection.normalized * (vacuumforce / vacuumWeight), ForceMode.Acceleration);
    }
}