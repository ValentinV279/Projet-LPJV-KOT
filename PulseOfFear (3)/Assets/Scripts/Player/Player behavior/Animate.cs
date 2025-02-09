using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animate : MonoBehaviour
{
    public AudioSource footstepAudioSource; // Audio source pour les bruits de pas (doit être distincte)
    public AudioSource clapAudioSource; // Nouvelle AudioSource dédiée au clap
    public AudioClip clapSound; // Son du clap

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();

        if (animator == null)
        {
            Debug.LogError("Aucun Animator trouvé sur cet objet.");
        }

        if (clapAudioSource == null)
        {
            Debug.LogError("La référence 'clapAudioSource' n'est pas assignée dans l'Inspector.");
        }

        if (clapSound == null)
        {
            Debug.LogWarning("Aucun son de clap assigné dans l'Inspector.");
        }
    }

    void Update()
    {
        // Vérifie l'entrée du clap via le clavier (Clic gauche) ou la manette (joystick button 6)
        if (Input.GetMouseButtonDown(0) || Input.GetButtonDown("Clap"))
        {
            PerformClap();
        }
    }

    void PerformClap()
    {
        if (animator != null && animator.HasParameter("ClapTrigger"))
        {
            animator.SetTrigger("ClapTrigger");
        }
        else
        {
            Debug.LogError("Le paramètre 'ClapTrigger' est introuvable dans l'Animator.");
        }

        PlayClapSound();
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

// Vérification améliorée de l’existence du paramètre dans l’Animator
public static class AnimatorExtensions
{
    public static bool HasParameter(this Animator animator, string paramName)
    {
        foreach (AnimatorControllerParameter param in animator.parameters)
        {
            if (param.name == paramName)
            {
                return true;
            }
        }
        return false;
    }
}
