using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : InteractableObject
{
    [SerializeField] private float killingStrength = 1;
    [SerializeField] private float explosionRadius = 1;
    [SerializeField] private float explosionStrength = 1;
    [SerializeField] private Transform bombMesh;
    [SerializeField] private ParticleSystem explosionFX;
    [SerializeField] private Rigidbody rb;

    private Tween scaleTween;
    private Collider[] collidersDetected;
    private bool activated = false;


    protected override void PhysicsHandler_onBumped()
    {
        base.PhysicsHandler_onBumped();
        Activation();
    }

    protected override void PhysicsHandler_onVacuumed()
    {
        base.PhysicsHandler_onVacuumed();
    }

    private void Explosion()
    {
        scaleTween.Kill();

        collidersDetected = Physics.OverlapSphere(transform.position, explosionRadius);

        for (int i = 0; i < collidersDetected.Length; i++)
        {
            collidersDetected[i].GetComponentInParent<PhysicsHandler>()?.ApplyExplosionForce((collidersDetected[i].transform.position - transform.position).normalized, explosionStrength);
            //if (collidersDetected[i].GetComponentInParent<Enemy>())
            //{
            //    collidersDetected[i].GetComponentInParent<Enemy>().Death((collidersDetected[i].transform.position - transform.position).normalized, explosionStrength);
            //}
        }

        rb.isKinematic = true;
        rb.detectCollisions = false;
        bombMesh.gameObject.SetActive(false);
        transform.rotation = Quaternion.identity;
        explosionFX.Play();
    }

    protected override void TouchedByEnemy()
    {
        base.TouchedByEnemy();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (/*collision.gameObject.CompareTag("Player") || */collision.gameObject.CompareTag("Enemy"))
        {
            Explosion();
        }
    }

    private void Activation()
    {
        if (activated)
        {
            return;
        }

        activated = true;
        scaleTween = bombMesh.DOPunchScale(Vector3.one * 0.5f, 0.5f, 1, 1).SetLoops(4, LoopType.Restart);
        scaleTween.onComplete += Explosion;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, .1f);

        Gizmos.DrawSphere(transform.position, explosionRadius);
    }
}
