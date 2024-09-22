using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/**
 * @file
 * @brief Clase que modifica la clase torre para funcionar de forma distinta
 */
public class TowerAntena : _Tower, IAtacante
{
    #region Variables
    public float divisionesRayo = 10;
    private LineRenderer LRRayo;
    public List<Vector3> puntos;
    public int potenciaRayo;
    public float DuracionDisparo = 1f;
    public MathRNG mathRNG = new MathRNG(1121);
    private bool TimeToStopThis = false;
    private bool ShootStoped = false;
    #endregion

    public override void Start()
    {
        base.Start();
        LRRayo = GetComponent<LineRenderer>();
    }
    private void FixedUpdate()
    {
        if (enemigo != null && enemigo.vida > 0)
        {
            DispararRayo();
        }
        else
        {
            LRRayo.positionCount = 0;
            ShootStoped = false;
            TimeToStopThis = false;
            StopCoroutine(CourutineDetenerDisparo());
        }
    }
    public override void Disparar()
    {
    }
    public void DispararRayo()
    {
        if (!TimeToStopThis)
        {
            TimeToStopThis = true;
            StartCoroutine(CourutineDetenerDisparo());
        }
        if (!ShootStoped)
        {
            puntos = ObtenerPuntos();
            puntos.Insert(0, puntasCanon[0].transform.position);
            var posEnemigo = enemigo.transform.position;
            posEnemigo.y += 1;
            puntos.Add(posEnemigo);
            LRRayo.positionCount = puntos.Count;
            LRRayo.SetPositions(puntos.ToArray());
            MakeDamage(potenciaRayo);
        }
    }

    IEnumerator CourutineDetenerDisparo()
    {
        yield return new WaitForSeconds(DuracionDisparo);
        TimeToStopThis = false;
        ShootStoped = true;
        enemigo = null;
        yield return new WaitForSeconds(TiempoCadencia);
        ShootStoped = false;
    }

    private List<Vector3> ObtenerPuntos()
    {
        if (divisionesRayo == 0)
        {
            return null;
        }
        List<Vector3> puntosTemporales = new List<Vector3>();
        float divisor = 1f / divisionesRayo;
        float linear = 0f;
        bool esPositivo = false;
        if (divisionesRayo == 1)
        {
            var punto = Vector3.Lerp(puntasCanon[0].transform.position, enemigo.transform.position, 0.5f);
            puntosTemporales.Add(punto);
            return puntosTemporales;
        }
        for (int l = 0; l < divisionesRayo; l++)
        {
            if (l == 0)
            {
                linear = divisor / 2;
            }
            else
            {
                linear += divisor;
            }
            var punto = Vector3.Lerp(puntasCanon[0].transform.position, enemigo.transform.position, linear);
            if (esPositivo)
            {
                punto.x += (float)(mathRNG.GetRandom() * 2);
                esPositivo = false;
            }
            else
            {
                punto.x -= (float)(mathRNG.GetRandom() * 2);
                esPositivo = true;
            }
            puntosTemporales.Add(punto);
        }
        return puntosTemporales;
    }

    public void MakeDamage(int damage = 0)
    {
        enemigo.GetComponent<_Enemy>().ReceiveDamage(damage);
    }
}