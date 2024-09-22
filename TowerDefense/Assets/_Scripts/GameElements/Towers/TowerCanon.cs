using UnityEngine;
/**
 * @file
 * @brief Clase que modifica la clase torre para funcionar de forma distinta disparando proyectiles en base a la fisica
 */
public class TowerCanon : _Tower
{
    public override void Disparar()
    {
        ShotBullet();
    }
    public void ShotBullet()
    {
        var ang = CalculateLaunchDirection(transform.position, enemigo.transform.position, 20f);
        GameObject temp = Instantiate(prefabbala.gameObject, puntasCanon[0].transform.position, transform.rotation);
        Rigidbody tempRB = temp.GetComponent<Rigidbody>();
        tempRB.velocity = ang;
    }
    public Vector3 CalculateLaunchDirection(Vector3 cannonPosition, Vector3 enemyPosition, float projectileSpeed)
    {
        Vector3 direction = enemyPosition - cannonPosition;
        float horizontalDistance = new Vector3(direction.x, 0, direction.z).magnitude;
        float heightDifference = direction.y;
        float g = Mathf.Abs(Physics.gravity.y);
        float angle = Mathf.Atan2(heightDifference, horizontalDistance);
        float velocityX = Mathf.Sqrt(horizontalDistance * g / Mathf.Sin(2 * angle));
        float velocityY = Mathf.Sqrt(g * heightDifference + 0.5f * g * horizontalDistance * Mathf.Tan(angle));
        Vector3 launchDirection = new Vector3(direction.x, heightDifference, direction.z).normalized;
        Vector3 launchVelocity = launchDirection * projectileSpeed;
        return launchVelocity;
    }
}