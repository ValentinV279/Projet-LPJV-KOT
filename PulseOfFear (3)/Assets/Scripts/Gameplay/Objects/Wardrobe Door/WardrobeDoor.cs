using UnityEngine;

public class WardrobeDoor : MonoBehaviour
{
    public Transform pivotPoint; // Point de pivot (Empty)
    public float rotationSpeed = 35f; // Vitesse de rotation
    public float finalDecelerationFactor = 0.1f; // Facteur de décélération progressive
    public float rotationDuration = 3f; // Temps maximum pour ouvrir/fermer la porte
    public float openAngle = 90f; // Angle d'ouverture initial (positif pour la droite, négatif pour la gauche)

    public AudioClip openSound; // Son d'ouverture
    public AudioClip closeSound; // Son de fermeture

    private bool isRotating = false; // Indique si une rotation est en cours
    public AudioSource audioSource; // Composant AudioSource
    private float currentAngle = 0f; // Suivi de l'angle actuel de la porte

    void Start()
    {
        // Configure la source audio
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public void ToggleDoor()
    {
        if (isRotating) return; // Empêche une nouvelle rotation si la porte est déjà en mouvement

        // Détermine l'angle cible en fonction de l'état actuel de la porte
        float targetAngle = Mathf.Approximately(currentAngle, 0f) ? openAngle : 0f;

        // Inverse l'angle d'ouverture pour la prochaine interaction
        openAngle *= -1;

        // Joue le son approprié
        PlaySound(Mathf.Approximately(currentAngle, 0f) ? openSound : closeSound);

        // Lancer la rotation
        StartCoroutine(RotateDoor(targetAngle));
    }

    private System.Collections.IEnumerator RotateDoor(float targetAngle)
    {
        isRotating = true; // Marque la porte comme en rotation

        float elapsedTime = 0f;
        float initialSpeed = rotationSpeed;

        while (elapsedTime < rotationDuration)
        {
            float step = initialSpeed * Time.deltaTime;
            float remainingAngle = Mathf.Abs(targetAngle - currentAngle);

            // Applique une décélération progressive lorsque l'angle restant est faible
            if (remainingAngle < 20f) // Décélération dans les 20 derniers degrés
            {
                step *= Mathf.Lerp(1f, finalDecelerationFactor, 1f - remainingAngle / 20f);
            }

            // Met à jour l'angle et applique la rotation
            float deltaAngle = step * Mathf.Sign(targetAngle - currentAngle);
            currentAngle += deltaAngle;
            transform.RotateAround(pivotPoint.position, Vector3.up, deltaAngle);

            // Vérifie si l'angle cible est atteint
            if (Mathf.Abs(currentAngle - targetAngle) < 0.1f)
            {
                break;
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Fixe la rotation pour s'assurer qu'elle s'arrête exactement à l'angle cible
        transform.RotateAround(pivotPoint.position, Vector3.up, targetAngle - currentAngle);
        currentAngle = targetAngle; // Met à jour l'angle actuel

        isRotating = false; // Marque la rotation comme terminée
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
