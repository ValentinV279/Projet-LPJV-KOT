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
            Debug.Log("Objet interactif d�tect� : " + hit.collider.name);

            // V�rifie si l'objet a le Tag "Chest" (Coffre)
            if (hit.collider.CompareTag("Chest"))
            {
                ChestController chest = hit.collider.GetComponent<ChestController>();
                if (chest != null)
                {
                    chest.ToggleChest(); // Ouvre/Ferme le coffre
                }
            }

            // V�rifie si l'objet a le Tag "Drawer" (Tiroir)
            if (hit.collider.CompareTag("Drawer"))
            {
                DrawerController drawer = hit.collider.GetComponent<DrawerController>();
                if (drawer != null)
                {
                    drawer.ToggleDrawer(); // Ouvre/Ferme le tiroir
                }
            }

            // V�rifie si l'objet a le Tag "Wardrobe" (Armoire)
            if (hit.collider.CompareTag("Wardrobe"))
            {
                WardrobeController wardrobe = hit.collider.GetComponent<WardrobeController>();
                if (wardrobe != null)
                {
                    wardrobe.ToggleWardrobe(); // Ouvre/Ferme l'armoire
                }
            }

            // V�rifie si l'objet a le Tag "DoorLeft" (Porte gauche)
            if (hit.collider.CompareTag("DoorLeft"))
            {
                DoorLeftController doorLeft = hit.collider.GetComponent<DoorLeftController>();
                if (doorLeft != null)
                {
                    doorLeft.ToggleDoor(); // Ouvre/Ferme la porte gauche
                }
            }

            // V�rifie si l'objet a le Tag "Atelier" (Porte gauche)
            if (hit.collider.CompareTag("TiroirAtelier3"))
            {
                DrawerAtelier3 TiroirAtelier3 = hit.collider.GetComponent<DrawerAtelier3>();
                if (TiroirAtelier3 != null)
                {
                    TiroirAtelier3.ToggleAtelier3(); // Ouvre/Ferme la porte gauche
                }
            }

            // V�rifie si l'objet a le Tag "Atelier" (Porte gauche)
            if (hit.collider.CompareTag("TiroirAtelier2"))
            {
                DrawerAtelier2 TiroirAtelier2 = hit.collider.GetComponent<DrawerAtelier2>();
                if (TiroirAtelier2 != null)
                {
                    TiroirAtelier2.ToggleAtelier2(); // Ouvre/Ferme la porte gauche
                }
            }

            // V�rifie si l'objet a le Tag "Atelier" (Porte gauche)
            if (hit.collider.CompareTag("TiroirAtelier1"))
            {
                DrawerAtelier1 TiroirAtelier1 = hit.collider.GetComponent<DrawerAtelier1>();
                if (TiroirAtelier1 != null)
                {
                    TiroirAtelier1.ToggleAtelier1(); // Ouvre/Ferme la porte gauche
                }
            }



            // V�rifie si l'objet a le Tag "Atelier" (Porte gauche)
            if (hit.collider.CompareTag("TiroirAtelier1"))
            {
                AtelierBureau1 TiroirAtelier1 = hit.collider.GetComponent<AtelierBureau1>();
                if (TiroirAtelier1 != null)
                {
                    TiroirAtelier1.ToggleBureau1(); // Ouvre/Ferme la porte gauche
                }
            }

            // V�rifie si l'objet a le Tag "Atelier" (Porte gauche)
            if (hit.collider.CompareTag("TiroirAtelier2"))
            {
                AtelierBureau2 TiroirAtelier2 = hit.collider.GetComponent<AtelierBureau2>();
                if (TiroirAtelier2 != null)
                {
                    TiroirAtelier2.ToggleBureau2(); // Ouvre/Ferme la porte gauche
                }
            }

            // V�rifie si l'objet a le Tag "Atelier" (Porte gauche)
            if (hit.collider.CompareTag("TiroirAtelier3"))
            {
                AtelierBureau3 TiroirAtelier3 = hit.collider.GetComponent<AtelierBureau3>();
                if (TiroirAtelier3 != null)
                {
                    TiroirAtelier3.ToggleBureau3(); // Ouvre/Ferme la porte gauche
                }
            }

            // V�rifie si l'objet a le Tag "Atelier" (Porte gauche)
            if (hit.collider.CompareTag("TiroirAtelier3"))
            {
                DoorHall TiroirAtelier3 = hit.collider.GetComponent<DoorHall>();
                if (TiroirAtelier3 != null)
                {
                    TiroirAtelier3.ToggleArmoirhall(); // Ouvre/Ferme la porte gauche
                }
            }

            // V�rifie si l'objet a le Tag "Atelier" (Porte gauche)
            if (hit.collider.CompareTag("TiroirAtelier3"))
            {
                Armoirehallportegauche TiroirAtelier3 = hit.collider.GetComponent<Armoirehallportegauche>();
                if (TiroirAtelier3 != null)
                {
                    TiroirAtelier3.ToggleArmoirhallLeft(); // Ouvre/Ferme la porte gauche
                }
            }

            // V�rifie si l'objet a le Tag "Atelier" (Porte gauche)
            if (hit.collider.CompareTag("TiroirAtelier3"))
            {
                Commodehall TiroirAtelier3 = hit.collider.GetComponent<Commodehall>();
                if (TiroirAtelier3 != null)
                {
                    TiroirAtelier3.TogglecommodeHall1(); // Ouvre/Ferme la porte gauche
                }
            }

            // V�rifie si l'objet a le Tag "Atelier" (Porte gauche)
            if (hit.collider.CompareTag("TiroirAtelier3"))
            {
                Commodehall2 TiroirAtelier3 = hit.collider.GetComponent<Commodehall2>();
                if (TiroirAtelier3 != null)
                {
                    TiroirAtelier3.ToggleCommodeHall2(); // Ouvre/Ferme la porte gauche
                }
            }

            // V�rifie si l'objet a le Tag "Atelier" (Porte gauche)
            if (hit.collider.CompareTag("TiroirAtelier3"))
            {
                Commodehall3 TiroirAtelier3 = hit.collider.GetComponent<Commodehall3>();
                if (TiroirAtelier3 != null)
                {
                    TiroirAtelier3.ToggleCommodeHall3(); // Ouvre/Ferme la porte gauche
                }
            }

            
            // V�rifie si l'objet a le Tag "Atelier" (Porte gauche)
            if (hit.collider.CompareTag("TiroirAtelier3"))
            {
                DoorHall1 TiroirAtelier3 = hit.collider.GetComponent<DoorHall1>();
                if (TiroirAtelier3 != null)
                {
                    TiroirAtelier3.ToggleDoorHall(); // Ouvre/Ferme la porte gauche
                }
            }
            

            // V�rifie si l'objet a le Tag "Atelier" (Porte gauche)
            if (hit.collider.CompareTag("TiroirAtelier3"))
            {
                DoorHall2 TiroirAtelier3 = hit.collider.GetComponent<DoorHall2>();
                if (TiroirAtelier3 != null)
                {
                    TiroirAtelier3.ToggleDoorHall2(); // Ouvre/Ferme la porte gauche
                }
            }

            // V�rifie si l'objet a le Tag "Atelier" (Porte gauche)
            if (hit.collider.CompareTag("TiroirAtelier3"))
            {
                DoorHall3 TiroirAtelier3 = hit.collider.GetComponent<DoorHall3>();
                if (TiroirAtelier3 != null)
                {
                    TiroirAtelier3.ToggleDoorHall3(); // Ouvre/Ferme la porte gauche
                }
            }
            // V�rifie si l'objet a le Tag "Atelier" (Porte gauche)
            if (hit.collider.CompareTag("TiroirAtelier3"))
            {
                coiffeuse TiroirAtelier3 = hit.collider.GetComponent<coiffeuse>();
                if (TiroirAtelier3 != null)
                {
                    TiroirAtelier3.Togglecoiffeuse(); // Ouvre/Ferme la porte gauche
                }
            }
        }
        else
        {
            Debug.Log("Aucun objet interactif d�tect�.");
        }
    }

}
