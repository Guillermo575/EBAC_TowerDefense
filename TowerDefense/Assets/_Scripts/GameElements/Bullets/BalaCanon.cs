using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/**
 * @file
 * @brief Clase de la bala que disparan las torres, usa fisicas para moverse
 */
public class BalaCanon : _DamageElement
{
    void Update()
    {
        bool sleeping = GetComponent<Rigidbody>().IsSleeping();
        if (sleeping)
        {
            Destroy(gameObject);
        }
    }
    public override void CheckCollision(GameObject gameObject)
    {
        if (gameObject.tag == "Enemigo")
        {
            enemigo = gameObject;
            MakeDamage(damage);
            DestroyGameObject();
        }
        if (gameObject.tag == "Suelo" || gameObject.tag == "Plataforma")
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