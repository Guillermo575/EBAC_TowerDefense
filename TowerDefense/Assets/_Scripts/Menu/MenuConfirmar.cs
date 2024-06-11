using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
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
    public void OpenWindow(UnityEvent EventoConfirmar, string titulo = null)
    {
        menuManager = MenuManager.GetManager();
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