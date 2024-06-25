using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class _Enemy : MonoBehaviour, IAtacante, IAtacable
{
    internal Animator animator;
    internal int vidaMaxima = 100;
    internal GameManager gameManager;
    private GameObject objetivo;
    public int vida = 100;
    public int damage = 40;
    public int recursosGanados = 200;
    private void OnEnable()
    {
        gameManager = GameManager.GetManager();
        objetivo = gameManager.referenciaObjetivo.gameObject;
        objetivo.GetComponent<SceneObjective>().EnObjetivoDestruido += Detener;
    }
    private void OnDisable()
    {
        try
        {
            objetivo.GetComponent<SceneObjective>().EnObjetivoDestruido -= Detener;
        } catch
        {

        }
    }
    internal virtual void Start()
    {
        vidaMaxima = vida;
        animator = GetComponent<Animator>();
        animator.SetBool("IsMoving", true);
        GetComponent<NavMeshAgent>().SetDestination(objetivo.transform.position);
    }
    internal virtual void Update()
    {
        if (vida <= 0)
        {
            Death();
        }
    }
    public virtual void OnDestroy()
    {
        gameManager.ModificarRecursos(recursosGanados);
    }
    internal virtual void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Objetivo")
        {
            animator.SetBool("IsMoving", false);
            animator.SetTrigger("OnObjectiveReached");
        }
    }
    public void MakeDamage(int damage = 0)
    {
        if (objetivo == null) return;
        damage = damage <= 0 ? this.damage : damage;
        objetivo?.GetComponent<SceneObjective>().ReceiveDamage(damage);
    }
    public void ReceiveDamage(int damage)
    {
        vida -= damage;
    }
    public void Death()
    {
        animator.SetTrigger("OnDeath");
        GetComponent<NavMeshAgent>().SetDestination(transform.position);
        Destroy(gameObject, 3);
    }
    public void Detener()
    {
        animator.SetTrigger("OnObjectiveDestroyed");
        GetComponent<NavMeshAgent>().SetDestination(transform.position);
    }
}