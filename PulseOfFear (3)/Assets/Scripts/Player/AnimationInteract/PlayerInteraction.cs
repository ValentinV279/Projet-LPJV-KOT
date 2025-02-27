using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float interactionDistance = 5f; // Distance d'interaction avec l'objet
    public LayerMask interactableLayer; // Layer des objets interactifs (tiroirs, coffres, armoires, etc.)

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

            // Vérifie si l'objet a le Tag "Chest" (Coffre)
            if (hit.collider.CompareTag("Chest"))
            {
                ChestController chest = hit.collider.GetComponent<ChestController>();
                if (chest != null)
                {
                    chest.ToggleChest(); // Ouvre/Ferme le coffre
                }
            }

            // Vérifie si l'objet a le Tag "Drawer" (Tiroir)
            if (hit.collider.CompareTag("Drawer"))
            {
                DrawerController drawer = hit.collider.GetComponent<DrawerController>();
                if (drawer != null)
                {
                    drawer.ToggleDrawer(); // Ouvre/Ferme le tiroir
                }
            }

            // Vérifie si l'objet a le Tag "Wardrobe" (Armoire)
            if (hit.collider.CompareTag("Wardrobe"))
            {
                WardrobeController wardrobe = hit.collider.GetComponent<WardrobeController>();
                if (wardrobe != null)
                {
                    wardrobe.ToggleWardrobe(); // Ouvre/Ferme l'armoire
                }
            }

            // Vérifie si l'objet a le Tag "DoorLeft" (Porte gauche)
            if (hit.collider.CompareTag("DoorLeft"))
            {
                DoorLeftController doorLeft = hit.collider.GetComponent<DoorLeftController>();
                if (doorLeft != null)
                {
                    doorLeft.ToggleDoor(); // Ouvre/Ferme la porte gauche
                }
            }
        }
        else
        {
            Debug.Log("Aucun objet interactif détecté.");
        }
    }

}
