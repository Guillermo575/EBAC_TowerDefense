using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class AdministradorUI : MonoBehaviour
{
    private GameManager gameManager;
    public GameObject canvasPrincipal;
    void Start()
    {
        gameManager = GameManager.GetManager();
        gameManager.OnGameResume += delegate { canvasPrincipal.SetActive(true); };
        gameManager.OnGamePause += delegate { canvasPrincipal.SetActive(false); };
        gameManager.OnGameEnd += delegate { canvasPrincipal.SetActive(false); };
    }
    void Update()
    {
    }
    public void MostrarMenuFinOleada()
    {
    }
    public void OcultarMenuFinOleada()
    {
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
}