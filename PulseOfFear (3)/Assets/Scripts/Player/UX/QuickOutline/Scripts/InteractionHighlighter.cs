using UnityEngine;

public class InteractionHighlighter : MonoBehaviour
{
    public float interactionDistance = 3f;
    public LayerMask interactableLayer;
    public LayerMask obstacleLayer; // Ajout du layer pour les obstacles

    private Transform highlightedObject;
    private Outline outline;

    void Start()
    {
        // Désactive l'outline sur tous les objets interactables au démarrage
        foreach (Outline obj in FindObjectsOfType<Outline>())
        {
            obj.enabled = false;
        }
    }

    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        // Vérifie si un objet est détecté sur la trajectoire
        if (Physics.Raycast(ray, out hit, interactionDistance))
        {
            Transform hitTransform = hit.transform;

            // Vérifie si le premier objet touché est un obstacle
            if (((1 << hit.collider.gameObject.layer) & obstacleLayer.value) != 0)
            {
                RemoveHighlight(); // Un obstacle bloque la vue → Pas d'outline
                return;
            }

            // Vérifie si c'est un objet interactable
            if (((1 << hit.collider.gameObject.layer) & interactableLayer.value) != 0)
            {
                if (highlightedObject != hitTransform)
                {
                    RemoveHighlight();
                    highlightedObject = hitTransform;
                    ApplyHighlight();
                }
                return;
            }
        }

        RemoveHighlight(); // Si aucun objet interactable détecté ou si un obstacle bloque
    }

    void ApplyHighlight()
    {
        if (highlightedObject != null)
        {
            outline = highlightedObject.GetComponent<Outline>();
            if (outline == null)
            {
                outline = highlightedObject.gameObject.AddComponent<Outline>();
            }

            outline.enabled = true;
            outline.OutlineMode = Outline.Mode.OutlineAll;
            outline.OutlineColor = Color.white;
            outline.OutlineWidth = 5f;
        }
    }

    void RemoveHighlight()
    {
        if (highlightedObject != null && outline != null)
        {
            outline.enabled = false;
        }
        highlightedObject = null;
    }
}
