using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
/**
 * @file
 * @brief Interfaz que cambia el volumen de la musica de fondo
 */
public class SliderVolumenMusica : MonoBehaviour
{
    #region Variables
    private MenuManager menuManager;
    private Opciones opciones;
    Slider slider;
    #endregion

    #region Awake & Start
    void Awake()
    {
        menuManager = MenuManager.GetSingleton();
        opciones = menuManager.opciones;
    }
    public void Start()
    {
        slider = this.GetComponent<Slider>();
        slider.value = (int)opciones.VolumenMusica;
        slider.onValueChanged.AddListener(delegate { ControlarCambios(); });
    }
    #endregion

    #region General
    public void ControlarCambios()
    {
        opciones.CambiarVolumenMusica(slider.value);
    }
    #endregion
}