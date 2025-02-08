using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private Vector3 originalPosition; // Position initiale de la caméra
    private float shakeDuration = 0f; // Durée restante du tremblement
    private float shakeMagnitude = 0.05f; // Intensité du tremblement (doux)
    private float dampingSpeed = 1f; // Vitesse d'atténuation (plus lente)

    void Start()
    {
        // Sauvegarde de la position initiale de la caméra
        originalPosition = transform.localPosition;
    }

    void Update()
    {
        if (shakeDuration > 0)
        {
            // Appliquer un tremblement aléatoire doux autour de la position initiale
            transform.localPosition = originalPosition + Random.insideUnitSphere * shakeMagnitude;

            // Réduire la durée restante du tremblement
            shakeDuration -= Time.deltaTime * dampingSpeed;
        }
        else
        {
            shakeDuration = 0f;
            transform.localPosition = originalPosition;
        }
    }

    /// <summary>
    /// Démarre le tremblement de la caméra.
    /// </summary>
    /// <param name="duration">Durée du tremblement</param>
    /// <param name="magnitude">Intensité du tremblement</param>
    public void StartShake(float duration, float magnitude)
    {
        shakeDuration = duration;
        shakeMagnitude = magnitude;
    }
}
