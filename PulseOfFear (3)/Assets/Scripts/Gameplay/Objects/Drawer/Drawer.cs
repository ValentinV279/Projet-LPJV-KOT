using UnityEngine;

public class Drawer : MonoBehaviour
{
    public float moveDistance = 0.15f;
    public float moveSpeed = 2f; // Vitesse de déplacement
    private bool isOpen = false; // État actuel du tiroir
    private Vector3 closedPosition; // Position fermée initiale
    private Vector3 openPosition; // Position ouverte cible
    private Coroutine moveCoroutine; // Coroutine de mouvement

    public AudioClip openSound; // Son d'ouverture
    public AudioClip closeSound; // Son de fermeture
    public AudioSource audioSource; // Source audio pour jouer les sons

    void Start()
    {
        // Initialise les positions ouverte et fermée
        closedPosition = transform.localPosition;
        openPosition = closedPosition + new Vector3(0, 0, moveDistance);

        // Configure la source audio
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public void ToggleDrawer()
    {
        // Stoppe la coroutine en cours pour éviter des conflits
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }

        // Joue le son approprié
        PlaySound(isOpen ? closeSound : openSound);

        // Lance la coroutine pour ouvrir ou fermer le tiroir
        moveCoroutine = StartCoroutine(MoveDrawer(isOpen ? closedPosition : openPosition));
        isOpen = !isOpen; // Bascule l'état
    }

    private System.Collections.IEnumerator MoveDrawer(Vector3 targetPosition)
    {
        while (Vector3.Distance(transform.localPosition, targetPosition) > 0.01f)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, Time.deltaTime * moveSpeed);
            yield return null; // Attend la prochaine frame
        }

        transform.localPosition = targetPosition; // Assure une position finale précise
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
