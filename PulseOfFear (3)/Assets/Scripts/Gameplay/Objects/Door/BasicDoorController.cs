using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class BasicDoorController : MonoBehaviour
{
    public Transform pivotPoint; // Point de pivot de la porte
    public float rotationSpeed = 35f; // Vitesse de rotation
    public float finalDecelerationFactor = 0.1f; // Facteur de décélération progressive
    public float rotationDuration = 3f; // Durée de la rotation complète
    public float openAngle = 90f; // Angle d'ouverture (positif pour la droite, négatif pour la gauche)
    public float initialAutoOpenDelay = 5f; // Délai initial avant ouverture (désactivé maintenant)
    public float subsequentAutoOpenDelay = 10f; // Délai avant réouverture automatique après la première ouverture

    public AudioClip openSound; // Son d'ouverture
    public AudioClip closeSound; // Son de fermeture

    private NavMeshObstacle navMeshObstacle; // Référence au NavMeshObstacle
    private bool isRotating = false; // Indique si une rotation est en cours
    private bool isClosed = true; // Indique si la porte est fermée
    private bool hasBeenOpenedOnce = false; // Vérifie si la porte a été ouverte au moins une fois
    private float currentAngle = 0f; // Suivi de l'angle actuel de la porte
    private float timeSinceClose = 0f; // Temps écoulé depuis la fermeture

    private AudioSource doorAudioSource; // Composant AudioSource

    void Start()
    {
        // Configure la source audio
        doorAudioSource = GetComponent<AudioSource>();
        if (doorAudioSource == null)
        {
            doorAudioSource = gameObject.AddComponent<AudioSource>();
            doorAudioSource.spatialBlend = 1.0f; // Configure le son comme entièrement 3D
            doorAudioSource.playOnAwake = false; // Évite de jouer un son au démarrage
        }

        // Configure le NavMeshObstacle
        navMeshObstacle = GetComponent<NavMeshObstacle>();
        if (navMeshObstacle != null)
        {
            navMeshObstacle.carving = true; // Active le carving au démarrage
        }
    }

    void Update()
    {
        // Vérifie si la porte doit se réouvrir automatiquement
        if (isClosed && !isRotating && hasBeenOpenedOnce) // 🔹 Ne s'ouvre automatiquement que si elle a déjà été ouverte au moins une fois
        {
            timeSinceClose += Time.deltaTime;
            if (timeSinceClose >= subsequentAutoOpenDelay)
            {
                ToggleDoor(); // Rouvre la porte après 10 secondes
            }
        }
    }

    public void ToggleDoor()
    {
        if (isRotating) return; // Empêche une nouvelle rotation si la porte est déjà en mouvement

        // Si c'est la première ouverture, on marque la porte comme ayant été ouverte
        if (!hasBeenOpenedOnce)
        {
            hasBeenOpenedOnce = true;
        }

        // Détermine l'angle cible en fonction de l'état actuel de la porte
        float targetAngle = Mathf.Approximately(currentAngle, 0f) ? openAngle : 0f;

        // Inverse l'angle d'ouverture pour la prochaine interaction
        openAngle *= -1;

        // Joue le son approprié via l'AudioSource
        PlaySound(Mathf.Approximately(currentAngle, 0f) ? openSound : closeSound);

        // Garde le carving actif, quelle que soit la position de la porte
        if (navMeshObstacle != null)
        {
            navMeshObstacle.carving = true; // Toujours actif
        }

        // Met à jour l'état de la porte
        isClosed = Mathf.Approximately(targetAngle, 0f);
        if (isClosed)
        {
            timeSinceClose = 0f; // Réinitialise le compteur de temps de fermeture
        }

        // Lancer la rotation
        StartCoroutine(RotateDoor(targetAngle));
    }

    private IEnumerator RotateDoor(float targetAngle)
    {
        isRotating = true; // Marque la porte comme en rotation

        float elapsedTime = 0f;
        float initialSpeed = rotationSpeed;

        while (elapsedTime < rotationDuration)
        {
            float step = initialSpeed * Time.deltaTime;
            float remainingAngle = Mathf.Abs(targetAngle - currentAngle);

            // Applique une décélération progressive lorsque l'angle restant est faible
            if (remainingAngle < 20f)
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
        if (doorAudioSource != null && clip != null)
        {
            doorAudioSource.clip = clip;
            doorAudioSource.Play();
        }
    }
}
