using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
/**
 * @file
 * @brief Menu que se abre para advertir al jugador si desea realizar alguna accion,
 * en caso de que el jugador seleccione que si se ejecutara un evento generico
 */
public class MenuConfirmar : _Menu
{
    #region Variables
    public TMP_Text txtTitulo;
    UnityEvent EventoConfirmar;
    #endregion

    #region Awake
    protected override void Start()
    {
        base.Start();
    }
    #endregion

    #region General
    /**
     * Abre la ventana y agrega el evento generico
     * @param EventoConfirmar
     * @param titulo
     */
    public void OpenWindow(UnityEvent EventoConfirmar, string titulo = null)
    {
        menuManager = MenuManager.GetSingleton();
        menuManager.ShowMenu(this.gameObject);
        this.EventoConfirmar = EventoConfirmar;
        if (!string.IsNullOrEmpty(titulo))
        {
            txtTitulo.text = titulo;
        }
    }
    public void ConfirmarNo()
    {
        menuManager.BackMenu();
    }
    public void ConfirmarSi()
    {
        EventoConfirmar.Invoke();
        menuManager.BackMenu();
    }
    #endregion
}