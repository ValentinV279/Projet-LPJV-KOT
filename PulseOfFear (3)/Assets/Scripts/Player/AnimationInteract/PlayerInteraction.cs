using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float interactionDistance = 3f; // Distance d'interaction avec l'objet
    public LayerMask interactableLayer; // Layer des objets interactifs (tiroirs, coffres, etc.)

    private Camera playerCamera;

    void Start()
    {
        playerCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) // Interagir lorsqu'on appuie sur "E"
        {
            TryInteractWithObject();
        }
    }

    private void TryInteractWithObject()
    {
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionDistance, interactableLayer))
        {
            Debug.Log("Objet interactif détecté : " + hit.collider.name);

            // Vérifier s'il s'agit d'un tiroir
            DrawerController drawer = hit.collider.GetComponent<DrawerController>();
            if (drawer != null)
            {
                Debug.Log("Tiroir détecté !");
                drawer.ToggleDrawer(); // Ouvre/Ferme le tiroir
                return; // Si c'est un tiroir, on arrête ici
            }

            // Vérifier s'il s'agit d'un coffre
            ChestController chest = hit.collider.GetComponent<ChestController>();
            if (chest != null)
            {
                Debug.Log("Coffre détecté !");
                chest.ToggleChest(); // Ouvre/Ferme le coffre
            }
            else
            {
                Debug.Log("Aucun tiroir ou coffre détecté.");
            }
        }
        else
        {
            Debug.Log("Aucun objet interactif à portée.");
        }
    }

}
