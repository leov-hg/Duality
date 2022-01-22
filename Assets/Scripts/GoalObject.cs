using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalObject : MonoBehaviour
{
    [SerializeField] private Transform goalUI;

    private void Update()
    {
        goalUI.LookAt(GameManager.Instance.endModule.slabPoint);
    }
}
