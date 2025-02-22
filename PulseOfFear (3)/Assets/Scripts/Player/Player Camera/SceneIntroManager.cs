using System.Collections;
using UnityEngine;
using UnityEngine.UI; // Pour le fade avec une Image UI
using TMPro; // Pour TextMeshPro

public class SceneIntroManager : MonoBehaviour
{
    [Header("UI & Effects")]
    [SerializeField] private TextMeshProUGUI introText; // Texte d'introduction
    [SerializeField] private Image fadeImage; // Image noire pour le fondu (URP)
    [SerializeField] private float screenBlackDuration = 5f; // Temps où l'écran reste noir
    [SerializeField] private float fadeDuration = 1f; // Durée du fade noir + texte
    [SerializeField] private GameObject puzzleCanvas; // Référence au PuzzleCanvas

    [Header("Player References")]
    [SerializeField] private GameObject player; // Référence à l'objet Player
    private Rigidbody playerRigidbody;
    private Animate clapAnimate;
    private FirstPersonController firstPersonController;
    private AudioListener audioListener;
    private Collider playerCollider;
    
    private void Start()
    {
        if (player != null)
        {
            clapAnimate = player.GetComponent<Animate>();
            firstPersonController = player.GetComponent<FirstPersonController>();
            audioListener = player.GetComponent<AudioListener>();
            playerRigidbody = player.GetComponent<Rigidbody>();
            playerCollider = player.GetComponent<Collider>();
        }

        // Désactiver tous les mouvements et entrées du joueur
        DisablePlayerMovement();

        // Désactive le PuzzleCanvas au démarrage
        if (puzzleCanvas != null) puzzleCanvas.SetActive(false);

        // Configure l'image noire (fade) et le texte
        if (fadeImage != null) fadeImage.color = new Color(0f, 0f, 0f, 1f); // Noir opaque au début
        if (introText != null) introText.alpha = 1f; // Texte totalement visible au début

        // Lance l'introduction de la scène
        StartCoroutine(PlaySceneIntro());
    }

    private IEnumerator PlaySceneIntro()
    {
        // Garde l’écran noir et le texte affiché pendant 5 secondes
        yield return new WaitForSeconds(screenBlackDuration);

        // Fade-out simultané du noir et du texte (1s)
        if (fadeImage != null && introText != null)
        {
            yield return StartCoroutine(FadeOutBlackAndText());
        }

        // Activer le joueur après le fade-out
        EnablePlayerMovement();

        // Réafficher le PuzzleCanvas après un court délai
        StartCoroutine(ShowPuzzleCanvasAfterDelay(0.1f));
    }

    private IEnumerator FadeOutBlackAndText()
    {
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = 1f - (timer / fadeDuration);

            if (fadeImage != null) fadeImage.color = new Color(0f, 0f, 0f, alpha);
            if (introText != null) introText.alpha = alpha;

            yield return null;
        }

        // Assurer la fin du fade
        if (fadeImage != null) fadeImage.color = new Color(0f, 0f, 0f, 0f);
        if (introText != null) introText.alpha = 0f;
    }

    private void DisablePlayerMovement()
    {
        if (clapAnimate != null) clapAnimate.enabled = false;
        if (firstPersonController != null) firstPersonController.enabled = false;
        if (audioListener != null) audioListener.enabled = false;

        if (playerRigidbody != null)
        {
            playerRigidbody.isKinematic = true; // Empêche les forces externes
        }

        if (playerCollider != null)
        {
            playerCollider.enabled = false; // Désactive les collisions
        }
    }

    private void EnablePlayerMovement()
    {
        if (clapAnimate != null) clapAnimate.enabled = true;
        if (firstPersonController != null) firstPersonController.enabled = true;
        if (audioListener != null) audioListener.enabled = true;

        if (playerRigidbody != null)
        {
            playerRigidbody.isKinematic = false; // Réactive la physique
        }

        if (playerCollider != null)
        {
            playerCollider.enabled = true; // Réactive les collisions
        }
    }

    private IEnumerator ShowPuzzleCanvasAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (puzzleCanvas != null) puzzleCanvas.SetActive(true);
    }
}
