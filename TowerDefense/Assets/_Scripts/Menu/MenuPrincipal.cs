using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuPrincipal : _Menu
{
    #region Awake
    protected override void Start()
    {
        base.Start();
        Time.timeScale = 1;
    }
    #endregion

    #region Interfaz
    public void IniciarJuego()
    {
        SceneManager.LoadScene(1);
    }
    public void MostrarOpciones()
    {
        menuManager.ShowMenu(menuManager.menuOpciones);
    }
    public void FinalizarJuego()
    {
        Application.Quit();
    }
    public void BackMenu()
    {
        menuManager.BackMenu();
    }
    #endregion
}