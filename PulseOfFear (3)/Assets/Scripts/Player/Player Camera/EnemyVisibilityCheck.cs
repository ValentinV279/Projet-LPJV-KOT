using UnityEngine;

public class EnemyVisibilityCheck : MonoBehaviour
{
    public Transform enemy; // Référence à l'ennemi
    public Transform player; // Référence au joueur
    public float fieldOfViewAngle = 45f; // Angle du champ de vision (en degrés)
    public float maxActivationDistance = 10f; // Distance maximale pour désactiver l'ennemi
    public LayerMask visibilityLayer; // Couches visibles pour le raycast

    void Update()
    {
        // Debug pour visualiser le champ de vision
        Debug.DrawRay(player.position, player.forward * maxActivationDistance, Color.green);
    }

    public bool IsEnemyVisible()
    {
        if (enemy == null || player == null) return false;

        Vector3 directionToEnemy = enemy.position - player.position;
        float distanceToEnemy = directionToEnemy.magnitude;

        // Vérifie si l'ennemi est dans la distance maximale
        if (distanceToEnemy > maxActivationDistance)
        {
            Debug.Log("L'ennemi est trop loin pour être désactivé.");
            return false;
        }

        // Vérifie si l'ennemi est dans le champ de vision
        float angleToEnemy = Vector3.Angle(player.forward, directionToEnemy);
        if (angleToEnemy > fieldOfViewAngle)
        {
            Debug.Log("L'ennemi est hors du champ de vision.");
            return false;
        }

        // Vérifie s'il y a un obstacle entre le joueur et l'ennemi
        if (Physics.Raycast(player.position, directionToEnemy.normalized, out RaycastHit hit, maxActivationDistance, visibilityLayer))
        {
            if (hit.transform != enemy)
            {
                Debug.Log($"Un obstacle bloque la vision de l'ennemi. Obstacle : {hit.transform.name}");
                return false; // Obstacle détecté
            }
        }

        Debug.Log("L'ennemi est visible et à portée.");
        return true; // L'ennemi est visible et à moins de 10 mètres
    }
}
