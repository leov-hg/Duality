using System;
using DG.Tweening;
using MyBox;
using UnityEngine;

public class PlayerBump : PlayerBase
{
    [SerializeField] private float ejectForce;
    [SerializeField] private float cooldown;
    [SerializeField] private float detectionAngle = 15;
    [SerializeField] private SphereCollider detectionCollider;
    [SerializeField] private ParticleSystem bumpEffect;
    [SerializeField] private Material bumpConeMat;

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
                if (Mathf.Abs(Vector3.Angle(upperBody.transform.forward, (obj.transform.position.SetY(0) - upperBody.transform.position.SetY(0)).normalized)) < detectionAngle / 2)
                {
                    obj.ApplyBumpForce(obj.transform.position - transform.position, ejectForce);
                }
            }

            _timeCpt = 0;

            bumpConeMat.SetVector("_MainTex_ST", new Vector4(1, 1, 1, 0));
            bumpConeMat.DOKill();
            bumpConeMat.DOVector(new Vector4(1, 1, -1, 0), "_MainTex_ST", .2f);
            bumpEffect.Play();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;

        Gizmos.matrix = upperBody.localToWorldMatrix;

        Vector3 leftPoint = new Vector3(Mathf.Sin((-detectionAngle / 2) * Mathf.Deg2Rad), 0, Mathf.Cos((-detectionAngle / 2) * Mathf.Deg2Rad)) * detectionCollider.radius;
        Vector3 rightPoint = new Vector3(Mathf.Sin((detectionAngle / 2) * Mathf.Deg2Rad), 0, Mathf.Cos((detectionAngle / 2) * Mathf.Deg2Rad)) * detectionCollider.radius;

        Gizmos.DrawLine(Vector3.zero, leftPoint);
        Gizmos.DrawLine(Vector3.zero, rightPoint);
    }
}