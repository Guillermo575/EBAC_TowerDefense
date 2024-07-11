using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Bala : MonoBehaviour, IAtacante
{
    public Vector3 destino;
    public float velocidad = 20;
    public GameObject enemigo;
    public int damage = 10;
    void Start()
    {
        destino.y += 1;
    }
    void Update()
    {
        var paso = velocidad * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, destino, paso);
        if (Vector3.Distance(transform.position, destino) <0.01f)
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
    }
    public void MakeDamage(int damage = 0)
    {
        enemigo.GetComponent<_Enemy>().ReceiveDamage(damage);
    }
}