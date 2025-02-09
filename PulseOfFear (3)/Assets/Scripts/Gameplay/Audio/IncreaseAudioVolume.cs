using UnityEngine;

public class IncreaseAudioVolume : MonoBehaviour
{
    public AudioSource audioSource; // Référence à l'AudioSource
    public float targetVolume = 1.0f; // Volume final souhaité
    public float increaseSpeed = 0.1f; // Vitesse d'augmentation du volume

    void Update()
    {
        // Assurez-vous qu'il y a une AudioSource assignée
        if (audioSource != null)
        {
            // Augmente progressivement le volume vers la valeur cible
            if (audioSource.volume < targetVolume)
            {
                audioSource.volume += increaseSpeed * Time.deltaTime;

                // S'assurer que le volume ne dépasse pas la valeur cible
                if (audioSource.volume > targetVolume)
                {
                    audioSource.volume = targetVolume;
                }
            }
        }
    }
}
