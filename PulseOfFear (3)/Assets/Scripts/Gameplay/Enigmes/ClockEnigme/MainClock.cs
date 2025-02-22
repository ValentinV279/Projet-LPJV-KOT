using System.Collections;
using UnityEngine;

namespace ClockSample
{
    public class MainClock : MonoBehaviour
    {
        public Transform handHours;
        public Transform handMinutes;

        private float targetHour; // L'heure cible de l'horloge principale
        private float targetMinute;

        public static MainClock Instance; // Singleton pour accéder facilement à l'instance

        private void Awake()
        {
            Instance = this; // Définit le singleton
        }

        private void Start()
        {
            // Génération d'une heure cible aléatoire
            targetHour = UnityEngine.Random.Range(0, 12);
            targetMinute = UnityEngine.Random.Range(0, 60);

            // Placement initial des aiguilles
            SetClockHands();

            // Debug de l'horaire défini
            Debug.Log($"Horloge principale définie à : {Mathf.Floor(targetHour)}:{Mathf.Floor(targetMinute)}");
        }

        private void SetClockHands()
        {
            // Conversion de l'heure cible en rotation
            float handRotationHours = targetHour * 30 + (targetMinute / 60f) * 30; // 360/12
            float handRotationMinutes = targetMinute * 6; // 360/60

            // Application des rotations aux aiguilles
            if (handHours)
            {
                handHours.localEulerAngles = new Vector3(0, 0, handRotationHours);
            }

            if (handMinutes)
            {
                handMinutes.localEulerAngles = new Vector3(0, 0, handRotationMinutes);
            }
        }

        // Méthodes pour récupérer les heures
        public float GetTargetHour() => targetHour;
        public float GetTargetMinute() => targetMinute;
    }
}
