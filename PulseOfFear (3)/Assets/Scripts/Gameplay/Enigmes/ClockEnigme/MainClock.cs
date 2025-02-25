using System.Collections;
using UnityEngine;

using System.Collections;
using UnityEngine;

using System.Collections;
using UnityEngine;

namespace ClockSample
{
    public class MainClock : MonoBehaviour
    {
        // Utilisation de tableaux pour permettre plusieurs aiguilles
        public Transform[] handHours;
        public Transform[] handMinutes;

        private float targetHour;   // L'heure cible de l'horloge principale
        private float targetMinute;

        public static MainClock Instance; // Singleton pour accéder facilement à l'instance

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            // Génération d'une heure cible aléatoire
            targetHour = UnityEngine.Random.Range(0, 12);
            targetMinute = UnityEngine.Random.Range(0, 60);

            // Placement initial des aiguilles avec la rotation modifiée
            SetClockHands();

            // Debug de l'horaire défini
            Debug.Log($"Horloge principale définie à : {Mathf.Floor(targetHour)}:{Mathf.Floor(targetMinute)}");
        }

        private void SetClockHands()
        {
            // Calcul des rotations relatives pour les heures et les minutes
            float handRotationHours = targetHour * 30f + (targetMinute / 60f) * 30f;
            float handRotationMinutes = targetMinute * 6f;

            // Rotation de base forcée : X = 0, Y = -90, Z = -90 (pour respecter l'orientation du modèle)
            Quaternion defaultRotation = Quaternion.Euler(0f, -90f, -90f);

            // Appliquer la rotation aux aiguilles des heures
            foreach (Transform handHour in handHours)
            {
                if (handHour != null)
                {
                    handHour.localRotation = defaultRotation * Quaternion.Euler(0f, handRotationHours, 0f);
                }
            }

            // Appliquer la rotation aux aiguilles des minutes
            foreach (Transform handMinute in handMinutes)
            {
                if (handMinute != null)
                {
                    handMinute.localRotation = defaultRotation * Quaternion.Euler(0f, handRotationMinutes, 0f);
                }
            }
        }

        // Méthodes pour récupérer les valeurs de l'heure cible
        public float GetTargetHour() => targetHour;
        public float GetTargetMinute() => targetMinute;
    }
}
