using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class _Enemy : MonoBehaviour
{
    public GameObject objetivo;
    public int vida = 100;
    internal Animator animator;
    internal virtual void Start()
    {
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
        objetivo?.GetComponent<SceneObjective>().ReceiveDamage(40);
    }
    public void ReceiveDamage(int damage)
    {
        vida -= damage;
    }
}