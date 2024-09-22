using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/**
 * @file
 * @brief Interfaz para atacar a los objetos de la interfaz IAtacable
 */
public interface IAtacante
{
    public void MakeDamage(int damage = 0);
}