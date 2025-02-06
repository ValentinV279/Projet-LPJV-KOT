using System.Collections;
using UnityEngine;

public class KnockDetection : MonoBehaviour
{
    public float knockRange = 2.0f; // Distance maximale pour frapper une surface
    public LayerMask knockableLayer; // Couches des objets que le joueur peut frapper
    public Transform playerHand; // Point d'origine du Raycast (ex : main du joueur)
    public AudioSource audioSource; // Source audio pour jouer les sons

    // Cooldown
    public float knockCooldown = 1.0f; // Temps entre chaque knock (en secondes)
    private bool canKnock = true; // Permet de savoir si le joueur peut frapper

    // Sounds
    public AudioClip missSound;
    public AudioClip knockStartSound;
    public AudioClip woodSound;
    public AudioClip metalSound;

    void Start()
    {
        // Vérifie que les références nécessaires sont assignées
        if (playerHand == null)
        {
            Debug.LogError("La référence 'playerHand' n'est pas assignée dans l'Inspector.");
        }

        if (audioSource == null)
        {
            Debug.LogError("La référence 'audioSource' n'est pas assignée dans l'Inspector.");
        }
    }

    void Update()
    {
        // Vérifie l'entrée pour frapper (souris + manette)
        if ((Input.GetMouseButtonDown(1) || Input.GetButtonDown("Knock")) && canKnock) 
        {
            StartKnock();
        }
    }

    void StartKnock()
    {
        // Joue le son de démarrage
        PlayKnockStartSound();

        // Gérer l'impact après le son de démarrage
        HandleKnock();

        // Lance le cooldown
        StartCoroutine(KnockCooldownRoutine());
    }

    void HandleKnock()
    {
        if (playerHand == null)
        {
            Debug.LogError("playerHand n'est pas assignée. Impossible d'effectuer le raycast.");
            return;
        }

        RaycastHit hit;
        if (Physics.Raycast(playerHand.position, playerHand.forward, out hit, knockRange, knockableLayer))
        {
            // Succès : une surface est détectée
            Debug.Log($"Knock réussi ! Surface détectée avec le tag : {hit.collider.tag}");

            // Joue un son basé sur le tag de la surface
            PlayKnockSound(hit.collider.tag);
        }
        else
        {
            // Échec : aucune surface détectée
            Debug.Log("Knock échoué : aucune surface détectée ou hors de portée.");

            // Joue le son d'échec
            PlayMissSound();
        }
    }

    void PlayKnockSound(string tag)
    {
        AudioClip clipToPlay = null;

        // Détecte le son selon le tag
        if (tag == "Bois")
        {
            clipToPlay = woodSound;
        }
        else if (tag == "Metal")
        {
            clipToPlay = metalSound;
        }

        if (clipToPlay != null)
        {
            audioSource?.PlayOneShot(clipToPlay);
        }
        else
        {
            Debug.LogWarning($"Aucun son assigné pour le tag '{tag}'.");
        }
    }

    void PlayMissSound()
    {
        if (missSound != null)
        {
            audioSource?.PlayOneShot(missSound);
        }
        else
        {
            Debug.LogWarning("Aucun son d'échec assigné.");
        }
    }

    void PlayKnockStartSound()
    {
        if (knockStartSound != null)
        {
            audioSource?.PlayOneShot(knockStartSound);
        }
        else
        {
            Debug.LogWarning("Aucun son de démarrage assigné.");
        }
    }

    IEnumerator KnockCooldownRoutine()
    {
        canKnock = false; // Bloque les knocks
        yield return new WaitForSeconds(knockCooldown); // Attend le temps du cooldown
        canKnock = true; // Débloque les knocks
    }
}
