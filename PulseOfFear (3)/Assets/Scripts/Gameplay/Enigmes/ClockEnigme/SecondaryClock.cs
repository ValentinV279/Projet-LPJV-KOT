using System.Collections;
using UnityEngine;
using TMPro;

namespace ClockSample
{
    public class SecondaryClock : MonoBehaviour
    {
        public Transform handHours1, handMinutes1;
        public Transform handHours2, handMinutes2;
        public Transform handHours3, handMinutes3;

        public static float targetHour; // Défini par MainClock
        public static float targetMinute; // Défini par MainClock

        private float currentHour1, currentMinute1;
        private float currentHour2, currentMinute2;
        private float currentHour3, currentMinute3;

        private float speed1 = 0.15f, speed2 = 0.1f, speed3 = 0.05f;

        private Coroutine clockCoroutine1, clockCoroutine2, clockCoroutine3;

        private bool isRunning1 = true, isRunning2 = true, isRunning3 = true;
        private bool isClock1Complete = false, isClock2Complete = false, isClock3Complete = false; // Nouveaux états
        private int completedClocks = 0; // Compteur des horloges correctement définies

        public AudioSource successSound; // Son de succès
        public AudioSource failureSound; // Son d'échec
        public Light spotlight; // Spotlight à changer de couleur

        public SubtitleManager subtitleManager; // Référence au SubtitleManager
        public TextMeshProUGUI puzzleText;

        private const float marginMinutes = 10f; // Marge d'erreur de 10 minutes

        private void Start()
        {
            currentHour1 = 0;
            currentMinute1 = 0;
            currentHour2 = 0;
            currentMinute2 = 0;
            currentHour3 = 0;
            currentMinute3 = 0;

            clockCoroutine1 = StartCoroutine(UpdateClock1());
            clockCoroutine2 = StartCoroutine(UpdateClock2());
            clockCoroutine3 = StartCoroutine(UpdateClock3());
        }

        private IEnumerator UpdateClock1()
        {
            while (true)
            {
                UpdateTime(ref currentHour1, ref currentMinute1, speed1);
                UpdateHands(handHours1, handMinutes1, currentHour1, currentMinute1);
                yield return null;
            }
        }

        private IEnumerator UpdateClock2()
        {
            while (true)
            {
                UpdateTime(ref currentHour2, ref currentMinute2, speed2);
                UpdateHands(handHours2, handMinutes2, currentHour2, currentMinute2);
                yield return null;
            }
        }

        private IEnumerator UpdateClock3()
        {
            while (true)
            {
                UpdateTime(ref currentHour3, ref currentMinute3, speed3);
                UpdateHands(handHours3, handMinutes3, currentHour3, currentMinute3);
                yield return null;
            }
        }

        private void UpdateTime(ref float hour, ref float minute, float speed)
        {
            float minutesPerSecond = 60f / (speed * 12f); // Minutes ajoutées par frame
            minute += minutesPerSecond * Time.deltaTime;

            if (minute >= 60f)
            {
                minute -= 60f;
                hour += 1f;
                if (hour >= 12f) hour -= 12f;
            }
        }

        private void UpdateHands(Transform handHours, Transform handMinutes, float hour, float minute)
        {
            if (handHours) handHours.localRotation = Quaternion.Euler(0f, 0f, hour * 30f + (minute / 60f) * 30f);
            if (handMinutes) handMinutes.localRotation = Quaternion.Euler(0f, 0f, minute * 6f);
        }

