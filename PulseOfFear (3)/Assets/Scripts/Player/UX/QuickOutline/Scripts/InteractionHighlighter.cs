using UnityEngine;

public class InteractionHighlighter : MonoBehaviour
{
    public float interactionDistance = 3f;
    public LayerMask interactableLayer;

    private Transform highlightedObject;
    private Outline outline;

    void Start()
    {
        // Désactive l'outline sur tous les objets interactifs au démarrage
        foreach (Outline obj in FindObjectsOfType<Outline>())
        {
            obj.enabled = false;
        }
    }

    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionDistance, interactableLayer))
        {
            Transform hitTransform = hit.transform;

            if (highlightedObject != hitTransform)
            {
                RemoveHighlight();
                highlightedObject = hitTransform;
                ApplyHighlight();
            }
        }
        else
        {
            RemoveHighlight();
        }
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

            outline.enabled = true;  // Active uniquement lorsque le joueur regarde l'objet
            outline.OutlineMode = Outline.Mode.OutlineAll;
            outline.OutlineColor = Color.white;
            outline.OutlineWidth = 5f;
        }
    }

    void RemoveHighlight()
    {
        if (highlightedObject != null && outline != null)
        {
            outline.enabled = false;  // Désactive l'outline proprement
        }
        highlightedObject = null;
    }
}
