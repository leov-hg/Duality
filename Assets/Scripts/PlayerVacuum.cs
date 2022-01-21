using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerVacuum : PlayerBase
{
    [SerializeField] private float vacuumForce;
    [SerializeField] private Vector3 detectionBoxSize;
    
    protected override void Interact()
    {
        ScanForObjects();

        foreach (Rigidbody obj in _detectedObjects)
        {
            obj.AddForce((transform.position - obj.transform.position).normalized * vacuumForce);
        }
    }
    
    protected override void ScanForObjects()
    {
        _detectedCollider = Physics
            .OverlapBox(transform.position + (transform.forward * detectionBoxSize.z), detectionBoxSize).ToList();
        foreach (Collider col in _detectedCollider)
        {
            Rigidbody rb = col.GetComponent<Rigidbody>();
            if (rb != null)
                _detectedObjects.Add(rb);
        }
    }
}