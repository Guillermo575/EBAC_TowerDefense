using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
public class GameManager : MonoBehaviour
{
    #region Singleton
    private static GameManager SingletonGameManager;
    private GameManager()
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
    public static GameManager GetManager()
    {
        return SingletonGameManager;
    }
    #endregion

    #region Privados
    private AudioManager audioManager;
    public MathRNG mathRNG = new MathRNG(3241);
    [HideInInspector] public int enemigosBaseDerrotados;
    [HideInInspector] public int enemigosJefeDerrotados;
    [HideInInspector] public int recursos;
    [HideInInspector] public int RondaActual = 0;
    [HideInInspector] public List<GameObject> EnemigosGenerados;
    #endregion

    #region Editor Variables
    public SceneObjective referenciaObjetivo;
    #endregion

    #region Level Game Variables
    private bool _GameEnd = false;
    public bool IsGameEnd { get { return _GameEnd; } }

    private bool GamePause = false;
    public bool IsGamePause { get { return GamePause; } }

    private bool LevelCleared = false;
    public bool IsLevelCleared { get { return LevelCleared; } }
    public bool IsGameActive { get { return !_GameEnd && !GamePause && !LevelCleared; } }
    #endregion

    #region EventHandlers
    public delegate void GameEvent();
    public event GameEvent OnGameStart;
    public event GameEvent OnGamePause;
    public event GameEvent OnGameResume;
    public event GameEvent OnGameEnd;
    public event GameEvent OnGameOver;
    public event GameEvent OnGameExit;
    public event GameEvent OnGameLevelCleared;
    public event GameEvent OnWaveStart;
    public event GameEvent OnWaveEnd;
    public event GameEvent OnWaveWon;
    public delegate void RecursosModificados();
    public event RecursosModificados EnRecursosModificados;
    public void StartGame()
    {
        OnGameStart();
    }
    public void PauseGame()
    {
        OnGamePause();
    }
    public void ResumeGame()
    {
        OnGameResume();
    }
    public void GameEnd()
    {
        OnGameEnd();
    }
    public void GameOver()
    {
        OnGameOver();
    }
    public void StartWave()
    {
        OnWaveStart();
    }
    public void EndWave()
    {
        OnWaveEnd();
    }
    public void WaveWon()
    {
        OnWaveWon();
    }
    #endregion

    #region Awake, Start & Update
    private void Awake()
    {
        CreateSingleton();
        OnGameStart += delegate { _GameEnd = false; ResetValores(); Time.timeScale = 1; };
        OnGamePause += delegate { GamePause = true; Time.timeScale = 0; };
        OnGameResume += delegate { GamePause = false; Time.timeScale = 1; };
        OnGameEnd += delegate { _GameEnd = true; Time.timeScale = 0; };
        OnGameOver += delegate { OnGameEnd(); };
        OnGameLevelCleared += delegate { LevelCleared = true; OnGameEnd(); };
        OnGameExit += delegate { Time.timeScale = 1; };
    }
    private void Start()
    {
        audioManager = AudioManager.GetManager();
        if (audioManager != null)
        {
            OnGamePause += delegate { audioManager.BGM.Pause(); audioManager.SFX.Pause(); };
            OnGameResume += delegate { audioManager.BGM.UnPause(); audioManager.SFX.UnPause(); };
            OnGameEnd += delegate { audioManager.BGM.Stop(); audioManager.SFX.Stop(); };
        }
        OnGameStart();
        Invoke("StartWave", 0.5f);
    }
    private void Update()
    {
        if (IsGameEnd) return;
        var lstEnemy = GameObject.FindObjectsByType<_Enemy>(FindObjectsSortMode.InstanceID);
        if (lstEnemy.Length == 0)
        {
            var lstSpawners = GameObject.FindObjectsByType<AdminSpawnerEnemigos>(FindObjectsSortMode.InstanceID);
            var lstSpawnersEnemigosPendientes = (from x in lstSpawners where x.getHordaActual() != null && x.getHordaActual().EnemigosPendientes select x).ToArray();
            if (lstSpawnersEnemigosPendientes.Length == 0)
            {
                var lstSpawnersOlasFinalizadas = (from x in lstSpawners where !x.getOleadaFinalizada() select x).ToArray();
                if (lstSpawnersOlasFinalizadas.Length > 0)
                {
                    RondaActual++;
                    OnWaveStart();
                }
                else
                {
                    OnGameLevelCleared();
                }
            }
        }
    }
    private void OnEnable()
    {
        referenciaObjetivo.EnObjetivoDestruido += GameOver;
    }
    private void OnDisable()
    {
        referenciaObjetivo.EnObjetivoDestruido -= GameOver;
    }
    #endregion

    #region General
    public void ModificarRecursos(int modificacion)
    {
        recursos += modificacion;
        if (EnRecursosModificados != null)
        {
            EnRecursosModificados();
        }
    }
    public void ResetValores()
    {
        RondaActual = 0;
        enemigosBaseDerrotados = 0;
        enemigosJefeDerrotados = 0;
    }
    #endregion
}