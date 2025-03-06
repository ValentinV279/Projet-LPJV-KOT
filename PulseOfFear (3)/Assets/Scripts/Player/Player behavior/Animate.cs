using System.Collections;
using UnityEngine;

public class Animate : MonoBehaviour
{
    public Animator animator; // Public pour l'assigner manuellement
    public AudioSource clapAudioSource;
    public AudioClip clapSound;
    public float clapDuration = 1f;

    private Coroutine clapCoroutine;

    void Start()
    {
        if (animator == null)
            Debug.LogError("L'Animator n'est pas assigné. Assurez-vous de le lier dans l'Inspector.");
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (clapCoroutine != null)
                StopCoroutine(clapCoroutine);

            clapCoroutine = StartCoroutine(PerformClap());
        }
    }

    IEnumerator PerformClap()
    {
        if (animator != null)
        {
            animator.SetTrigger("Clap");
            PlayClapSound();
            yield return new WaitForSeconds(clapDuration);
        }
    }

    void PlayClapSound()
    {
        if (clapAudioSource != null && clapSound != null)
            clapAudioSource.PlayOneShot(clapSound);
    }
}