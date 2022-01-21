using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerBump : PlayerBase
{
    [SerializeField] private float ejectForce;
    [SerializeField] private float ejectionRadius;

    private List<Rigidbody> _detectedObjects = new List<Rigidbody>();
    private List<Collider> _detectedCollider = new List<Collider>();

    protected override void Interact()
    {
        ScanObjectsAround();

        foreach (Rigidbody obj in _detectedObjects)
        {
            obj.AddExplosionForce(ejectForce, transform.position, ejectionRadius, 0, ForceMode.Impulse);
        }
    }

    private void ScanObjectsAround()
    {
        _detectedCollider = Physics.OverlapSphere(transform.position, ejectionRadius).ToList();
        foreach (Collider col in _detectedCollider)
        {
            Rigidbody rb = col.GetComponent<Rigidbody>();
            if (rb != null)
                _detectedObjects.Add(rb);
        }
    }
}