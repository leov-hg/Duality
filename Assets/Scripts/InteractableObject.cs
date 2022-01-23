using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    [SerializeField] private PhysicsHandler physicsHandler;

    private void Awake()
    {
        physicsHandler.onBumped += PhysicsHandler_onBumped;
        physicsHandler.onVacuumed += PhysicsHandler_onVacuumed;
    }

    protected virtual void PhysicsHandler_onVacuumed()
    {
        
    }

    protected virtual void PhysicsHandler_onBumped()
    {
        
    }

    protected virtual void TouchedByEnemy()
    {

    }
}
