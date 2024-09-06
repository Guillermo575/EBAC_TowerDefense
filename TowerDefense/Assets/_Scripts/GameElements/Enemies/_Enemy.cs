using System;
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
    private bool IsDead;
    public int vida = 100;
    public int damage = 40;
    public int recursosGanados = 200;
    private AudioSource SourceDisparo;
    public List<AudioClip> lstClipSpawn;
    public List<AudioClip> lstClipAttack;
    public List<AudioClip> lstClipDeath;

    private void OnEnable()
    {
        gameManager = GameManager.GetSingleton();
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
        SourceDisparo = this.GetComponent<AudioSource>();
        try
        {
            PlaySound(lstClipSpawn[gameManager.mathRNGOther.GetRandom(lstClipSpawn.Count - 1)]);
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }
    internal virtual void Update()
    {
        if (vida <= 0 && !IsDead)
        {
            IsDead = true;
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
        try
        {
            PlaySound(lstClipAttack[gameManager.mathRNGOther.GetRandom(lstClipAttack.Count - 1)]);
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }
    public void ReceiveDamage(int damage)
    {
        vida -= damage;
    }
    public void Death()
    {
        var getIsDead = animator.GetBool("IsDead");
        if (getIsDead) return;
        animator.SetBool("IsDead", true);
        animator.SetTrigger("OnDeath");
        GetComponent<NavMeshAgent>().SetDestination(transform.position);
        Destroy(gameObject, 3);
        try
        {
            PlaySound(lstClipDeath[gameManager.mathRNGOther.GetRandom(lstClipDeath.Count - 1)]);
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }
    public void Detener()
    {
        animator.SetTrigger("OnObjectiveDestroyed");
        GetComponent<NavMeshAgent>().SetDestination(transform.position);
    }
    public void PlaySound(AudioClip clip)
    {
        try
        {
            SourceDisparo.clip = clip;
            SourceDisparo.Play();
        }
        catch (Exception e) { 
            Debug.LogException(e);
        }
    }
}