using System.Collections.Generic;
using UnityEngine;

public class GearSpawner : MonoBehaviour
{
    [System.Serializable]
    public class Room
    {
        public string roomName;
        public List<Transform> spawnPoints; // Liste des spawn points dans la pièce
    }

    [Header("Settings")]
    public List<Room> rooms; // Liste des pièces avec leurs spawn points
    public GameObject gearPrefab; // Prefab de l'engrenage

    void Start()
    {
        SpawnGears();
    }

    private void SpawnGears()
    {
        if (rooms == null || rooms.Count == 0)
        {
            Debug.LogError("Aucune pièce assignée dans GearSpawner.");
            return;
        }

        if (gearPrefab == null)
        {
            Debug.LogError("Prefab d'engrenage non assigné dans GearSpawner.");
            return;
        }

        foreach (Room room in rooms)
        {
            if (room.spawnPoints == null || room.spawnPoints.Count == 0)
            {
                Debug.LogWarning($"La pièce {room.roomName} n'a aucun spawn point défini.");
                continue;
            }

            // Sélection aléatoire d'un spawn point dans la pièce
            int randomIndex = Random.Range(0, room.spawnPoints.Count);
            Transform selectedSpawnPoint = room.spawnPoints[randomIndex];
            
            // Instancie l'engrenage au spawn point sélectionné
            Instantiate(gearPrefab, selectedSpawnPoint.position, selectedSpawnPoint.rotation);
        }
    }
}