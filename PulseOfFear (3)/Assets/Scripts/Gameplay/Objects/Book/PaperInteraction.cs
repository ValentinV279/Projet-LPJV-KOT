using UnityEngine;
using UnityEngine.UI;

public class PaperInteraction : MonoBehaviour
{
    public Camera playerCamera; // La caméra du joueur
    public GameObject paperHUD; // Le HUD qui affiche l'image et le texte
    public Text paperText;      // Le texte affiché sur le papier
    public string paperContent = "C'est une note effrayante..."; // Contenu du papier
    public float interactDistance = 5f; // Distance maximale pour interagir
    public MonoBehaviour playerMovementScript; // Référence au script de mouvement du joueur (ex: CharacterController)
    public GameObject player; // Le GameObject du joueur
    public GameObject playerCameraObject; // Le GameObject de la caméra
    public Rigidbody rb; // Référence au Rigidbody pour contrôler la physique
    public Animator animator; // Référence à l'Animator pour jouer les animations

    private GameObject currentPaper = null; // L'objet papier actuellement visé par le raycast
    private bool isReadingPaper = false;    // Indique si le joueur est en train de lire une note
    private bool isWalking = false; // Indicateur pour savoir si le joueur marche

    // Ajout d'un booléen pour savoir si le mouvement est autorisé
    private bool canMove = true;

    private bool cameraFrozen = false; // Indique si la caméra est gelée

    void Start()
    {
        if (paperHUD != null)
        {
            paperHUD.SetActive(false); // Désactive le HUD au début
        }

        // Vérifier si le script de mouvement est bien assigné
        if (playerMovementScript == null)
        {
            Debug.LogError("Erreur: Le script de mouvement du joueur n'a pas été assigné dans l'inspecteur !");
        }

        if (rb == null)
        {
            rb = player.GetComponent<Rigidbody>(); // Assure que le Rigidbody est bien assigné
        }

        if (animator == null)
        {
            animator = player.GetComponent<Animator>(); // Assure que l'Animator est bien assigné
        }
    }

    void Update()
    {
        // Vérifiez si le joueur est en train de marcher
        isWalking = Input.GetButton("Vertical") || Input.GetButton("Horizontal");

        // Si le joueur est en train de lire une note, il peut la fermer en appuyant sur "E"
        if (isReadingPaper && Input.GetKeyDown(KeyCode.E))
        {
            ClosePaper();
            return; // On retourne ici pour ne pas relancer de Raycast pendant la lecture
        }

        // Empêcher les mouvements et la rotation de la caméra si le joueur lit un papier
        if (!cameraFrozen)
        {
            RaycastHit hit;

            // Lancer un Raycast depuis la caméra vers l'avant
            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, interactDistance))
            {
                // Si le rayon touche un objet avec le tag "Paper" et que le joueur n'est pas déjà en train de lire un papier
                if (hit.collider.CompareTag("Paper") && !isReadingPaper)
                {
                    currentPaper = hit.collider.gameObject;

                    // Afficher le texte si le joueur appuie sur "E"
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        OpenPaper();
                    }
                }
            }
            else
            {
                // Si le rayon ne touche plus rien, réinitialiser le papier détecté
                if (currentPaper != null && !isReadingPaper)
                {
                    currentPaper = null;
                }
            }
        }

        // Gérer l'animation de marche
        HandleWalkingAnimation();
    }

    void OpenPaper()
    {
        isReadingPaper = true; // Le joueur commence à lire la note

        // Désactiver les entrées de mouvement et de caméra
        canMove = false;
        cameraFrozen = true; // Geler la rotation de la caméra

        // Vérifier si le HUD est assigné avant de l'afficher
        if (paperHUD != null)
        {
            paperHUD.SetActive(true); // Affiche le HUD
        }

        // Vérifier si le texte est assigné avant de l'afficher
        if (paperText != null)
        {
            paperText.text = paperContent; // Affiche le contenu de la note
        }

        // Désactiver le mouvement du joueur
        if (playerMovementScript != null)
        {
            playerMovementScript.enabled = false; // Désactive le mouvement du joueur
        }

        // Désactiver la rotation de la caméra sans désactiver la caméra elle-même
        if (playerCamera != null)
        {
            playerCamera.transform.rotation = playerCamera.transform.rotation; // Geler la rotation de la caméra
        }

        // Empêcher la rotation du personnage en réinitialisant la rotation du Rigidbody
        if (rb != null)
        {
            rb.freezeRotation = true; // Empêche la rotation physique du personnage
        }

        Debug.Log("Papier affiché : " + paperContent);
    }

    void ClosePaper()
    {
        isReadingPaper = false; // Le joueur arrête de lire la note

        // Réactiver les entrées de mouvement et de caméra
        canMove = true;
        cameraFrozen = false; // Réactiver la rotation de la caméra

        // Vérifier si le HUD est assigné avant de le cacher
        if (paperHUD != null)
        {
            paperHUD.SetActive(false); // Cache le HUD
        }

        // Réactiver le mouvement du joueur
        if (playerMovementScript != null)
        {
            playerMovementScript.enabled = true; // Réactive le mouvement du joueur
        }

        // Réactiver la rotation du Rigidbody pour permettre la rotation normale du personnage
        if (rb != null)
        {
            rb.freezeRotation = false; // Permet de nouveau la rotation physique du personnage
        }

        Debug.Log("Papier fermé.");
    }

    void HandleWalkingAnimation()
    {
        if (animator != null)
        {
            // Ne jouer l'animation de marche que si le joueur est en train de marcher **et** n'est pas en train de lire la note
            animator.SetBool("isWalking", isWalking && !isReadingPaper);
        }
    }
}
