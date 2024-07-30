using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Bala : _DamageElement
{
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
            DestroyGameObject();
        }
        if (collision.gameObject.tag == "Suelo" || collision.gameObject.tag == "Plataforma")
        {
            DestroyGameObject();
        }
    }
    public void DestroyGameObject()
    {
        if (particulaExplosion != null)
        {
            GameObject particulas = Instantiate(particulaExplosion, transform.position, Quaternion.identity) as GameObject;
        }
        Destroy(gameObject);
    }
}