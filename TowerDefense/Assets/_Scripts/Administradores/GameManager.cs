using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
/**
 * @file
 * @brief Administrador principal del juegol, indica las fases en que se ordena el ciclo de juego
 */
public class GameManager : MonoBehaviour
{
    #region Singleton
    /** @hidden*/
    private static GameManager SingletonGameManager;
    /** @hidden*/
    private GameManager()
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
    /** Solo se puede crear un objeto de la clase GameManager, este metodo obtiene el objeto creado */
    public static GameManager GetSingleton()
    {
        return SingletonGameManager;
    }
    #endregion

    #region Privados
    /** @hidden*/
    private AudioManager audioManager;
    public MathRNG mathRNG = new MathRNG(3241);
    public MathRNG mathRNGOther = new MathRNG(3241);
    /** @hidden*/
    private int enemigosBaseDerrotados;
    /** @hidden*/
    private int enemigosJefeDerrotados;
    /** @hidden*/
    private int RondaActual = 0;
    /** @hidden*/
    private int RondasTotales = 0;
    /** @hidden*/
    private List<GameObject> EnemigosGenerados;
    /** @hidden*/
    private GameState ActualGameState;
    /** @hidden*/
    private AdminSpawnerEnemigos[] lstSpawners;
    #endregion

    #region Getters & Setters
    /** Obtiene la Ronda Actual*/
    public int GetRondaActual()
    {
        return RondaActual;
    }
    /** Obtiene las rondas totales del nivel*/
    public int GetRondasTotales()
    {
        return RondasTotales;
    }
    /** Obtiene los enemigos base derrotados*/
    public int GetEnemigosBaseDerrotados()
    {
        return enemigosBaseDerrotados;
    }
    /** Obtiene la cantidad de recursos oro (actual)*/
    public int GetRecursos()
    {
        return recursos;
    }
    /** Cada vez que un enemigo base es eliminado se agrega a este contador*/
    public void AddEnemigosBaseDerrotados(int incremento = 1)
    {
        enemigosBaseDerrotados += incremento;
    }
    /** Obtiene los enemigos jefe derrotados*/
    public int GetEnemigosJefeDerrotados()
    {
        return enemigosJefeDerrotados;
    }
    /** Cada vez que un enemigo jefe es eliminado se agrega a este contador*/
    public void AddEnemigosJefeDerrotados(int incremento = 1)
    {
        enemigosJefeDerrotados += incremento;
    }
    /** Obtiene el estado dela ctual del juego*/
    public GameState GetActualGameState()
    {
        return ActualGameState;
    }
    /** Indica si es la rond final*/
    public bool RondaFinal()
    {
        return RondaActual >= RondasTotales - 1;
    }
    /** Obtiene los AdminSpawnerEnemigos del nivel*/
    public AdminSpawnerEnemigos[] getSpawners()
    {
        return lstSpawners;
    }
    #endregion

    #region Editor Variables
    /** Si este objeto es destruido pierdes el juego*/
    public SceneObjective referenciaObjetivo;
    /** Cantidad de recursos(oro) inicial*/
    public int recursos = 800;
    #endregion

    #region Level Game Variables
    /** @hidden*/
    private bool _GameEnd = false;
    /** Indica si el juego termino (nivel completado o es Game Over)*/
    public bool IsGameEnd { get { return _GameEnd; } }
    /** @hidden*/
    private bool GamePause = false;
    /** Indica si el juego esta pausado*/
    public bool IsGamePause { get { return GamePause; } }
    /** @hidden*/
    private bool LevelCleared = false;
    /** Indica si el nivel del juego fue completado*/
    public bool IsLevelCleared { get { return LevelCleared; } }
    /** Indica si el juego no esta pausado ni terminado*/
    public bool IsGameActive { get { return !_GameEnd && !GamePause && !LevelCleared; } }
    #endregion

    #region GameState
    /** Estados del juego*/
    public enum GameState
    {
        Preparation = 0,
        Action = 1,
        Ended = 2,
    }
    #endregion

    #region EventHandlers
    /** @hidden*/ public delegate void GameEvent();
    /** @hidden*/ public event GameEvent OnGameStart;
    /** @hidden*/ public event GameEvent OnGamePause;
    /** @hidden*/ public event GameEvent OnGameResume;
    /** @hidden*/ public event GameEvent OnGameEnd;
    /** @hidden*/ public event GameEvent OnGameOver;
    /** @hidden*/ public event GameEvent OnGameExit;
    /** @hidden*/ public event GameEvent OnGameLevelCleared;
    /** @hidden*/ public event GameEvent OnWavePreparation;
    /** @hidden*/ public event GameEvent OnWaveStart;
    /** @hidden*/ public event GameEvent OnWaveEnd;
    /** @hidden*/ public delegate void RecursosModificados();
    /** @hidden*/ public event RecursosModificados EnRecursosModificados;
    /** Clip musical que se reproduce en el juego*/
    public AudioClip ClipBGM;
    /** Clip musical que se reproduce cuando se completa el nivel*/
    public AudioClip ClipLevelCleared;
    /** Clip musical que se reproduce cuando pierdes el nivel*/
    public AudioClip ClipGameOver;

