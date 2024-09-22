using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/**
 * @file
 * @brief Clase que guarda las funciones del objetivo del escenario, al destruirse pierdes el juego
 */
public class SceneObjective : MonoBehaviour, IAtacable
{
    public int vida = 100;
    public delegate void ObjetivoDestruido();
    public event ObjetivoDestruido EnObjetivoDestruido;
    void Start()
    {
    }
    void Update()
    {
        if (vida <= 0)
        {
            if (EnObjetivoDestruido != null)
            {
                EnObjetivoDestruido();
            }
            Destroy(this.gameObject, 0.2f);
        }
    }
    public void ReceiveDamage(int damage = 20)
    {
        vida -= damage;
    }
}