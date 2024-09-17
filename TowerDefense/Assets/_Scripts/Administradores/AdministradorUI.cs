using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static GameManager;
public class AdministradorUI : MonoBehaviour
{
    //public void AdministrarToogles(Toggle toggle)
    //{
    //    GameObject[] lstToggle = GameObject.FindGameObjectsWithTag("ToggleTorre");
    //    foreach (var objToggle in lstToggle)
    //    {
    //        var obj = objToggle.GetComponent<Toggle>();
    //        obj.isOn = GameObject.ReferenceEquals(obj, toggle);
    //    }
    //}

    #region Singleton
    private static AdministradorUI SingletonGameManager;
    private AdministradorUI()
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
    public static AdministradorUI GetSingleton()
    {
        return SingletonGameManager;
    }
    #endregion

    #region Variables
    private GameManager gameManager;
    public GameObject canvasPrincipal;
    public GameObject canvasResultados;
    public GameObject canvasOlaFinal;
    public GameObject canvasOlaInicio;
    public GameObject menuTorres;
    public GameObject btnStartWave;
    public TextMeshProUGUI txtRecursos;
    public TextMeshProUGUI txtOleada;
    public TextMeshProUGUI txtEnemigosDerrotados;
    public TextMeshProUGUI txtJefesDerrotados;
    #endregion

    #region Start & Update
    void Awake()
    {
        CreateSingleton();
    }
    void Start()
    {
        gameManager = GameManager.GetSingleton();
        gameManager.OnGameResume += delegate { canvasPrincipal.SetActive(true); };
        gameManager.OnGamePause += delegate { canvasPrincipal.SetActive(false); };
        gameManager.OnGameEnd += delegate { canvasPrincipal.SetActive(false); OcultarCanvasResultados(); };
        gameManager.OnWaveStart += delegate {
            if (gameManager.RondaFinal())
            {
                MostrarCanvasOlaFinal(); 
                Invoke("OcultarCanvasOlaFinal", 3f);
            }
            else
            {
                MostrarCanvasOlaInicio();
                Invoke("OcultarCanvasOlaInicio", 3f);
            }
        };
        gameManager.OnWavePreparation += delegate { btnStartWave.SetActive(true); OcultarCanvasResultados(); };
        gameManager.OnWaveEnd += delegate { MostrarCanvasResultados(); };
    }
    void Update()
    {
        txtOleada.text = $"Wave: {gameManager.GetRondaActual() + 1} - { gameManager.GetRondasTotales() }";
        txtRecursos.text = $"Gold: {gameManager.GetRecursos()}";
        txtEnemigosDerrotados.text = $"Enemy Defeated: {gameManager.GetEnemigosBaseDerrotados()}";
        txtJefesDerrotados.text = $"Bosses Defeated: {gameManager.GetEnemigosJefeDerrotados()}";
    }
    #endregion

    #region Canvas Resultados
    public void MostrarCanvasResultados()
    {
        canvasResultados.SetActive(true);
    }
    public void OcultarCanvasResultados()
    {
        canvasResultados.SetActive(false);
    }
    #endregion

    #region Canvas Ola Inicio
    public void MostrarCanvasOlaInicio()
    {
        canvasOlaInicio.SetActive(true);
    }
    public void OcultarCanvasOlaInicio()
    {
        canvasOlaInicio.SetActive(false);
    }
    #endregion

    #region Canvas Ola Final
    public void MostrarCanvasOlaFinal()
    {
        canvasOlaFinal.SetActive(true);
    }
    public void OcultarCanvasOlaFinal()
    {
        canvasOlaFinal.SetActive(false);
    }
    #endregion

    #region Interfaz
    public void IniciarOleada()
    {
        if (gameManager.GetActualGameState() == GameState.Preparation)
        {
            btnStartWave.SetActive(false);
            gameManager.StartWave();
        }
    }
    public void PausarJuego()
    {
        if (!gameManager.IsGamePause)
        {
            gameManager.PauseGame();
        }
    }
    #endregion

    #region MenuTorres
    public void MostrarMenuTorres()
    {
        menuTorres.SetActive(true);
    }
    public void OcultarMenuTorres()
    {
        menuTorres.SetActive(false);
    }
    #endregion
}