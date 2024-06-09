using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyZombieCop : _Enemy
{
    internal override void Start()
    {
        base.Start();
    }
    internal override void Update()
    {
        base.Update();
        animator.SetBool("IsHalfLife", vida < vidaMaxima/2);
        GetComponent<NavMeshAgent>().speed = vida < vidaMaxima / 2 ? 1 : 2;
    }
    internal override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
    }
}