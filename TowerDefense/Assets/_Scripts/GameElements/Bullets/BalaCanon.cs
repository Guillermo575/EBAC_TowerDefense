using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            Destroy(gameObject);
        }
        if (gameObject.tag == "Suelo" || gameObject.tag == "Plataforma")
        {
            Destroy(gameObject);
        }
    }
}