using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    public bool isActivated = false;  // Indique si la plaque est activée
    public AudioSource audioSource;   // Source audio pour le son de la plaque
    public AudioClip activateSound;   // Son joué lorsque la plaque est activée
    public AudioClip deactivateSound; // Son joué lorsque la plaque est désactivée

    private void Start()
    {
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Quand un objet entre dans la zone de la plaque, on l'active
        if (!isActivated)
        {
            isActivated = true;
            Debug.Log($"Un objet a activé la plaque de pression : {gameObject.name}");
            PlaySound(activateSound);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Quand un objet sort de la zone de la plaque, on la désactive
        if (isActivated)
        {
            isActivated = false;
            Debug.Log($"Un objet a désactivé la plaque de pression : {gameObject.name}");
            PlaySound(deactivateSound);
        }
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
