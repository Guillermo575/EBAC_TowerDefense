using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
/**
 * @file
 * @brief Clase que se manda a llamar en un frame determinado de una animacion
 */
public class _AnimacionScript : MonoBehaviour
{
    /** Evento generico que se manda a llamar por medio del metodo CallEvent*/
    public UnityEvent evento;
    /** Metodo que se manda a llamar en medio de la animacion*/
    public void CallEvent()
    {
        if (evento == null) return;
        evento?.Invoke();
    }
}