using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AdministradorTorres : MonoBehaviour
{

    #region Singleton
    private static AdministradorTorres SingletonGameManager;
    private AdministradorTorres()
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
    public static AdministradorTorres GetSingleton()
    {
        return SingletonGameManager;
    }
    #endregion

    #region Variables
    private GameManager gameManager;
    private AdminSpawnerEnemigos[] referenciaSpawners;
    private List<GameObject> lstTorresInstanciadas;
    public GameObject Objetivo;
    public enum TorreSeleccionada
    {
        Torre1,Torrre2,Torre3,Torre4,Torre5,
    }
    [HideInInspector] public GameObject plataformaSeleccionada;
    public TorreSeleccionada torreSeleccionada;
    public List<GameObject> prefabTorres;
    public delegate void EnemigoObjetivoActualizado();
    #endregion

    //private void OnEnable()
    //{
    //    if (referenciaAdminToques == null) return;
    //    referenciaAdminToques.EnPlataformaTocada += CrearTorre;
    //    gameManager.OnWaveStart += ActualizarObjetivo;
    //    lstTorresInstanciadas = new List<GameObject>();
    //}
    //private void OnDisable()
    //{
    //    if (referenciaAdminToques == null) return;
    //    referenciaAdminToques.EnPlataformaTocada -= CrearTorre;
    //    gameManager.OnWaveStart -= ActualizarObjetivo;
    //}

    #region Start & Update
    void Awake()
    {
        CreateSingleton();
    }
    void Start()
    {
        gameManager = GameManager.GetSingleton();
        referenciaSpawners = gameManager.getSpawners();
        lstTorresInstanciadas = new List<GameObject>();
    }
    void Update()
    {     
    }
    #endregion

    #region Crear Torre
    public void InstanciarTorre(int torre)
    {
        ConfigurarTorre(torre);
        CrearTorre(plataformaSeleccionada);
    }
    void CrearTorre(GameObject plataforma)
    {
        int indiceTorre = (int)torreSeleccionada;
        var prefabTorre = prefabTorres[indiceTorre].GetComponent<_Tower>();
        if (plataforma.transform.childCount == 0 && gameManager.GetRecursos() > prefabTorre.CostoInstalacion)
        {
            gameManager.ModificarRecursos(-prefabTorre.CostoInstalacion);
            Debug.Log("Creando Torre");
            Vector3 posparaInstanciar = plataforma.transform.position;
            posparaInstanciar.y += 1f;
            GameObject torreInstanciada = Instantiate<GameObject>(prefabTorre.gameObject, posparaInstanciar, Quaternion.identity);
            torreInstanciada.transform.SetParent(plataforma.transform);
            lstTorresInstanciadas.Add(torreInstanciada);
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
    #endregion
}