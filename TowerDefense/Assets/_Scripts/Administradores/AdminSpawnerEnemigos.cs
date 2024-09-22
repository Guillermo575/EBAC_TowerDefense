using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
/**
 * @file
 * @brief Clase que administra la aparicion de enemigos en una zona de juego especifica
 */
public class AdminSpawnerEnemigos : MonoBehaviour
{
    #region Variables
    /** @hidden */
    private GameManager gameManager;
    /** Cada elemento de la lista es una horda y cada horda tendra la configuracion de enemigos que apareceran */
    public List<HordaEnemigos> ConfigHorda;
    /** Objeto que representa el lugar donde apareceran los enemigos */
    public GameObject gameBase;
    #endregion

    #region Getters
    /** Obtiene la horda que se esta desplegando*/
    public HordaEnemigos getHordaActual()
    {
        if (gameManager.GetRondaActual() >= ConfigHorda.Count) return null;
        return ConfigHorda[gameManager.GetRondaActual()];
    }
    /** Indica si la oleada acaba de iniciar*/
    public bool getOleadaIniciada()
    {
        return !getOleadaFinalizada();
    }
    /** Indica cuandas hordas posee*/
    public int getRondasTotales()
    {
        return ConfigHorda.Count;
    }
    /** Indica si ya no hay mas hordas por desplegar*/
    public bool getOleadaFinalizada()
    {
        if(getHordaActual() == null) return false;
        return gameManager.RondaFinal() && getHordaActual().enemigosDuranteEstaOleada <= 0;
    }
    #endregion

    #region Start & Update
    /** @hidden */
    void Start()
    {
        gameManager = GameManager.GetSingleton();
        gameManager.OnWaveStart += delegate { IniciarOla(); };
        foreach (var objHorda in ConfigHorda)
        {
            objHorda.Initialize();
        }
    }
    /** @hidden */
    void Update()
    {
        if (getHordaActual().enemigosPorOleada == 0)
        {
            gameBase.SetActive(false);
        }
        else
        {
            gameBase.SetActive(true);
        }
    }
    #endregion

    #region General
    /** Iniciar la corutina que crea los enemigos */
    public void IniciarOla()
    {
        if (!getOleadaFinalizada())
        {
            //InstanciarEnemigo();
            StartCoroutine(CourutineSpawn());
        }
    }
    /** Corutina que se encarga de la creacion de enemigos, el tiempo de espera y el enemigo dependera de como se configuro en el objeto ConfigHorda*/
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
    }
    /** @hidden */
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
    /** Clase que contiene la forma en que iran apareciendo los enemigos */
    [Serializable]
    public class HordaEnemigos
    {
        /** Lista de la clase SpawnHorda*/
        public List<SpawnHorda> lstEnemigos;
        /** indica cuantos enemigos apareceran en esta Horda */
        public int enemigosPorOleada;
        /** Tiempo minimo de espera para la aparicion del enemigo */
        public float TiempoEsperaSpawnMinimo = 2f;
        /** Tiempo maximo de espera para la aparicion del enemigo */
        public float TiempoEsperaSpawnMaximo = 2f;
        /** Indica si aun quedan enemigos por desplegar*/
        public bool EnemigosPendientes { get { return enemigosDuranteEstaOleada > 0; } }
        /** @hidden*/
        internal int[] IndexHorda;
        /** @hidden*/
        internal int SumaPesos;
        /** @hidden*/
        internal int enemigosDuranteEstaOleada;
        /** Clase que guarda los datos del enemigo
         @param prefab: el objeto fisico que contiene los datos del enemigo
         @param Peso: Los enemigos que apareceran tendran una aleatoridad y entre mayor sea este numero mas probabilidades tiene de que se despliegue
         @param PosicionFijo: numero en el que quiere que aparezca de forma obligatoria el enemigo
         */
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
        /** Clase que hace los calculos de aleatoridad para indicar cuales enemigos y en que orden se crearan*/
        public void Initialize()
        {
            TiempoEsperaSpawnMinimo = TiempoEsperaSpawnMinimo <= 0 ? 0.5f : TiempoEsperaSpawnMinimo;
            TiempoEsperaSpawnMaximo = TiempoEsperaSpawnMaximo <= 0 ? 0.5f : TiempoEsperaSpawnMaximo;
            var gameManager = GameManager.GetSingleton();
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