using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class _AnimacionScript : MonoBehaviour
{
    public UnityEvent evento;
    public void CallEvent()
    {
        if (evento == null) return;
        evento?.Invoke();
    }
}