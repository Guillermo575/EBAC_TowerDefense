using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
/**
 * @file
 * @brief Clase que usa los enemigos jefes
 */
public class EnemyBoss : _Enemy
{
    internal override void Start()
    {
        base.Start();
    }
    internal override void Update()
    {
        base.Update();
        GetComponent<NavMeshAgent>().speed = vida < vidaMaxima / 2 ? 1f : 2;
    }
    internal override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
    }
    public override void OnDestroy()
    {
        base.OnDestroy();
        gameManager.AddEnemigosJefeDerrotados();
    }
}