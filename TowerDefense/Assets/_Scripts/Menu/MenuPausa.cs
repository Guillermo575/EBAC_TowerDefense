using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
public class MenuPausa : _Menu
{
    #region Variables
    private GameManager gameManager;
    #endregion

    #region Awake & Update
    protected override void Start()
    {
        base.Start();
        gameManager = GameManager.GetManager();
        gameManager.OnGamePause += MostrarMenuPausa;
        gameManager.OnGameResume += OcultarMenuPausa;
    }
    private void Update()
    {
        if (!gameManager.IsGameEnd)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (gameManager.IsGamePause)
                {
                    gameManager.ResumeGame();
                }
                else
                {
                    gameManager.PauseGame();
                }
            }
        }
    }
    #endregion

    #region Mostrar/Ocultar menu
    private void MostrarMenuPausa(object sender, EventArgs e)
    {
        menuManager.ShowMenu(menuManager.menuPausa);
    }
    public void OcultarMenuPausa(object sender, EventArgs e)
    {
        menuManager.DeleteMenuTree();
    }
    #endregion

    #region Botones Interfaz
    public void RegresarAPantallaPrincipal()
    {
        MostrarPantallaConfirmar(EventoRegresarAPantallaPrincipal, "Do you want to return to the main menu?");
    }
    private void EventoRegresarAPantallaPrincipal()
    {
        SceneManager.LoadScene(0);
    }
    public void ReintentarNivel()
    {
        MostrarPantallaConfirmar(EventoReintentarNivel, "Do you want to restart the level?");
    }
    public void EventoReintentarNivel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void MostrarMenuOpciones()
    {
        menuManager.ShowMenu(menuManager.menuOpciones);
    }
    public void ResumeGame()
    {
        gameManager.ResumeGame();
    }
    public void BackMenu()
    {
        menuManager.BackMenu();
    }
    #endregion

    #region Panel Confirmar
    private void MostrarPantallaConfirmar(UnityAction evt, String msg)
    {
        var menuConfirmar = menuManager.menuConfirmar;
        if (menuConfirmar != null)
        {
            UnityEvent objEvent = new UnityEvent();
            objEvent.AddListener(evt);
            menuConfirmar.OpenWindow(objEvent, msg);
        }
        else
        {
            evt();
        }
    }
    #endregion
}