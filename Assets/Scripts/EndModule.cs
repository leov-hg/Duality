using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndModule : MonoBehaviour
{
    public Transform slabPoint;


    private void Awake()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<GoalObject>())
        {
            GameManager.Instance.CompleteLevel();
        }
    }
}