        public bool ToggleClockState(int clockIndex)
        {
            switch (clockIndex)
            {
                case 0:
                    if (isClock1Complete) return false; // Empêche d'interagir avec une horloge complétée
                    if (isRunning1)
                    {
                        StopCoroutine(clockCoroutine1);
                        isRunning1 = false;
                        CheckTime(currentHour1, currentMinute1, 0);
                    }
                    else
                    {
                        clockCoroutine1 = StartCoroutine(UpdateClock1());
                        isRunning1 = true;
                    }
                    return isRunning1;

                case 1:
                    if (isClock2Complete) return false; // Empêche d'interagir avec une horloge complétée
                    if (isRunning2)
                    {
                        StopCoroutine(clockCoroutine2);
                        isRunning2 = false;
                        CheckTime(currentHour2, currentMinute2, 1);
                    }
                    else
                    {
                        clockCoroutine2 = StartCoroutine(UpdateClock2());
                        isRunning2 = true;
                    }
                    return isRunning2;

                case 2:
                    if (isClock3Complete) return false; // Empêche d'interagir avec une horloge complétée
                    if (isRunning3)
                    {
                        StopCoroutine(clockCoroutine3);
                        isRunning3 = false;
                        CheckTime(currentHour3, currentMinute3, 2);
                    }
                    else
                    {
                        clockCoroutine3 = StartCoroutine(UpdateClock3());
                        isRunning3 = true;
                    }
                    return isRunning3;

                default:
                    return false;
            }
        }

        private void CheckTime(float hour, float minute, int clockIndex)
        {
            float targetHour = MainClock.Instance.GetTargetHour();
            float targetMinute = MainClock.Instance.GetTargetMinute();

            float targetTotalMinutes = targetHour * 60 + targetMinute;
            float currentTotalMinutes = hour * 60 + minute;

            float difference = Mathf.Min(
                Mathf.Abs(targetTotalMinutes - currentTotalMinutes),
                720 - Mathf.Abs(targetTotalMinutes - currentTotalMinutes)
            );

            bool isSuccess = difference <= marginMinutes;

            if (isSuccess)
            {
                successSound?.Play();
                CompleteClock(clockIndex); // Marquer l'horloge comme complétée
            }
            else
            {
                failureSound?.Play();
                StartCoroutine(HighlightSpotlight(Color.red));
            }

            Debug.Log($"Heure cible définie : {Mathf.Floor(targetHour)}:{Mathf.Floor(targetMinute)} | Heure arrêtée : {Mathf.Floor(hour)}:{Mathf.Floor(minute)} | Différence : {difference} minutes");
        }

        private void CompleteClock(int clockIndex)
        {
            switch (clockIndex)
            {
                case 0:
                    isClock1Complete = true;
                    break;
                case 1:
                    isClock2Complete = true;
                    break;
                case 2:
                    isClock3Complete = true;
                    break;
            }

            completedClocks++;

            if (completedClocks == 3)
            {
                SetSpotlightBlue();
                subtitleManager?.ShowSubtitle("ca y est ! les horloges sont synchronisees");
                TaskManager.Instance.CompleteTask();
                if (puzzleText != null)
                {
                    puzzleText.fontStyle = FontStyles.Strikethrough;
                }
            }
            else
            {
                StartCoroutine(HighlightSpotlight(Color.green));
            }
        }

        private void SetSpotlightBlue()
        {
            if (spotlight != null)
            {
                spotlight.color = Color.blue;
            }
        }

        private IEnumerator HighlightSpotlight(Color color)
        {
            if (spotlight != null && completedClocks < 3) // Ne change pas si toutes les horloges sont complétées
            {
                spotlight.color = color;
                yield return new WaitForSeconds(1f);
                spotlight.color = Color.red; // Retour à la couleur par défaut
            }
        }

        public string GetClockTime(int clockIndex)
        {
            switch (clockIndex)
            {
                case 0: return $"{Mathf.Floor(currentHour1)}:{Mathf.Floor(currentMinute1)}";
                case 1: return $"{Mathf.Floor(currentHour2)}:{Mathf.Floor(currentMinute2)}";
                case 2: return $"{Mathf.Floor(currentHour3)}:{Mathf.Floor(currentMinute3)}";
                default: return "Invalid Clock";
            }
        }
    }
}
