using UnityEngine;

public class PlayerVacuum : PlayerBase
{
    [SerializeField] private float vacuumForce;

    protected override void Interact()
    {
        base.Interact();

        foreach (PhysicsHandler obj in _detectedObjects)
        {
            obj.ApplyVacuumForce(transform.position - obj.transform.position, vacuumForce);
        }
    }
}