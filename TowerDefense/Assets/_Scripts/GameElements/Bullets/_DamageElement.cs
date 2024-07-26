using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class _DamageElement : MonoBehaviour, IAtacante
{
    protected GameManager gameManager;
    List<string> damageLog = new List<string>();
    public int damage = 1;
    public Vector3 destino;
    public float velocidad = 20;
    public GameObject enemigo;

    void Start()
    {
        gameManager = GameManager.GetManager();
    }
    public void MakeDamage(int damage = 0)
    {
        var Damagedbefore = (from x in damageLog where x == enemigo.gameObject.GetInstanceID().ToString() select x).Count();
        if (Damagedbefore > 0) return;
        damageLog.Add(enemigo.gameObject.GetInstanceID().ToString());
        enemigo.GetComponent<_Enemy>().ReceiveDamage(damage);
    }
    private void OnTriggerEnter(Collider other)
    {
        CheckCollision(other.gameObject);
    }
    private void OnCollisionEnter(Collision collision)
    {
        CheckCollision(collision.gameObject);
    }
    public virtual void CheckCollision(GameObject gameObject)
    {
        if (gameObject.tag == "Enemigo")
        {
            enemigo = gameObject;
            MakeDamage(damage);
        }
    }
}