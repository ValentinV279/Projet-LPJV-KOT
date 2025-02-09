using System.Collections;
using UnityEngine;

public class ExitDoubleDoor : MonoBehaviour
{
    public float rotationSpeed = 35f;
    public float finalDecelerationFactor = 0.1f;
    public float rotationDuration = 3f;
    public float openAngle = 90f;

    public AudioClip openSound;
    public AudioClip closeSound;
    public AudioSource audioSource;

    private bool isRotating = false;
    private bool isOpen = false;
    private Quaternion initialRotation;
    private Quaternion targetRotation;

    void Start()
    {
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Stocker la rotation de base
        initialRotation = transform.rotation;

        // Définir l'angle d'ouverture en fonction du tag
        if (gameObject.CompareTag("ExitLeftDoor"))
        {
            openAngle = -Mathf.Abs(openAngle);
        }
        else if (gameObject.CompareTag("ExitRightDoor"))
        {
            openAngle = Mathf.Abs(openAngle);
        }
    }

    public void ToggleDoor()
    {
        if (isRotating) return;

        Debug.Log("ToggleDoor appelé sur " + gameObject.name);

        isOpen = !isOpen;
        targetRotation = isOpen
            ? Quaternion.Euler(0, openAngle, 0) * initialRotation
            : initialRotation;

        PlaySound(isOpen ? openSound : closeSound);
        StartCoroutine(RotateDoor());
    }

    private IEnumerator RotateDoor()
    {
        isRotating = true;
        float elapsedTime = 0f;

        while (elapsedTime < rotationDuration)
        {
            float t = elapsedTime / rotationDuration;
            t = Mathf.SmoothStep(0, 1, t); // Ajoute une interpolation douce

            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.rotation = targetRotation; // Fixer la rotation finale
        isRotating = false;
        Debug.Log($"Rotation terminée sur {gameObject.name}");
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}