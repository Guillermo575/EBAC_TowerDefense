using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;

public class AudioManager : MonoBehaviour
{
    #region Singleton
    private static AudioManager SingletonGameManager;
    private AudioManager()
    {
    }
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
    public static AudioManager GetSingleton()
    {
        return SingletonGameManager;
    }
    #endregion

    #region Variables
    public AudioMixer audioMixer;
    public Opciones opciones;
    private AudioSource audioSource;
    #endregion

    #region Awake & Start
    void Awake()
    {
        CreateSingleton();
    }
    private void Start()
    {
        audioSource = this.GetComponentInParent<AudioSource>();
        SetBGMVolume(opciones.VolumenMusica);
        SetSFXVolume(opciones.VolumenSonido);        
    }
    #endregion

    #region Change Volume
    public void SetBGMVolume(float volume)
    {
        audioMixer.SetFloat("BGMVol", volume);
    }
    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("SFXVol", volume);
    }
    public static float LinearToDecibel(float linear)
    {
        float dB;
        if (linear != 0)
            dB = 20.0f * Mathf.Log10(linear);
        else
            dB = -144.0f;
        return dB;
    }
    public static float DecibelToLinear(float dB)
    {
        if (dB == -80f) return 0;
        float linear = Mathf.Pow(10.0f, dB / 20.0f);
        return linear;
    }
    #endregion

    #region Play Sound
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