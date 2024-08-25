using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using TMPro;
public class AdministradorGPGS : MonoBehaviour
{
    private GameManager gameManager;
    public TMPro.TMP_Text GPGSText;
    void Start()
    {
        PlayGamesPlatform.Activate();
        PlayGamesPlatform.Instance.Authenticate(ProcesarAutenticacion);
        gameManager = GameManager.GetManager();
        gameManager.OnWaveEnd += delegate { DesbloquearLogro(); };
    }
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
    internal void DesbloquearLogro()
    {
        string mStatus;
        Social.ReportProgress(
            GPGSIds.achievement_primer_oleada_ganada,
            100.0f,
            (bool success) =>
            {
                mStatus = success ? "Logro desbloqueado" : "Fallo en el desbloqueo delo logro";
                GPGSText.text = mStatus;
            }
        );
    }
}