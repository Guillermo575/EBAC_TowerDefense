using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class _Enemy : MonoBehaviour
{
    public GameObject objetivo;
    internal int vidaMaxima = 100;
    public int vida = 100;
    public int damage = 40;
    internal Animator animator;
    internal virtual void Start()
    {
        vidaMaxima = vida;
        GetComponent<NavMeshAgent>().SetDestination(objetivo.transform.position);
        animator = GetComponent<Animator>();
        animator.SetBool("IsMoving", true);
    }
    internal virtual void Update()
    {
    }
    internal virtual void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Objetivo")
        {
            animator.SetBool("IsMoving", false);
            animator.SetTrigger("OnObjectiveReached");
        }
    }
    public void MakeDamage()
    {
        if (objetivo == null) return;
        objetivo?.GetComponent<SceneObjective>().ReceiveDamage(damage);
    }
    public void ReceiveDamage(int damage)
    {
        vida -= damage;
    }
}