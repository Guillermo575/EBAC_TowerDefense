using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AdministradorTorres : MonoBehaviour
{
    private AdministradorToques referenciaAdminToques;
    private GameManager gameManager;
    private AdminSpawnerEnemigos[] referenciaSpawners;
    public GameObject Objetivo;
    public enum TorreSeleccionada
    {
        Torre1,Torrre2,Torre3,Torre4,Torre5,
    }
    public TorreSeleccionada torreSeleccionada;
    public List<GameObject> prefabTorres;
    private List<GameObject> lstTorresInstanciadas;
    public delegate void EnemigoObjetivoActualizado();
    public event EnemigoObjetivoActualizado EnEnemigoObjetivoActualizado;

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
    private void ActualizarObjetivo()
    {
        float distanciaMasCorta = float.MaxValue;
        _Enemy enemigoMasCercano = null;
        var lstEnemy = GameObject.FindObjectsByType<_Enemy>(FindObjectsSortMode.InstanceID);
        foreach (var enemigo in lstEnemy)
        {
            float dist = Vector3.Distance(enemigo.transform.position, Objetivo.transform.position);
            if (dist < distanciaMasCorta)
            {
                distanciaMasCorta = dist;
                enemigoMasCercano = enemigo;
            }
        }
        if (enemigoMasCercano != null)
        {
            foreach (GameObject torre in lstTorresInstanciadas)
            {
                torre.GetComponent<_Tower>().enemigo = enemigoMasCercano;
                torre.GetComponent<_Tower>().Disparar();
            }
            if (EnEnemigoObjetivoActualizado != null)
            {
                EnEnemigoObjetivoActualizado();
            }
        }
    }
    IEnumerator CourutineActualizarObjetivo()
    {
        while (gameManager.GetActualGameState() == GameManager.GameState.Action)
        {
            ActualizarObjetivo();
            yield return new WaitForSeconds(3);
        }
    }
    void Start()
    {
        gameManager = GameManager.GetManager();
        referenciaSpawners = gameManager.getSpawners();
        referenciaAdminToques = AdministradorToques.GetManager();
        referenciaAdminToques.EnPlataformaTocada += CrearTorre;
        gameManager.OnWaveStart += delegate { StartCoroutine(CourutineActualizarObjetivo()); };
        lstTorresInstanciadas = new List<GameObject>();
    }
    void Update()
    {     
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
}