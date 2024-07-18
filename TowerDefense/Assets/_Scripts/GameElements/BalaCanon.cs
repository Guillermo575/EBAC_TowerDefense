using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalaCanon : MonoBehaviour, IAtacante
{
    public Vector3 destino;
    public float velocidad = 20;
    public GameObject enemigo;
    public int damage = 10;
    void Update()
    {
        bool sleeping = GetComponent<Rigidbody>().IsSleeping();
        if (sleeping)
        {
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemigo")
        {
            enemigo = collision.gameObject;
            MakeDamage(damage);
            Destroy(gameObject);
        }
        if (collision.gameObject.tag == "Suelo" || collision.gameObject.tag == "Plataforma")
        {
            Destroy(gameObject);
        }
    }
    public void MakeDamage(int damage = 0)
    {
        enemigo.GetComponent<_Enemy>().ReceiveDamage(damage);
    }
}
