using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
/**
 * @file
 * @brief Administra el audiomixer 
 */
public class AudioManager : MonoBehaviour
{
    #region Singleton
    /** @hidden */
    private static AudioManager SingletonGameManager;
    /** @hidden */
    private AudioManager()
    {
    }
    /** Aqui se crea el objeto singleton */
    private void CreateSingleton()
    {
        if (SingletonGameManager == null)
        {
            SingletonGameManager = this;
        }
        else
        {
            Debug.LogError("Ya existe una instancia de esta clase");
        }
    }
    /** Solo se puede crear un objeto de la clase AudioManager, este metodo obtiene el objeto creado */
    public static AudioManager GetSingleton()
    {
        return SingletonGameManager;
    }
    #endregion

    #region Variables
    /**
     * Objeto que administra el audio BGM y SFX
     */
    public AudioMixer audioMixer;
    /**
     * Objeto de la clase Opciones (Scriptable Object) que carga el volumen del AudioMizer
     */
    public Opciones opciones;
    /**
     * Objeto de la clase AudioSource que reproduce el sonido de fondo (BGM)
     */
    private AudioSource audioSource;
    #endregion

    #region Awake & Start
    /** @hidden */
    void Awake()
    {
        CreateSingleton();
    }
    /** @hidden */
    private void Start()
    {
        audioSource = this.GetComponentInParent<AudioSource>();
        SetBGMVolume(opciones.VolumenMusica);
        SetSFXVolume(opciones.VolumenSonido);        
    }
    #endregion

    #region Change Volume
    /**
     * Configura el volumen de fondo (BGM)
     * @param volume
     */
    public void SetBGMVolume(float volume)
    {
        audioMixer.SetFloat("BGMVol", volume);
    }

    /**
     * Configura el volumen de efectos de sonido (SFX)
     * @param volume
     */
    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("SFXVol", volume);
    }

    /** @hidden */
    public static float LinearToDecibel(float linear)
    {
        float dB;
        if (linear != 0)
            dB = 20.0f * Mathf.Log10(linear);
        else
            dB = -144.0f;
        return dB;
    }

    /** @hidden */
    public static float DecibelToLinear(float dB)
    {
        if (dB == -80f) return 0;
        float linear = Mathf.Pow(10.0f, dB / 20.0f);
        return linear;
    }
    #endregion

    #region Play Sound
    /**
     * Reproduce musica de fondo
     * @param clip: pista seleccionada
     * @param ReiniciarRepetido: en caso de que se reproduzca la misma pista de sonido reiniciara la pista (desactivado por defecto)
     */
    public void PlaySound(AudioClip clip, Boolean ReiniciarRepetido = false)
    {
        try
        {
            if (clip.name == audioSource.clip.name && !ReiniciarRepetido) return;
            audioSource.clip = clip;
            audioSource.Play();
        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogException(e);
        }
    }
    #endregion
}