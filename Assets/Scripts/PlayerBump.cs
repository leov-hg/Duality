using System.Linq;
using UnityEngine;

public class PlayerBump : PlayerBase
{
    [SerializeField] private float ejectForce;
    [SerializeField] private float ejectionRadius;

    protected override void Interact()
    {
        ScanForObjects();

        foreach (Rigidbody obj in _detectedObjects)
        {
            obj.AddExplosionForce(ejectForce, transform.position, ejectionRadius, 0, ForceMode.Impulse);
        }
    }

    protected override void ScanForObjects()
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