using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static GameManager;
/**
 * @file
 * @brief Administrador de la Interfaz de Usuario
 */
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
    /** @hidden*/
    private AdministradorUI()
    {
    }
    /** Aqui se crea el objeto singleton */
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
    /** Solo se puede crear un objeto de la clase AdministradorUI, este metodo obtiene el objeto creado */
    public static AdministradorUI GetSingleton()
    {
        return SingletonGameManager;
    }
    #endregion

    #region Variables
    /** @hidden*/
    private GameManager gameManager;
    /** Canvas que muestra la informacion principal del juego (el boton de pausa, los recursos y el numero de oleada)*/
    public GameObject canvasPrincipal;
    /** Canvas que muestra los enemigos eliminados*/
    public GameObject canvasResultados;
    /** Canvas que indica si es la Horda Final*/
    public GameObject canvasOlaFinal;
    /** Canvas que indica la oleada no final actual*/
    public GameObject canvasOlaInicio;
    /** Canvas que se despliega cuando seleccionas una base para torres mostrando las que se encuentran disponibles*/
    public GameObject menuTorres;
    /** Boton que inicia la oleada*/
    public GameObject btnStartWave;
    /** Etiqueta que muestra la cantidad de recursos (oro) disponible para crear torres*/
    public TextMeshProUGUI txtRecursos;
    /** Etiqueta que muestra la oleada actual*/
    public TextMeshProUGUI txtOleada;
    /** Etiqueta que muestra los enemigos derrotados*/
    public TextMeshProUGUI txtEnemigosDerrotados;
    /** Etiqueta que muestra los jefes derrotados*/
    public TextMeshProUGUI txtJefesDerrotados;
    #endregion

    #region Start & Update
    /** @hidden*/
    void Awake()
    {
        CreateSingleton();
    }
    /** @hidden*/
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
    /** @hidden*/
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