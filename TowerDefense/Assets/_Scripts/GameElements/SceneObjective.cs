using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SceneObjective : MonoBehaviour
{
    public int vida = 100;
    void Start()
    {
    }
    void Update()
    {
        if (vida <= 0)
        {
            Destroy(this.gameObject);
        }
    }
    public void ReceiveDamage(int damage = 20)
    {
        vida -= damage;
    }
}