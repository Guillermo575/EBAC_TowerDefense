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

    public Opciones opciones;
    public AudioSource BGM;
    public AudioSource SFX;
    public AudioMixer audioMixer;

    void Awake()
    {
        CreateSingleton();
    }
    void Start()
    {
        UpdateSound();
    }
    private void Update()
    {
        UpdateSound();
    }
    private void UpdateSound()
    {
        BGM.volume = DecibelToLinear(opciones.VolumenMusica);
        SFX.volume = DecibelToLinear(opciones.VolumenSonido);
        audioMixer.SetFloat("BGMVol", opciones.VolumenMusica);
        audioMixer.SetFloat("SFXVol", opciones.VolumenSonido);
    }
    private void PlayBGM(AudioClip clip)
    {
        if (opciones.VolumenMusica <= opciones.MinVolume) return;
        BGM.clip = clip;
        BGM.Play();
    }
    public void PlaySoundEffect(AudioClip clip)
    {
        if (opciones.VolumenMusica <= opciones.MinVolume) return;
        SFX.clip = clip;
        SFX.Play();
    }
    private float LinearToDecibel(float linear)
    {
        float dB;
        if (linear != 0)
            dB = 20.0f * Mathf.Log10(linear);
        else
            dB = -144.0f;
        return dB;
    }
    private float DecibelToLinear(float dB)
    {
        if (dB == -80f) return 0;
        float linear = Mathf.Pow(10.0f, dB / 20.0f);
        return linear;
    }
}