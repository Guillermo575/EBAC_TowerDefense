using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AdministradorTorres : MonoBehaviour
{
    private AdministradorToques referenciaAdminToques;
    private GameManager gameManager;
    public AdminSpawnerEnemigos[] referenciaSpawners;
    public GameObject Objetivo;
    public enum TorreSeleccionada
    {
        Torre1,Torrre2,Torre3,Torre4,Torre5,
    }
    public TorreSeleccionada torreSeleccionada;
    public List<GameObject> prefabTorres;
    public List<GameObject> lstTorresInstanciadas;
    public delegate void EnemigoObjetivoActualizado();
    public event EnemigoObjetivoActualizado EnEnemigoObjetivoActualizado;

    private void OnEnable()
    {
        if (referenciaAdminToques == null) return;
        referenciaAdminToques.EnPlataformaTocada -= CrearTorre;
        referenciaAdminToques.EnPlataformaTocada += CrearTorre;
    }
    private void OnDisable()
    {
        if (referenciaAdminToques == null) return;
        referenciaAdminToques.EnPlataformaTocada -= CrearTorre;
    }
    void Start()
    {
        gameManager = GameManager.GetManager();
        referenciaSpawners = gameManager.getSpawners();
        referenciaAdminToques = AdministradorToques.GetManager();
        referenciaAdminToques.EnPlataformaTocada += CrearTorre;
    }
    void Update()
    {     
    }
    void CrearTorre(GameObject plataforma)
    {
        if (plataforma.transform.childCount == 0)
        {
            Debug.Log("Creando Torre");
            int indiceTorre = (int)torreSeleccionada;
            Vector3 posparaInstanciar = plataforma.transform.position;
            posparaInstanciar.y += 1f;
            GameObject torreInstanciada = Instantiate<GameObject>(prefabTorres[indiceTorre], posparaInstanciar, Quaternion.identity);
            torreInstanciada.transform.SetParent(plataforma.transform);
        }
    }
    public void ConfigurarTorre(int torre)
    {
        if (Enum.IsDefined(typeof(TorreSeleccionada), torre))
        {
            torreSeleccionada = (TorreSeleccionada)torre;
        }
        else
        {
            Debug.LogError("Esa torre no esta definida");
        }
    }
}