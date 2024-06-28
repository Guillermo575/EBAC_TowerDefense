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
    private int enemigosBaseDerrotados;
    private int enemigosJefeDerrotados;
    private int recursos;
    private int RondaActual = 0;
    private int RondasTotales = 0;
    private List<GameObject> EnemigosGenerados;
    private GameState ActualGameState;
    #endregion

    #region Getters & Setters
    public int GetRondaActual()
    {
        return RondaActual;
    }
    public int GetEnemigosBaseDerrotados()
    {
        return enemigosBaseDerrotados;
    }
    public int GetRecursos()
    {
        return recursos;
    }
    public void AddEnemigosBaseDerrotados(int incremento = 1)
    {
        enemigosBaseDerrotados += incremento;
    }
    public int GetEnemigosJefeDerrotados()
    {
        return enemigosJefeDerrotados;
    }
    public void AddEnemigosJefeDerrotados(int incremento = 1)
    {
        enemigosJefeDerrotados += incremento;
    }
    public GameState GetActualGameState()
    {
        return ActualGameState;
    }
    public bool RondaFinal()
    {
        return RondaActual >= RondasTotales - 1;
    }
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

    #region GameState
    public enum GameState
    {
        Preparation = 0,
        Action = 1,
        Ended = 2,
    }
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
    public event GameEvent OnWavePreparation;
    public event GameEvent OnWaveStart;
    public event GameEvent OnWaveEnd;
    public delegate void RecursosModificados();
    public event RecursosModificados EnRecursosModificados;
    public void StartGame()
    {
        _GameEnd = false;
        ResetValores();
        OnGameStart();
    }
    public void PauseGame()
    {
        GamePause = true;
        OnGamePause();
    }
    public void ResumeGame()
    {
        GamePause = false;
        OnGameResume();
    }
    public void GameEnd()
    {
        _GameEnd = true;
        OnGameEnd();
    }
    public void GameOver()
    {
        OnGameOver();
    }
    public void GameLevelCleared()
    {
        LevelCleared = true;
        OnGameLevelCleared();
    }
    public void WavePreparation()
    {
        ActualGameState = GameState.Preparation;
        OnWavePreparation();
    }
    public void StartWave()
    {
        if (ActualGameState == GameState.Preparation)
        {
            ActualGameState = GameState.Action;
            OnWaveStart();
        }
    }
    public void EndWave()
    {
        ActualGameState = GameState.Ended;
        OnWaveEnd();
    }
    #endregion

    #region Awake, Start & Update
    private void Awake()
    {
        CreateSingleton();
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
        WavePreparation();
        var lstSpawners = GameObject.FindObjectsByType<AdminSpawnerEnemigos>(FindObjectsSortMode.InstanceID);
        var lstSpawnersCount = (from x in lstSpawners select x.getRondasTotales()).ToArray();
        RondasTotales = lstSpawnersCount.Max(x => x);
        //Invoke("StartWave", 0.5f);
    }
    private void Update()
    {
        if (IsGameEnd) return;
        if (ActualGameState == GameState.Action)
        {
            var lstEnemy = GameObject.FindObjectsByType<_Enemy>(FindObjectsSortMode.InstanceID);
            if (lstEnemy.Length == 0)
            {
                var lstSpawners = GameObject.FindObjectsByType<AdminSpawnerEnemigos>(FindObjectsSortMode.InstanceID);
                var lstSpawnersEnemigosPendientes = (from x in lstSpawners where x.getHordaActual() != null && x.getHordaActual().EnemigosPendientes select x).ToArray();
                if (lstSpawnersEnemigosPendientes.Length == 0)
                {
                    EndWave();
                }
            }
        }
    }
    private void CheckEndWave()
    {
        var lstSpawners = GameObject.FindObjectsByType<AdminSpawnerEnemigos>(FindObjectsSortMode.InstanceID);
        var lstSpawnersOlasFinalizadas = (from x in lstSpawners where !x.getOleadaFinalizada() select x).ToArray();
        if (lstSpawnersOlasFinalizadas.Length > 0)
        {
            RondaActual++;
            WavePreparation();
        }
        else
        {
            OnGameLevelCleared();
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