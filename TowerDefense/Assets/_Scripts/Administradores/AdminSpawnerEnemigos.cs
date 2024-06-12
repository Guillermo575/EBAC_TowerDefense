using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class AdminSpawnerEnemigos : MonoBehaviour
{
    public List<GameObject> prefabsEnemigos;
    public int oleada;
    public List<int> enemigosPorOleada;
    private int enemigosDuranteEstaOleada;
    public delegate void OleadaTerminada();
    public event OleadaTerminada EnOleadaTerminada;
    public SceneObjective referenciaObjetivo;
    void Start()
    {
        oleada = 0;
        ConfigurarCantidadDenemigos();
        InstanciarEnemigo();
    }
    void Update()
    {
    }
    public void TerminarOla()
    {
        if (EnOleadaTerminada != null)
        {
            EnOleadaTerminada();
        }
    }
    public void ConfigurarCantidadDenemigos()
    {
        enemigosDuranteEstaOleada = enemigosPorOleada[oleada];
    }
    public void InstanciarEnemigo()
    {
        int indiceAleatorio = Random.Range(0, prefabsEnemigos.Count);
        var obj = Instantiate<GameObject>(prefabsEnemigos[indiceAleatorio], transform.position, Quaternion.identity);
        obj.GetComponent<_Enemy>().objetivo = referenciaObjetivo.gameObject;
        enemigosDuranteEstaOleada--;
        if (enemigosDuranteEstaOleada < 0)
        {
            oleada++;
            ConfigurarCantidadDenemigos();
            TerminarOla();
            return;
        }
        Invoke("InstanciarEnemigo", 2);
    }
}