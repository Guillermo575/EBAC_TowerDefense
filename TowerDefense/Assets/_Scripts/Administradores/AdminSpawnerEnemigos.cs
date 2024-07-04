using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
public class AdminSpawnerEnemigos : MonoBehaviour
{
    #region Variables
    private GameManager gameManager;
    public List<HordaEnemigos> ConfigHorda;
    #endregion

    #region Getters
    public HordaEnemigos getHordaActual()
    {
        if (gameManager.GetRondaActual() >= ConfigHorda.Count) return null;
        return ConfigHorda[gameManager.GetRondaActual()];
    }
    public bool getOleadaIniciada()
    {
        return !getOleadaFinalizada();
    }
    public int getRondasTotales()
    {
        return ConfigHorda.Count;
    }
    public bool getOleadaFinalizada()
    {
        if(getHordaActual() == null) return false;
        return gameManager.RondaFinal() && getHordaActual().enemigosDuranteEstaOleada <= 0;
    }
    #endregion

    #region Start & Update
    void Start()
    {
        gameManager = GameManager.GetManager();
        gameManager.OnWaveStart += delegate { IniciarOla(); };
        foreach (var objHorda in ConfigHorda)
        {
            objHorda.Initialize();
        }
    }
    void Update()
    {
    }
    #endregion

    #region General
    public void IniciarOla()
    {
        if (!getOleadaFinalizada())
        {
            //InstanciarEnemigo();
            StartCoroutine(CourutineSpawn());
        }
    }
    IEnumerator CourutineSpawn()
    {
        var HordaActual = getHordaActual();
        foreach (var indexElegido in HordaActual.IndexHorda)
        {
            var tiempoEspera = gameManager.mathRNG.GetRandom(HordaActual.TiempoEsperaSpawnMinimo, HordaActual.TiempoEsperaSpawnMaximo);
            yield return new WaitForSeconds(tiempoEspera);
            var PrefabElegido = HordaActual.lstEnemigos[indexElegido].prefab;
            var obj = Instantiate<GameObject>(PrefabElegido, transform.position, Quaternion.identity);
            HordaActual.enemigosDuranteEstaOleada--;
        }
        yield return new WaitForSeconds(0.1f);
    }
    public void InstanciarEnemigo()
    {
        var HordaActual = getHordaActual();
        if (HordaActual.IndexHorda.Length <= 0) return;
        var indexElegido = HordaActual.IndexHorda[HordaActual.enemigosPorOleada - HordaActual.enemigosDuranteEstaOleada];
        var PrefabElegido = HordaActual.lstEnemigos[indexElegido].prefab;
        var obj = Instantiate<GameObject>(PrefabElegido, transform.position, Quaternion.identity);
        //obj.GetComponent<_Enemy>().objetivo = gameManager.referenciaObjetivo.gameObject;
        HordaActual.enemigosDuranteEstaOleada--;
        if (HordaActual.enemigosDuranteEstaOleada <= 0)
        {
            return;
        }
        var tiempoEspera = gameManager.mathRNG.GetRandom(HordaActual.TiempoEsperaSpawnMinimo, HordaActual.TiempoEsperaSpawnMaximo);
        Invoke("InstanciarEnemigo", tiempoEspera);
    }
    #endregion

    #region Class HordaEnemigos
    [Serializable]
    public class HordaEnemigos
    {
        public List<SpawnHorda> lstEnemigos;
        public int enemigosPorOleada;
        public float TiempoEsperaSpawnMinimo = 2f;
        public float TiempoEsperaSpawnMaximo = 2f;
        public bool EnemigosPendientes { get { return enemigosDuranteEstaOleada > 0; } }
        internal int[] IndexHorda;
        internal int SumaPesos;
        internal int enemigosDuranteEstaOleada;
        [Serializable]
        public class SpawnHorda
        {
            public GameObject prefab;
            public int Peso;
            public List<int> PosicionFijo;
            internal int Index;
            internal int RangoMinimo;
            internal int RangoMaximo;
        }
        public void Initialize()
        {
            TiempoEsperaSpawnMinimo = TiempoEsperaSpawnMinimo <= 0 ? 0.5f : TiempoEsperaSpawnMinimo;
            TiempoEsperaSpawnMaximo = TiempoEsperaSpawnMaximo <= 0 ? 0.5f : TiempoEsperaSpawnMaximo;
            var gameManager = GameManager.GetManager();
            enemigosDuranteEstaOleada = enemigosPorOleada;
            int RangoAnterior = 0;
            for (int l = 0; l < lstEnemigos.Count; l++)
            {
                var objEnemigo = lstEnemigos[l];
                objEnemigo.Index = l;
                if (objEnemigo.Peso > 0)
                {
                    objEnemigo.RangoMinimo = RangoAnterior;
                    objEnemigo.RangoMaximo = objEnemigo.RangoMinimo + objEnemigo.Peso - 1;
                    RangoAnterior += objEnemigo.Peso;
                    SumaPesos = objEnemigo.RangoMaximo;
                }
                else
                {
                    objEnemigo.RangoMinimo = -1;
                    objEnemigo.RangoMaximo = -1;
                }
            }
            var lstEnemigosPosicionFija = (from x in lstEnemigos where x.PosicionFijo != null select x);
            IndexHorda = Enumerable.Repeat<int>(-1, enemigosPorOleada).ToArray();
            foreach (var objFijo in lstEnemigosPosicionFija)
            {
                foreach (var objPos in objFijo.PosicionFijo)
                {
                    if (objPos < IndexHorda.Length)
                    {
                        IndexHorda[objPos] = objFijo.Index;
                    }
                }
            }
            for (int l = 0; l < IndexHorda.Length; l++)
            {
                if (IndexHorda[l] == -1)
                {
                    //int indiceAleatorio = UnityEngine.Random.Range(0, SumaPesos);
                    int indiceAleatorio = gameManager.mathRNG.GetRandom(0, SumaPesos);
                    var objEnemigo = (from x in lstEnemigos where x.RangoMinimo <= indiceAleatorio && indiceAleatorio <= x.RangoMaximo select x).ToList();
                    if (objEnemigo.Count > 0)
                    {
                        IndexHorda[l] = objEnemigo.First().Index;
                    }
                    else
                    {
                        IndexHorda[l] = 0;
                    }
                }
            }
        }
    }
    #endregion
}