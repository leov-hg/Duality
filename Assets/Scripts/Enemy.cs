using DG.Tweening;
using MyBox;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Separator("References", true)]
    [SerializeField] private PlayerRef[] players;
    [SerializeField] private GameObject meshGO;
    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Animator animator;

    [Separator("Settings", true)]
    [SerializeField] private float retargettingDelay = 0.2f;
    [SerializeField] private float speed = 10;
    [SerializeField] private float velocityAanimFactor = 30;
    [SerializeField] private float rotationSmoothSpeed = 10;

    //private Sequence sequence;
    private List<PlayerBase> activePlayers = new List<PlayerBase>();
    private Vector3[] navigationPoints;
    private Vector3 nextNavigationPoint;

    private void Awake()
    {
        meshGO.SetActive(false);
        navMeshAgent.enabled = false;
        rb.detectCollisions = false;
        rb.isKinematic = true;

        //sequence = DOTween.Sequence();
        //sequence.Append().SetLoops(-1, LoopType.Restart);

        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].playerBase)
            {
                activePlayers.Add(players[i].playerBase);
            }
        }
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

        animator.SetFloat("Velocity", rb.velocity.magnitude / velocityAanimFactor);

        rb.rotation = Quaternion.Lerp(rb.rotation, Quaternion.LookRotation(navMeshAgent.destination - rb.position).normalized, Time.deltaTime * rotationSmoothSpeed);
    }

    private void FixedUpdate()
    {
        if (!meshGO.activeSelf)
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
