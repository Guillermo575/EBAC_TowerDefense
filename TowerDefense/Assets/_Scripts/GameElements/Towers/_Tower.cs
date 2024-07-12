using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class _Tower : MonoBehaviour
{
    public _Enemy enemigo;
    public GameObject prefabbala;
    public List<GameObject> puntasCanon;
    public int CostoInstalacion = 300;
    void Start()
    {
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
            tempBala.GetComponent<Bala>().destino = enemigo.transform.position;
        }
    }
}