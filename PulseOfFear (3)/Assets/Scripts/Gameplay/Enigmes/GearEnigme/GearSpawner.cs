using System.Collections.Generic;
using UnityEngine;

public class GearSpawner : MonoBehaviour
{
    [Header("Settings")]
    public List<Transform> gearSpawnPoints; // Liste des spawn points
    public GameObject gearPrefab; // Prefab de l'engrenage

    [Range(1, 3)]
    public int numberOfGearsToSpawn = 2; // Nombre d'engrenages à faire apparaître

    void Start()
    {
        SpawnGears();
    }

    private void SpawnGears()
    {
        if (gearSpawnPoints == null || gearSpawnPoints.Count == 0)
        {
            Debug.LogError("Aucun point de spawn assigné dans GearSpawner.");
            return;
        }

        if (gearPrefab == null)
        {
            Debug.LogError("Prefab d'engrenage non assigné dans GearSpawner.");
            return;
        }

        // Copie des spawn points pour éviter les doublons
        List<Transform> availableSpawnPoints = new List<Transform>(gearSpawnPoints);

        for (int i = 0; i < numberOfGearsToSpawn; i++)
        {
            if (availableSpawnPoints.Count == 0)
            {
                Debug.LogWarning("Nombre de spawn points insuffisant pour le nombre d'engrenages à générer.");
                break;
            }

            // Sélectionne un spawn point aléatoire
            int randomIndex = Random.Range(0, availableSpawnPoints.Count);
            Transform spawnPoint = availableSpawnPoints[randomIndex];

            // Instancie l'engrenage au spawn point
            Instantiate(gearPrefab, spawnPoint.position, spawnPoint.rotation);

            // Retire le spawn point utilisé
            availableSpawnPoints.RemoveAt(randomIndex);
        }
    }
}
