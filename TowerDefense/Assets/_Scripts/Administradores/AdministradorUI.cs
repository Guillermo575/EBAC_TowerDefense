using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static GameManager;
public class AdministradorUI : MonoBehaviour
{
    private GameManager gameManager;
    public GameObject canvasPrincipal;
    public GameObject canvasResultados;
    public GameObject canvasOlaFinal;
    public TextMeshProUGUI txtRecursos;
    public TextMeshProUGUI txtOleada;
    public TextMeshProUGUI txtEnemigosDerrotados;
    public TextMeshProUGUI txtJefesDerrotados;

    void Start()
    {
        gameManager = GameManager.GetManager();
        gameManager.OnGameResume += delegate { canvasPrincipal.SetActive(true); };
        gameManager.OnGamePause += delegate { canvasPrincipal.SetActive(false); };
        gameManager.OnGameEnd += delegate { canvasPrincipal.SetActive(false); OcultarCanvasResultados(); };
        gameManager.OnWaveStart += delegate {
            if (gameManager.RondaFinal())
            {
                MostrarCanvasOlaFinal(); 
                Invoke("OcultarCanvasOlaFinal", 3f); 
            }
        };
        gameManager.OnWavePreparation += delegate { OcultarCanvasResultados(); };
        gameManager.OnWaveEnd += delegate { MostrarCanvasResultados(); };
    }
    void Update()
    {
        txtOleada.text = $"Oleada: {gameManager.GetRondaActual() + 1}";
        txtRecursos.text = $"Recursos: {gameManager.GetRecursos()}";
        txtEnemigosDerrotados.text = $"Enemigos derrotados: {gameManager.GetEnemigosBaseDerrotados()}";
        txtJefesDerrotados.text = $"Jefes derrotados: {gameManager.GetEnemigosJefeDerrotados()}";
    }
    public void MostrarCanvasResultados()
    {
        canvasResultados.SetActive(true);
    }
    public void OcultarCanvasResultados()
    {
        canvasResultados.SetActive(false);
    }
    public void MostrarCanvasOlaFinal()
    {
        canvasOlaFinal.SetActive(true);
    }
    public void OcultarCanvasOlaFinal()
    {
        canvasOlaFinal.SetActive(false);
    }
    public void AdministrarToogles(Toggle toggle)
    {
        GameObject[] lstToggle = GameObject.FindGameObjectsWithTag("ToggleTorre");
        foreach (var objToggle in lstToggle)
        {
            var obj = objToggle.GetComponent<Toggle>();
            obj.isOn = GameObject.ReferenceEquals(obj, toggle);
        }
    }
    public void IniciarOleada()
    {
        if (gameManager.GetActualGameState() == GameState.Preparation)
        {
            gameManager.StartWave();
        }
    }
}