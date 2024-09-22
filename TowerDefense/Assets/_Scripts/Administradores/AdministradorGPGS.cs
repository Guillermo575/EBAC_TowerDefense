using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using TMPro;
using UnityEngine.SceneManagement;
/**
 * @file
 * @brief Aqui se gestiona las funciones principales de Google Plays
 */
public class AdministradorGPGS : MonoBehaviour
{
    private GameManager gameManager;
    public TMPro.TMP_Text GPGSText;
    void Start()
    {
        PlayGamesPlatform.Activate();
        PlayGamesPlatform.Instance.Authenticate(ProcesarAutenticacion);
        gameManager = GameManager.GetSingleton();
        gameManager.OnWaveEnd += delegate { DesbloquearLogro(GPGSIds.achievement_primer_oleada_ganada); };
        gameManager.OnGameLevelCleared += delegate
        {
            var scene = SceneManager.GetActiveScene();
            switch (scene.name)
            {
                case "Scene_1": DesbloquearLogro(GPGSIds.achievement_first_level_completed); break;
                case "Scene_2": DesbloquearLogro(GPGSIds.achievement_second_level_completed); break;
                case "Scene_3": DesbloquearLogro(GPGSIds.achievement_final_level_completed); break;
            }
        };
    }
    /**
     * Metodo de pruebas para probar la autentificacion
     */
    internal void ProcesarAutenticacion(SignInStatus status)
    {
        if (status == SignInStatus.Success)
        {
            GPGSText.text = $"Good Auth \n {Social.localUser.userName} \n {Social.localUser.id}";
        }
        else
        {
            GPGSText.text = $"Bad Auth";
        }
    }

    /**
     * Sirve para activar un logro\n 
     * @param Logro: Id del logro de Google Plays
     */
    internal void DesbloquearLogro(string Logro)
    {
        string mStatus;
        Social.ReportProgress(
            Logro,
            100.0f,
            (bool success) =>
            {
                mStatus = success ? "Logro desbloqueado" : "Fallo en el desbloqueo delo logro";
                GPGSText.text = mStatus;
            }
        );
    }
}