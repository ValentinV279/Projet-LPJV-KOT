using UnityEngine;
using UnityEngine.SceneManagement;

public class CinematicManager : MonoBehaviour
{
    public float cinematicDuration = 60f; // Durée de la cinématique en secondes

    void Start()
    {
        // Démarre la transition automatique après la durée spécifiée
        Invoke("LoadLevel1", cinematicDuration);
    }

    void LoadLevel1()
    {
        // Charge la scène Level1
        SceneManager.LoadScene("Level1");
    }
}
