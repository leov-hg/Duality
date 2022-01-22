using System;
using MyBox;
using UnityEngine;

public class PlayerBump : PlayerBase
{
    [SerializeField] private float ejectForce;
    [SerializeField] private float cooldown;
    [SerializeField] private float detectionAngle = 15;

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
                if (Mathf.Abs(Vector3.Angle(transform.forward, (obj.transform.position - transform.position).normalized)) < detectionAngle)
                {
                    obj.ApplyBumpForce(obj.transform.position - transform.position, ejectForce);
                }
            }

            _timeCpt = 0;
        }
    }
}