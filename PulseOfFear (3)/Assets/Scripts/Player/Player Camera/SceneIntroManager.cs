using System.Collections;
using UnityEngine;
using TMPro; // Pour TextMeshPro
using UnityStandardAssets.ImageEffects; // Pour le Tonemapping

public class SceneIntroManager : MonoBehaviour
{
    [Header("UI & Effects")]
    [SerializeField] private TextMeshProUGUI introText; // Texte d'introduction
    [SerializeField] private Tonemapping tonemapping; // Référence au Tonemapping
    [SerializeField] private float textFadeDuration = 2f; // Durée du fondu du texte
    [SerializeField] private float screenFadeDuration = 2f; // Durée du fondu de l'écran noir
    [SerializeField] private GameObject puzzleCanvas; // Référence au PuzzleCanvas

    [Header("Player References")]
    [SerializeField] private GameObject player; // Référence à l'objet Player
    private Animate clapAnimate; // Référence au script Animate
    private FirstPersonController firstPersonController; // Référence au script FirstPersonController
    private AudioListener audioListener; // Référence à l'AudioListener du joueur

    private const float initialExposure = 0.001f; // Valeur initiale de l'exposition
    private const float targetExposure = 1.5f; // Valeur cible de l'exposition
    private const float playerControlDelay = 2f; // Délai avant de rendre les contrôles au joueur
    private const float puzzleCanvasDelay = 0.1f; // Temps avant la réapparition du PuzzleCanvas après le fade

    private void Start()
    {
        // Récupère les composants depuis l'objet Player
        if (player != null)
        {
            clapAnimate = player.GetComponent<Animate>();
            firstPersonController = player.GetComponent<FirstPersonController>();
            audioListener = player.GetComponent<AudioListener>();
        }

        // Désactive les scripts et l'AudioListener du joueur au démarrage
        if (clapAnimate != null) clapAnimate.enabled = false;
        if (firstPersonController != null) firstPersonController.enabled = false;
        if (audioListener != null) audioListener.enabled = false;

        // Désactive le PuzzleCanvas au démarrage (au cas où)
        if (puzzleCanvas != null) puzzleCanvas.SetActive(false);

        // Configure le tonemapping
        if (tonemapping != null)
        {
            tonemapping.type = Tonemapping.TonemapperType.Photographic;
            tonemapping.exposureAdjustment = initialExposure;
        }

        // Initialise le texte en le rendant invisible
        if (introText != null)
        {
            introText.alpha = 0f;
        }

        // Lance l'introduction de la scène
        StartCoroutine(PlaySceneIntro());
    }

    private IEnumerator PlaySceneIntro()
    {
        // Affichage progressif du texte d'introduction
        if (introText != null)
        {
            yield return StartCoroutine(FadeTextIn());
            yield return new WaitForSeconds(2f); // Garde le texte visible pendant 2 secondes
            yield return StartCoroutine(FadeTextOut());
        }

        // Rends les contrôles du joueur après 2 secondes
        yield return StartCoroutine(EnablePlayerControlsAfterDelay(playerControlDelay));

        // Transition progressive de l'écran noir à la scène visible
        if (tonemapping != null)
        {
            yield return StartCoroutine(FadeScreenIn());

            // Après 3 secondes, réaffiche le PuzzleCanvas
            StartCoroutine(ShowPuzzleCanvasAfterDelay(puzzleCanvasDelay));
        }
    }

    private IEnumerator FadeTextIn()
    {
        float timer = 0f;

        while (timer < textFadeDuration)
        {
            timer += Time.deltaTime;
            float progress = timer / textFadeDuration;

            if (introText != null)
            {
                introText.alpha = Mathf.Clamp01(progress); // Rend le texte visible progressivement
            }

            yield return null;
        }

        if (introText != null)
        {
            introText.alpha = 1f; // Assure que le texte est complètement visible
        }
    }

    private IEnumerator FadeTextOut()
    {
        float timer = 0f;

        while (timer < textFadeDuration)
        {
            timer += Time.deltaTime;
            float progress = timer / textFadeDuration;

            if (introText != null)
            {
                introText.alpha = Mathf.Clamp01(1f - progress); // Rend le texte invisible progressivement
            }

            yield return null;
        }

        if (introText != null)
        {
            introText.alpha = 0f; // Assure que le texte est complètement invisible
        }
    }

    private IEnumerator FadeScreenIn()
    {
        float timer = 0f;

        // Désactive le PuzzleCanvas au moment du FadeIn
        if (puzzleCanvas != null)
        {
            puzzleCanvas.SetActive(false);
        }

        while (timer < screenFadeDuration)
        {
            timer += Time.deltaTime;
            float progress = timer / screenFadeDuration;

            if (tonemapping != null)
            {
                tonemapping.exposureAdjustment = Mathf.Lerp(initialExposure, targetExposure, progress);
            }

            yield return null;
        }

        if (tonemapping != null)
        {
            tonemapping.exposureAdjustment = targetExposure; // Assure que l'écran est complètement visible
        }
    }

    private IEnumerator EnablePlayerControlsAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Réactive les scripts et l'AudioListener après le délai
        if (clapAnimate != null) clapAnimate.enabled = true;
        if (firstPersonController != null) firstPersonController.enabled = true;
        if (audioListener != null) audioListener.enabled = true;
    }

    private IEnumerator ShowPuzzleCanvasAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Réactive le PuzzleCanvas après le délai
        if (puzzleCanvas != null)
        {
            puzzleCanvas.SetActive(true);
        }
    }
}
