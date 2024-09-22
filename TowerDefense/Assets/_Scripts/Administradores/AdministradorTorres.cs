using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/**
 * @file
 * @brief Aqui se gestiona la creacion y borrado de las torres
 */
public class AdministradorTorres : MonoBehaviour
{

    #region Singleton
    /** @hidden */
    private static AdministradorTorres SingletonGameManager;
    /** @hidden */
    private AdministradorTorres()
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
    /** Solo se puede crear un objeto de la clase AdministradorTorres, este metodo obtiene el objeto creado */
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
    /** @hidden */
    void Awake()
    {
        CreateSingleton();
    }
    /** @hidden */
    void Start()
    {
        gameManager = GameManager.GetSingleton();
        referenciaSpawners = gameManager.getSpawners();
        lstTorresInstanciadas = new List<GameObject>();
    }
    /** @hidden */
    void Update()
    {     
    }
    #endregion

    #region Crear Torre
    /** Instancia la torre 
        @param torre
     */
    public void InstanciarTorre(int torre)
    {
        ConfigurarTorre(torre);
        CrearTorre(plataformaSeleccionada);
    }
    /** @hidden */
    void CrearTorre(GameObject plataforma)
    {
        int indiceTorre = (int)torreSeleccionada;
        var prefabTorre = prefabTorres[indiceTorre].GetComponent<_Tower>();
        for (var i = 0; i < plataforma.transform.childCount; i++)
        {
            if (prefabTorre.NombreTorre == plataforma.transform.GetChild(i).gameObject.GetComponent<_Tower>().NombreTorre) return;
        }
        if (gameManager.GetRecursos() >= prefabTorre.CostoInstalacion)
        {
            if (plataforma.transform.childCount > 0)
            {
                for (var i = 0; i < plataforma.transform.childCount; i++)
                {
                    GameObject.Destroy(plataforma.transform.GetChild(i).gameObject);
                }
            }
            gameManager.ModificarRecursos(-prefabTorre.CostoInstalacion);
            Debug.Log("Creando Torre");
            Vector3 posparaInstanciar = plataforma.transform.position;
            posparaInstanciar.y += 1f;
            GameObject torreInstanciada = Instantiate<GameObject>(prefabTorre.gameObject, posparaInstanciar, Quaternion.identity);
            torreInstanciada.transform.SetParent(plataforma.transform);
            lstTorresInstanciadas.Add(torreInstanciada);
        }
    }
    /** @hidden */
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