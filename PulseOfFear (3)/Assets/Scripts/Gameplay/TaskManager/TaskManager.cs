using System.Collections;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    public static TaskManager Instance; // Singleton pour accès global
    public int TasksComplete = 0; // Nombre de tâches terminées
    public int TotalTasks = 4; // Nombre total de tâches à accomplir
    public ExitDoubleDoor[] exitDoors; // Référence aux portes de sortie
    public Camera playerCamera; // Caméra du joueur
    public Camera exitDoorCamera; // Caméra de la porte de sortie
    public static bool AllTasksCompleted = false; // Indique si toutes les tâches sont terminées

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // S'assurer que la caméra de la porte de sortie est désactivée au début
        if (exitDoorCamera != null)
        {
            exitDoorCamera.enabled = false;
        }
    }

    public void CompleteTask()
    {
        TasksComplete++;
        Debug.Log($"Tâche complétée : {TasksComplete}/{TotalTasks}");
        
        if (TasksComplete >= TotalTasks)
        {
            AllTasksCompleted = true;
            StartCoroutine(ShowExitDoor());
        }
    }

    private IEnumerator ShowExitDoor()
    {
        Debug.Log("Toutes les tâches sont complétées. Ouverture des portes de sortie !");
        
        // Désactiver la caméra du joueur et activer celle de la porte
        if (playerCamera != null && exitDoorCamera != null)
        {
            playerCamera.enabled = false;
            exitDoorCamera.enabled = true;
        }

        // Ouvrir immédiatement les portes pendant que la caméra de sortie est active
        foreach (var door in exitDoors)
        {
            if (door != null)
            {
                door.ToggleDoor();
            }
        }
        
        yield return new WaitForSeconds(4f);
        
        // Réactiver la caméra du joueur
        if (playerCamera != null && exitDoorCamera != null)
        {
            exitDoorCamera.enabled = false;
            playerCamera.enabled = true;
        }
    }
}
