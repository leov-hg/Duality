using System;
using DG.Tweening;
using MyBox;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public Action<Enemy> onDeath;
    
    [Separator("References", true)]
    [SerializeField] private PlayerRef[] players;
    [SerializeField] private GameObject meshGO;
    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Animator animator;
    [SerializeField] private PhysicsHandler physicsHandler;

    [Separator("Settings", true)]
    [SerializeField] private float retargettingDelay = 0.2f;
    [SerializeField] private float speed = 10;
    [SerializeField] private float velocityAanimFactor = 30;
    [SerializeField] private float rotationSmoothSpeed = 10;

    private List<PlayerBase> activePlayers = new List<PlayerBase>();
    private Vector3[] navigationPoints;
    private Vector3 nextNavigationPoint;
    private List<Rigidbody> ragdollRBs = new List<Rigidbody>();

    private bool stuned = false;
    private float velocityAnimSmooth = 0;


    private void Awake()
    {
        meshGO.SetActive(false);
        navMeshAgent.enabled = false;
        rb.detectCollisions = false;
        rb.isKinematic = true;

        ragdollRBs = GetComponentsInChildren<Rigidbody>().ToList();
        ragdollRBs.Remove(rb);

        for (int i = 0; i < ragdollRBs.Count; i++)
        {
            ragdollRBs[i].isKinematic = true;
        }

        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].playerBase)
            {
                activePlayers.Add(players[i].playerBase);
            }
        }

        physicsHandler.onBumped += PhysicsHandler_onBumped;
    }

    private void PhysicsHandler_onBumped()
    {
        animator.ResetTrigger("Knockback");
        animator.SetTrigger("Knockback");
    }

    public void KnockbackStart()
    {
        //animator.applyRootMotion = true;
        stuned = true;
    }

    public void KnockbackEnd()
    {
        animator.applyRootMotion = false;
        stuned = false;
    }

    public void Spawn(Vector3 spawnPos)
    {
        transform.position = spawnPos;
        meshGO.SetActive(true);
        navMeshAgent.enabled = true;
        rb.detectCollisions = true;
        rb.isKinematic = false;

        DOVirtual.DelayedCall(retargettingDelay, RefreshTargetting).SetLoops(-1, LoopType.Restart).SetDelay(retargettingDelay);
    }

    private void Update()
    {
        if (!meshGO.activeSelf)
        {
            return;
        }

        velocityAnimSmooth = Mathf.Lerp(velocityAnimSmooth, rb.velocity.magnitude / velocityAanimFactor, Time.deltaTime * 10);
        animator.SetFloat("Velocity", velocityAnimSmooth);

        rb.rotation = Quaternion.Lerp(rb.rotation, Quaternion.LookRotation(navMeshAgent.destination.SetY(0) - rb.position.SetY(0)).normalized, Time.deltaTime * rotationSmoothSpeed);
    }

    private void FixedUpdate()
    {
        if (!meshGO.activeSelf || stuned)
        {
            return;
        }

        rb.AddForce((nextNavigationPoint - rb.position).normalized * speed, ForceMode.Acceleration);
    }

    public void RefreshTargetting()
    {
        navMeshAgent.transform.position = rb.position;
        navMeshAgent.SetDestination(activePlayers.GetRandom().transform.position);
        navigationPoints = navMeshAgent.path.GetPointsOnPath(1).ToArray();

        for (int i = navigationPoints.Length - 1; i >= 0; i--)
        {
            if (Vector3.SqrMagnitude(navigationPoints[i] - rb.position) < 2 * 2)
            {
                nextNavigationPoint = navigationPoints[i];
                break;
            }
        }
    }

    [ButtonMethod()]
    public void Death()
    {
        onDeath?.Invoke(this);
        animator.enabled = false;
        for (int i = 0; i < ragdollRBs.Count; i++)
        {
            ragdollRBs[i].isKinematic = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        if (navigationPoints == null)
        {
            return;
        }

        for (int i = 0; i < navigationPoints.Length; i++)
        {
            Gizmos.DrawSphere(navigationPoints[i], .2f);
        }
    }
}
