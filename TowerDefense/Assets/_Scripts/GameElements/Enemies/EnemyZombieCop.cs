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
        GetComponent<NavMeshAgent>().speed = vida < vidaMaxima / 2 ? 0.5f : 2;
    }
    internal override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
    }
    public override void OnDestroy()
    {
        base.OnDestroy();
        gameManager.AddEnemigosBaseDerrotados();
    }
}