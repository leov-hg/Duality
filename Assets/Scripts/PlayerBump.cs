using System;
using UnityEngine;

public class PlayerBump : PlayerBase
{
    [SerializeField] private float ejectForce;
    [SerializeField] private float cooldown;

    private float _timeCpt;

    protected override void Update()
    {
        base.Update();

        _timeCpt += Time.deltaTime;
    }

    protected override void Interact()
    {
        if (_timeCpt >= cooldown)
        {
            base.Interact();

            foreach (PhysicsHandler obj in _detectedObjects)
            {
                obj.ApplyBumpForce(obj.transform.position - transform.position, ejectForce);
            }

            _timeCpt = 0;
        }
    }
}