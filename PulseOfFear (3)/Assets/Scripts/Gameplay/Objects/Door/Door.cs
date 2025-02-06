using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DoorScript
{
    // Ce composant assure qu'un AudioSource est attaché à l'objet
    [RequireComponent(typeof(AudioSource))]

    public class Door : MonoBehaviour {
        // Booléen indiquant si la porte est ouverte ou fermée
        public bool open;

        // Facteur de lissage pour ajuster la vitesse d'animation de la porte
        public float smooth = 1.0f;

        // Angle d'ouverture de la porte
        float DoorOpenAngle = -90.0f;

        // Angle de fermeture de la porte
        float DoorCloseAngle = 0.0f;

        // Référence à l'AudioSource pour jouer des sons
        public AudioSource asource;

        // Clips audio pour les sons d'ouverture et de fermeture de la porte
        public AudioClip openDoor, closeDoor;

        // Méthode appelée une seule fois lors de l'initialisation
        void Start () {
            // Récupère automatiquement le composant AudioSource attaché à l'objet
            asource = GetComponent<AudioSource>();
        }

        // Méthode appelée à chaque frame
        void Update () {
            if (open) {
                // Si la porte doit être ouverte, on définit une rotation cible avec DoorOpenAngle
                var target = Quaternion.Euler(0, DoorOpenAngle, 0);
                // Interpolation (transition douce) vers l'angle d'ouverture
                transform.localRotation = Quaternion.Slerp(
                    transform.localRotation,
                    target,
                    Time.deltaTime * 5 * smooth
                );
            } else {
                // Sinon, la rotation cible est DoorCloseAngle (porte fermée)
                var target1 = Quaternion.Euler(0, DoorCloseAngle, 0);
                // Interpolation vers l'angle de fermeture
                transform.localRotation = Quaternion.Slerp(
                    transform.localRotation,
                    target1,
                    Time.deltaTime * 5 * smooth
                );
            }
        }

        // Méthode appelée pour ouvrir ou fermer la porte
        public void OpenDoor() {
            // Inverse l'état de la porte (ouverte -> fermée ou fermée -> ouverte)
            open = !open;

            // Choisit le son à jouer en fonction de l'état actuel
            asource.clip = open ? openDoor : closeDoor;

            // Joue le clip audio sélectionné
            asource.Play();
        }
    }
}