    /** Inicia el juego*/
    public void StartGame()
    {
        _GameEnd = false;
        ResetValores();
        OnGameStart();
    }
    /** Pausa el juego*/
    public void PauseGame()
    {
        if (ActualGameState == GameState.Action || ActualGameState == GameState.Preparation)
        {
            GamePause = true;
            OnGamePause();
        }
    }
    /** Reanuda el juego si estuvo pausado*/
    public void ResumeGame()
    {
        GamePause = false;
        OnGameResume();
    }
    /** Finaliza el nivel*/
    public void GameEnd()
    {
        _GameEnd = true;
        OnGameEnd();
    }
    /** Pierdes el nivel*/
    public void GameOver()
    {
        OnGameOver();
    }
    /** Completas el nivel*/
    public void GameLevelCleared()
    {
        LevelCleared = true;
        OnGameLevelCleared();
    }
    /** Inicia la preparacion antes de la horda*/
    public void WavePreparation()
    {
        ActualGameState = GameState.Preparation;
        OnWavePreparation();
    }
    /** Inicia la horda*/
    public void StartWave()
    {
        if (ActualGameState == GameState.Preparation)
        {
            ActualGameState = GameState.Action;
            OnWaveStart();
        }
    }
    /** Finaliza la horda*/
    public void EndWave()
    {
        ActualGameState = GameState.Ended;
        OnWaveEnd();
    }
    #endregion

    #region Awake, Start & Update
    /** @hidden*/
    private void Awake()
    {
        OnGameStart += delegate { Time.timeScale = 1; };
        OnGamePause += delegate { Time.timeScale = 0; };
        OnGameResume += delegate { Time.timeScale = 1; };
        OnGameEnd += delegate { Time.timeScale = 0; };
        OnGameOver += delegate { OnGameEnd(); };
        OnGameLevelCleared += delegate { OnGameEnd(); };
        OnGameExit += delegate { Time.timeScale = 1; };
        OnWavePreparation += delegate { };
        OnWaveStart += delegate { };
        OnWaveEnd += delegate { CheckEndWave(); };
        EnRecursosModificados += delegate { };
        lstSpawners = GameObject.FindObjectsByType<AdminSpawnerEnemigos>(FindObjectsSortMode.InstanceID);
        var lstSpawnersCount = (from x in lstSpawners select x.getRondasTotales()).ToArray();
        RondasTotales = lstSpawnersCount.Max(x => x);
        EnemigosGenerados = new List<GameObject>();
    }
    /** @hidden*/
    private void Start()
    {
        audioManager = AudioManager.GetSingleton();
        if (audioManager != null)
        {
            OnWaveStart += delegate { audioManager.PlaySound(ClipBGM); };
            OnGameLevelCleared += delegate { audioManager.PlaySound(ClipLevelCleared); };
            OnGameOver += delegate { audioManager.PlaySound(ClipGameOver); };
        }
        OnGameStart();
        WavePreparation();
        //Invoke("StartWave", 0.5f);
    }
    /** @hidden*/
    private void Update()
    {
        if (IsGameEnd) return;
        if (ActualGameState == GameState.Action)
        {
            var lstEnemy = GameObject.FindObjectsByType<_Enemy>(FindObjectsSortMode.InstanceID);
            if (lstEnemy.Length == 0)
            {
                var lstSpawnersEnemigosPendientes = (from x in lstSpawners where x.getHordaActual() != null && x.getHordaActual().EnemigosPendientes select x).ToArray();
                if (lstSpawnersEnemigosPendientes.Length == 0)
                {
                    EndWave();
                }
            }
        }
    }
    /** Indica si ya finalizo la oleada*/
    private void CheckEndWave()
    {
        var lstSpawnersOlasFinalizadas = (from x in lstSpawners where !x.getOleadaFinalizada() select x).ToArray();
        if (lstSpawnersOlasFinalizadas.Length > 0)
        {
            RondaActual++;
            Invoke("WavePreparation", 3f);
        }
        else
        {
            Invoke("GameLevelCleared", 3f);
        }
    }
    /** @hidden*/
    private void OnEnable()
    {
        CreateSingleton();
        referenciaObjetivo.EnObjetivoDestruido += GameOver;
    }
    /** @hidden*/
    private void OnDisable()
    {
        referenciaObjetivo.EnObjetivoDestruido -= GameOver;
    }
    #endregion

    #region General
    /** Metodo que suma los recursos disponibles*/
    public void ModificarRecursos(int modificacion)
    {
        recursos += modificacion;
        if (EnRecursosModificados != null)
        {
            EnRecursosModificados();
        }
    }
    /** Reinicia los valores al iniciar o reiniciar el nivel*/
    public void ResetValores()
    {
        RondaActual = 0;
        enemigosBaseDerrotados = 0;
        enemigosJefeDerrotados = 0;
    }
    #endregion
}