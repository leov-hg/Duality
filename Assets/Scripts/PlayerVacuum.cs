using UnityEngine;

public class PlayerVacuum : PlayerBase
{
    [SerializeField] private float vacuumForce;

    protected override void Interact()
    {
        base.Interact();
        
        foreach (PhysicsHandler obj in _detectedObjects)
        {
            obj.GetComponent<Rigidbody>().AddForce((transform.position - obj.transform.position).normalized * vacuumForce);
        }
    }
}