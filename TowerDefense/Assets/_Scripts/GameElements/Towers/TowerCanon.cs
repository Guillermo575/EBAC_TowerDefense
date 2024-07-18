using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Timeline.TimelinePlaybackControls;
public class TowerCanon : _Tower
{
    public override void Disparar()
    {
        ShotBullet();
    }
    public void ShotBullet()
    {
        var rotacion = ObtenerAngulo(enemigo.transform.position - transform.position, 20);
        Vector3 v = new Vector3(rotacion, 90, 0.0f);
        Vector3 direccionDisparo = transform.rotation.eulerAngles;
        direccionDisparo.y = 90 - direccionDisparo.x;
        GameObject temp = Instantiate(prefabbala.gameObject, puntasCanon[0].transform.position, transform.rotation);
        Rigidbody tempRB = temp.GetComponent<Rigidbody>();
        tempRB.velocity = direccionDisparo.normalized * 20;
    }
    private float ObtenerAngulo(Vector3 Destino, float v, float g = 9.8f)
    {
        float g2 = g / 2;
        float t = v / g2;
        float senXcos = (Destino.x * g2) / Mathf.Pow(v, 2);
        float ang = Mathf.Asin(senXcos * 2) / 2;
        ang = ang * (180 / Mathf.PI);
        return ang;
    }
    private float ObtenerAngulo2(Vector3 Origen, Vector3 Destino, float v, float g = 9.8f)
    {
        float g2 = g / 2;
        float t = g2 * (Mathf.Pow(Destino.x, 2) / Mathf.Pow(v, 2));
        float tgx2 = -t;
        float tgx = Destino.x;
        float tg = -t + (Origen.y - Destino.y);
        var tcpSqrt = Mathf.Sqrt(Mathf.Pow(tgx, 2) - (4 * (tgx2 * tg)));
        var tcp = (-tgx + tcpSqrt) / (2 * tgx2);
        var tcp2 = (-tgx - tcpSqrt) / (2 * tgx2);
        float ang = Mathf.Asin(tcp);
        ang = ang * (180 / Mathf.PI);
        return ang;
    }
}