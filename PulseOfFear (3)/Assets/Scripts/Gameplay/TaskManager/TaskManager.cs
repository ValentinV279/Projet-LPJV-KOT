using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TaskManager : MonoBehaviour
{
    public static TaskManager Instance; // Singleton pour accès global
    public int TasksComplete = 0; // Nombre de tâches terminées
    public int TotalTasks = 4; // Nombre total de tâches à accomplir
    public ExitDoubleDoor[] exitDoors; // Référence aux portes de sortie
    public Camera playerCamera; // Caméra du joueur
    public Camera exitDoorCamera; // Caméra de la porte de sortie
    public GameObject cursorImage1; // Premier GameObject du curseur
    public GameObject cursorImage2; // Deuxième GameObject du curseur
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

        // Activer les curseurs au début
        if (cursorImage1 != null)
        {
            cursorImage1.SetActive(true);
        }
        if (cursorImage2 != null)
        {
            cursorImage2.SetActive(true);
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

        // Désactiver la caméra du joueur, activer celle de la porte et cacher les curseurs
        if (playerCamera != null && exitDoorCamera != null)
        {
            playerCamera.enabled = false;
            exitDoorCamera.enabled = true;
        }
        
        // Désactiver complètement les GameObjects des curseurs
        if (cursorImage1 != null)
        {
            cursorImage1.SetActive(false);
        }
        if (cursorImage2 != null)
        {
            cursorImage2.SetActive(false);
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
        
        // Réactiver la caméra du joueur et les curseurs
        if (playerCamera != null && exitDoorCamera != null)
        {
            exitDoorCamera.enabled = false;
            playerCamera.enabled = true;
        }
        
        // Réactiver les GameObjects des curseurs
        if (cursorImage1 != null)
        {
            cursorImage1.SetActive(true);
        }
        if (cursorImage2 != null)
        {
            cursorImage2.SetActive(true);
        }
    }
}
