using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
        //var ang = CalculateProjectileVelocity(transform.position, enemigo.transform.position, 20f);
        GameObject temp = Instantiate(prefabbala.gameObject, puntasCanon[0].transform.position, transform.rotation);
        temp.transform.LookAt(enemigo.transform);
        var ang = CalculateLaunchAngle(transform.position, enemigo.transform.position, 20f);
        Vector3 angv3 = new Vector3();
        angv3.x = temp.transform.rotation.x;
        angv3.y = ang;
        angv3.z = temp.transform.rotation.z;
        Rigidbody tempRB = temp.GetComponent<Rigidbody>();
        //tempRB.velocity = CalculateVelocity(enemigo.transform.position, puntasCanon[0].transform.position, 45f);
        tempRB.velocity = new Vector3(20f, 0f, 20f);
    }
    public Vector3 CalculateVelocity(Vector3 targetPosition, Vector3 cannonPosition, float angle)
    {
        float gravity = Physics.gravity.y/2;
        float targetDistance = Vector3.Distance(targetPosition, cannonPosition);
        float velocity = Mathf.Sqrt(targetDistance * -gravity / (Mathf.Sin(2 * angle * Mathf.Deg2Rad)));
        Vector3 direction = (targetPosition - cannonPosition).normalized;
        Vector3 velocityVector = velocity * direction;
        velocityVector.y = velocity * Mathf.Sin(angle * Mathf.Deg2Rad);
        return velocityVector;
    }
    public Vector3 CalculateProjectileVelocity(Vector3 cannonPosition, Vector3 enemyPosition, float projectileVelocity)
    {
        float g = Physics.gravity.y;
        float deltaX = enemyPosition.x - cannonPosition.x;
        float deltaY = enemyPosition.y - cannonPosition.y;
        float distance = Mathf.Sqrt(Mathf.Pow(deltaX, 2) + Mathf.Pow(deltaY, 2));
        if (distance <= 0)
            return Vector3.zero;

        // Usamos la fórmula para calcular el ángulo
        // theta = atan((v^2 + sqrt(v^4 - g(gx^2 + v^2y^2))) / (gx))
        float vSquared = Mathf.Pow(projectileVelocity, 2);
        float discriminant = Mathf.Pow(vSquared, 2) - g * (g * (deltaX * deltaX) + 2 * deltaY * vSquared);
        if (discriminant < 0)
            return Vector3.zero;

        float angle = Mathf.Atan((vSquared + Mathf.Sqrt(discriminant)) / (g * deltaX));
        Vector3 velocity = new Vector3(deltaX, 0, deltaY).normalized; // Normalizar para obtener la dirección
        velocity = Quaternion.Euler(0, angle * Mathf.Rad2Deg, 0) * velocity; // Aplicar rotación para dirección

        // Calcular la velocidad total considerando la velocidad en Y
        float verticalVelocity = projectileVelocity * Mathf.Sin(angle);
        float horizontalVelocity = projectileVelocity * Mathf.Cos(angle);

        // Crear el Vector3 como velocidad en X, Y, Z
        Vector3 finalVelocity = new Vector3(horizontalVelocity, verticalVelocity, horizontalVelocity) * velocity.magnitude;

        return finalVelocity;
    }
    // Método para calcular el ángulo de lanzamiento
    public float CalculateLaunchAngle(Vector3 cannonPosition, Vector3 enemyPosition, float projectileSpeed)
    {
        // Gravedad
        float g = Physics.gravity.y; // En Unity, la gravedad se toma en negativo

        // Vector entre el cañón y el enemigo
        Vector3 deltaPosition = enemyPosition - cannonPosition;
        float dx = deltaPosition.x; // Distancia horizontal
        float dy = deltaPosition.y; // Distancia vertical

        // No se puede calcular si la distancia es cero
        if (dx <= 0)
        {
            Debug.LogWarning("La distancia horizontal debe ser mayor que cero.");
            return 0f;
        }

        // Aplicamos la fórmula para calcular el ángulo
        float angle = Mathf.Atan2(dy, dx) * Mathf.Rad2Deg; // Convertimos a grados

        // Resolvemos la ecuación para obtener el ángulo de lanzamiento
        float v2 = projectileSpeed * projectileSpeed;

        float discriminant = v2 * v2 - g * (g * dx * dx + 2 * dy * v2);
        if (discriminant < 0)
        {
            Debug.LogWarning("No es posible alcanzar el objetivo con la velocidad dada.");
            return 0f;
        }
        float theta1 = Mathf.Atan((v2 + Mathf.Sqrt(discriminant)) / (g * dx));
        float theta2 = Mathf.Atan((v2 - Mathf.Sqrt(discriminant)) / (g * dx));
        angle = theta1 * Mathf.Rad2Deg;
        return theta1;
    }
}