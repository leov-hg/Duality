using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerBump : PlayerBase
{
    [SerializeField] private float ejectForce;
    [SerializeField] private float ejectionRadius;

    private List<Rigidbody> _detectedObjects;
    private List<Collider> _detectedCollider;

    protected override void Interact()
    {
        ScanObjectsAround();

        foreach (Rigidbody obj in _detectedObjects)
        {
            obj.AddForce((obj.transform.position - transform.position).normalized * ejectForce);
        }
    }

    private void ScanObjectsAround()
    {
        _detectedCollider = Physics.OverlapSphere(transform.position, ejectionRadius).ToList();
        foreach (Collider col in _detectedCollider)
        {
            _detectedObjects.Add(col.GetComponent<Rigidbody>());
        }
    }
}