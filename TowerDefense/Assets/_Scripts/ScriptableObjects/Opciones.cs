using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Opciones", menuName = "Tools/Opciones", order = 1)]
public class Opciones : ObjetoPersistente
{
    [HideInInspector] public float MinVolume = -60f;
    [HideInInspector] public float MaxVolume = -60f;
    public float VolumenSonido = 0;
    public float VolumenMusica = 0;
    public enum dificultad
    {
        facil,
        normal,
        dificil
    }
    public void CambiarVolumenMusica(float nuevaVolumen)
    {
        VolumenMusica = nuevaVolumen;
        AudioManager.GetSingleton().SetBGMVolume(VolumenMusica);
    }
    public void CambiarVolumenSonido(float nuevaVolumen)
    {
        VolumenSonido = nuevaVolumen;
        AudioManager.GetSingleton().SetSFXVolume(VolumenSonido);
    }
}