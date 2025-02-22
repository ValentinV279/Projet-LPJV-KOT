using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SubtitleManager : MonoBehaviour
{
    [Header("Subtitle UI")]
    public TextMeshProUGUI subtitleText; // Référence au TextMeshPro UI pour les sous-titres

    [Header("Settings")]
    public float subtitleDuration = 3f; // Durée d'affichage des sous-titres
    public float fadeDuration = 0.5f; // Durée de l'apparition/disparition progressive

    [Header("Trigger Objects")]
    public List<GameObject> subtitleTriggers; // Liste des objets triggers

    [Header("Audio")]
    public AudioSource playerClapAudioSource; // Référence à l'Audio Source du clap

    private Coroutine subtitleCoroutine;

    void Start()
    {
        // Assure que tous les objets triggers ont un collider et sont marqués comme "isTrigger"
        foreach (GameObject trigger in subtitleTriggers)
        {
            if (trigger != null)
            {
                Collider collider = trigger.GetComponent<Collider>();
                if (collider != null)
                {
                    collider.isTrigger = true;
                }
                else
                {
                    Debug.LogWarning($"L'objet {trigger.name} n'a pas de Collider attaché.");
                }
            }
        }

        // Assure que le texte des sous-titres est désactivé au démarrage
        if (subtitleText != null)
        {
            subtitleText.gameObject.SetActive(false);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        // Vérifie si l'objet déclencheur fait partie de la liste des triggers
        if (subtitleTriggers.Contains(other.gameObject))
        {
            if (other.gameObject.name == "IntroSubtitle")
            {
                ShowSubtitle("Ooh, what a strange place...");
            }
            else if (other.gameObject.name == "ClockSubtitleTrigger")
            {
                ShowSubtitle("Wow! the clock room");
            }
            else if (other.gameObject.name == "PressurePlateSubtitleTrigger")
            {
                ShowSubtitle("I see 2 pressure plates, something might happen when both are actived...");
            }
            else
            {
                ShowSubtitle(other.gameObject.name); // Affiche le nom de l'objet comme sous-titre
            }

            // Détruit le trigger après déclenchement
            Destroy(other.gameObject);
        }
    }

    public void ShowSubtitle(string dialogue)
    {
        if (subtitleCoroutine != null)
        {
            StopCoroutine(subtitleCoroutine);
        }
        subtitleCoroutine = StartCoroutine(DisplaySubtitle(dialogue));
    }

    public IEnumerator DisplaySubtitle(string dialogue)
    {
        if (subtitleText != null)
        {
            subtitleText.text = dialogue;
            subtitleText.gameObject.SetActive(true);

            Color textColor = subtitleText.color;
            textColor.a = 0;
            subtitleText.color = textColor;

            float timer = 0f;
            while (timer < fadeDuration)
            {
                timer += Time.deltaTime;
                textColor.a = Mathf.Clamp01(timer / fadeDuration);
                subtitleText.color = textColor;
                yield return null;
            }

            textColor.a = 1;
            subtitleText.color = textColor;
        }

        yield return new WaitForSeconds(subtitleDuration);

        if (subtitleText != null)
        {
            Color textColor = subtitleText.color;

            float timer = 0f;
            while (timer < fadeDuration)
            {
                timer += Time.deltaTime;
                textColor.a = Mathf.Clamp01(1 - (timer / fadeDuration));
                subtitleText.color = textColor;
                yield return null;
            }

            textColor.a = 0;
            subtitleText.color = textColor;
            subtitleText.gameObject.SetActive(false);
        }

        subtitleCoroutine = null;
    }

    public void OpenSettingsPanel()
    {
        if (playerClapAudioSource != null)
        {
            playerClapAudioSource.enabled = false;
        }
    }

    public void CloseSettingsPanel()
    {
        if (playerClapAudioSource != null)
        {
            playerClapAudioSource.enabled = true;
        }
    }
}
