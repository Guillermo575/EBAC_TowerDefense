using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public event EventHandler OnGameStart;
    public event EventHandler OnGamePause;
    public event EventHandler OnGameResume;
    public event EventHandler OnGameEnd;
    public event EventHandler OnGameOver;
    public event EventHandler OnGameExit;
    public event EventHandler OnGameLevelCleared;
    public void StartGame()
    {
        _GameEnd = false;
        OnGameStart?.Invoke(this, EventArgs.Empty);
    }
    public void PauseGame()
    {
        GamePause = true;
        OnGamePause?.Invoke(this, EventArgs.Empty);
    }
    public void ResumeGame()
    {
        GamePause = false;
        OnGameResume?.Invoke(this, EventArgs.Empty);
    }
    public void ExitGame()
    {
        OnGameExit?.Invoke(this, EventArgs.Empty);
    }
    public void LevelClearedGame()
    {
        LevelCleared = true;
        OnGameLevelCleared?.Invoke(this, EventArgs.Empty);
    }
    public void GameOver()
    {
        OnGameOver?.Invoke(this, EventArgs.Empty);
    }
    public void GameEnd()
    {
        _GameEnd = true;
        OnGameEnd?.Invoke(this, EventArgs.Empty);
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
        OnGameOver += delegate { GameEnd(); };
        OnGameLevelCleared += delegate { GameEnd(); };
        OnGameExit += delegate { Time.timeScale = 1;};
    }
    private void Start()
    {
        var objSuelo = GameObject.Find("Suelo");
        audioManager = AudioManager.GetManager();
        OnGamePause += delegate { audioManager.BGM.Pause(); audioManager.SFX.Pause(); };
        OnGameResume += delegate { audioManager.BGM.UnPause(); audioManager.SFX.UnPause(); };
        OnGameEnd += delegate { audioManager.BGM.Stop(); audioManager.SFX.Stop(); };
        StartGame();
    }
    private void Update()
    {
    }
    #endregion
}