using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class CharacterBoss : MonoBehaviour
{
    public GameObject objetivo;
    public int vida = 100;
    private Animator animator;
    void Start()
    {
        GetComponent<NavMeshAgent>().SetDestination(objetivo.transform.position);
        animator = GetComponent<Animator>();
        animator.SetBool("IsMoving", true);
    }
    void Update()
    {
    }
    private void OnCollisionEnter(Collision collision)
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