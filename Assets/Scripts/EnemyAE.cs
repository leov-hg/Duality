using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAE : MonoBehaviour
{
    [SerializeField] private Enemy enemy;

    public void KnockbackStart()
    {
        enemy.KnockbackStart();
    }

    public void KnockbackEnd()
    {
        enemy.KnockbackEnd();
    }
}
