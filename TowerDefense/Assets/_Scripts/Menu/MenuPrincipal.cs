using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/**
 * @file
 * @brief Menu que se abre al iniciar la aplicacion
 */
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
    public void BackToTitle()
    {
        SceneManager.LoadScene(0);
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
    public void MostrarCreditos()
    {
        menuManager.ShowMenu(menuManager.menuCreditos);
    }
    #endregion
}