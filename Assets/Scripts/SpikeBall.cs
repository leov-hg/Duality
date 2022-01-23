using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeBall : InteractableObject
{
    [SerializeField] private float killingStrength = 1;
    private Enemy _tempEnemy;

    protected override void PhysicsHandler_onBumped()
    {
        base.PhysicsHandler_onBumped();
    }

    protected override void PhysicsHandler_onVacuumed()
    {
        base.PhysicsHandler_onVacuumed();
    }

    protected override void TouchedByEnemy()
    {
        base.TouchedByEnemy();
    }

    private void OnCollisionEnter(Collision collision)
    {
        _tempEnemy = collision.gameObject.GetComponentInParent<Enemy>();

        if (_tempEnemy)
        {
            _tempEnemy.Death(transform.position, killingStrength);
            TouchedByEnemy();
        }
    }
}
