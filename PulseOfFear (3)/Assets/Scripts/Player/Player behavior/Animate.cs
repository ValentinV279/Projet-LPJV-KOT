using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animate : MonoBehaviour
{
    public AudioSource footstepAudioSource; // Audio source pour les bruits de pas (doit être distincte)
    public AudioSource clapAudioSource; // Nouvelle AudioSource dédiée au clap
    public AudioClip clapSound; // Son du clap

    public GameObject walkingArms;
    public GameObject clappingArms;
    public float clapDuration = 1f; // Durée pendant laquelle les bras de clap sont visibles

    private Coroutine clapCoroutine;

    void Start()
    {
        if (clapAudioSource == null)
        {
            Debug.LogError("La référence 'clapAudioSource' n'est pas assignée dans l'Inspector.");
        }

        if (clapSound == null)
        {
            Debug.LogWarning("Aucun son de clap assigné dans l'Inspector.");
        }

        // Initialiser les modèles d'animation
        walkingArms.SetActive(false);
        clappingArms.SetActive(false);
    }

    void Update()
    {
        bool isWalking = Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0;
        bool isClapping = Input.GetMouseButtonDown(0) || Input.GetButtonDown("Clap");

        if (isClapping)
        {
            if (clapCoroutine != null)
            {
                StopCoroutine(clapCoroutine);
            }
            clapCoroutine = StartCoroutine(PerformClap());
        }
        else if (!clappingArms.activeSelf) // Ne réaffiche les bras de marche que si le clap n'est pas actif
        {
            walkingArms.SetActive(isWalking);
        }
    }

    IEnumerator PerformClap()
    {
        walkingArms.SetActive(false);
        clappingArms.SetActive(true);

        PlayClapSound();
        
        yield return new WaitForSeconds(clapDuration);
        
        clappingArms.SetActive(false);
    }

    void PlayClapSound()
    {
        if (clapAudioSource != null && clapSound != null)
        {
            clapAudioSource.PlayOneShot(clapSound);
        }
        else
        {
            Debug.LogWarning("Impossible de jouer le son du clap. Vérifiez que 'clapAudioSource' et 'clapSound' sont assignés.");
        }
    }
}
