using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
/**
 * @file
 * @brief Interfaz que cambia el volumen de los efectos de sonidos
 */
public class SliderVolumenSonido : MonoBehaviour
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
        slider.value = (int)opciones.VolumenSonido;
        slider.onValueChanged.AddListener(delegate { ControlarCambios(); });
    }
    #endregion

    #region General
    public void ControlarCambios()
    {
        opciones.CambiarVolumenSonido(slider.value);
    }
    #endregion
}