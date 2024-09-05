using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using static AdministradorTorres;
public class _Tower : MonoBehaviour
{
    [HideInInspector] public GameManager gameManager;
    [HideInInspector] public _Enemy enemigo;
    public event EnemigoObjetivoActualizado EnEnemigoObjetivoActualizado;
    public GameObject prefabbala;
    public List<GameObject> puntasCanon;
    public int CostoInstalacion = 300;
    public float TiempoCadencia = 3f;
    public float DistanciaRango = 5f;
    private bool TorreActivada = false;
    public virtual void Start()
    {
        gameManager = GameManager.GetSingleton();
        gameManager.OnWaveStart += delegate { IniciarRutinaObjetivo(); };
        IniciarRutinaObjetivo();
    }
    void Update()
    {
        if (enemigo != null)
        {
            Apuntar();
        }
    }
    public void Apuntar()
    {
        transform.LookAt(enemigo.transform);
    }
    public virtual void Disparar()
    {
        foreach(GameObject punta in puntasCanon)
        {
            var tempBala = Instantiate<GameObject>(prefabbala, punta.transform.position, Quaternion.identity);
            tempBala.transform.LookAt(enemigo.transform);
            tempBala.GetComponent<Bala>().destino = enemigo.transform.position;
        }
    }
    private void ActualizarObjetivo()
    {
        float distanciaMasCorta = float.MaxValue;
        _Enemy enemigoMasCercano = null;
        var lstEnemy = GameObject.FindObjectsByType<_Enemy>(FindObjectsSortMode.InstanceID);
        foreach (var enemigo in lstEnemy)
        {
            float dist = Vector3.Distance(enemigo.transform.position, transform.position);
            if (dist <= DistanciaRango && dist < distanciaMasCorta)
            {
                distanciaMasCorta = dist;
                enemigoMasCercano = enemigo;
            }
        }
        if (enemigoMasCercano != null)
        {
            enemigo = enemigoMasCercano;
            Disparar();
            if (EnEnemigoObjetivoActualizado != null)
            {
                EnEnemigoObjetivoActualizado();
            }
        }
    }
    public void IniciarRutinaObjetivo()
    {
        if (TorreActivada) return;
        if (gameManager.GetActualGameState() == GameManager.GameState.Action)
        {
            StartCoroutine(CourutineActualizarObjetivo());
        }
    }
    IEnumerator CourutineActualizarObjetivo()
    {
        TorreActivada = true;
        while (gameManager.GetActualGameState() == GameManager.GameState.Action)
        {
            ActualizarObjetivo();
            yield return new WaitForSeconds(TiempoCadencia);
        }
        TorreActivada = false;
    }
}