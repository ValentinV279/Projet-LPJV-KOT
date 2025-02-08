using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndLevelTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Vérifie si l'objet entrant est le joueur
        if (other.CompareTag("Player"))
        {
            Debug.Log("Le joueur a atteint la fin du niveau. Chargement de la scène 'Credits'.");
            SceneManager.LoadScene("Credits");
        }
    }
}
