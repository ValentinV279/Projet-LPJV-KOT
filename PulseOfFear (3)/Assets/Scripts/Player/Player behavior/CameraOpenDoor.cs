using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CameraDoorScript
{
    public class CameraOpenDoor : MonoBehaviour {
        // Distance maximale pour détecter une porte devant la caméra
        public float DistanceOpen = 3;
        
        // Référence à un objet texte (par exemple, un message "Appuyez sur E pour ouvrir")
        public GameObject text;

        // Méthode appelée au début (initialisation)
        void Start () {
            // Ici, il n'y a pas d'initialisation particulière
        }

        // Méthode appelée à chaque frame
        void Update () {
            RaycastHit hit; // Contiendra les informations sur l'objet touché par le raycast
            
            // Lancer un raycast à partir de la position de l'objet (souvent la caméra) dans la direction avant
            if (Physics.Raycast(transform.position, transform.forward, out hit, DistanceOpen)) {
                // Vérifie si l'objet touché par le raycast possède un composant DoorScript.Door
                if (hit.transform.GetComponent<DoorScript.Door>()) {
                    // Affiche l'objet texte (indiquant qu'on peut interagir avec la porte)
                    text.SetActive(true);

                    // Vérifie si la touche "E" est pressée
                    if (Input.GetKeyDown(KeyCode.E)) {
                        // Appelle la méthode OpenDoor() sur la porte détectée
                        hit.transform.GetComponent<DoorScript.Door>().OpenDoor();
                    }
                } else {
                    // Si l'objet touché n'est pas une porte, cacher le texte
                    text.SetActive(false);
                }
            } else {
                // Si aucun objet n'est touché par le raycast, cacher le texte
                text.SetActive(false);
            }
        }
    }
}
