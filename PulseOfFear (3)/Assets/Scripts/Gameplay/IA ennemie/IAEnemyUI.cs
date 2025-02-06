using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using UnityStandardAssets.ImageEffects;

public class IAEnemyUI : MonoBehaviour
{
    [SerializeField] Camera enemyCamera;
    [SerializeField] Camera playerCamera;
    [SerializeField] TextMeshProUGUI gameOverText;
    [SerializeField] TextMeshProUGUI additionalText;
    [SerializeField] TextMeshProUGUI gameOverTextToCatch;
    [SerializeField] GameObject restartButton;
    [SerializeField] Image additionalUIElement;
    [SerializeField] Tonemapping tonemapping;
    [SerializeField] float fadeDuration = 2.5f;
    [SerializeField] Animator animator;

    // Références des Canvas
    [SerializeField] GameObject reticleCanvas;
    [SerializeField] GameObject puzzleCanvas;
    [SerializeField] GameObject settingsCanvas;
    [SerializeField] GameObject subtitlesCanvas;

    void Start()
    {
        // Initialisation des caméras
        if (enemyCamera != null) enemyCamera.enabled = false;
        if (playerCamera != null) playerCamera.enabled = true;

        // Désactivation des éléments UI au lancement
        if (gameOverText != null) gameOverText.gameObject.SetActive(false);
        if (additionalText != null) additionalText.gameObject.SetActive(false);
        if (gameOverTextToCatch != null) gameOverTextToCatch.gameObject.SetActive(false);
        if (restartButton != null) restartButton.SetActive(false);
        if (additionalUIElement != null) additionalUIElement.gameObject.SetActive(true);

        // Réinitialisation du tonemapping
        if (tonemapping != null) tonemapping.exposureAdjustment = 1.5f;
    }

    public void CapturePlayerEffects()
    {
        // Désactivation des autres AudioSources dans la scène
        MuteAllAudioExceptEnemy();

        // Activation de la caméra de l'ennemi
        if (playerCamera != null) playerCamera.enabled = false;
        if (enemyCamera != null) enemyCamera.enabled = true;

        // Activation du booléen "Desactivate" dans l'Animator
        if (animator != null)
        {
            animator.SetBool("Desactivate", true);
        }

        // Affichage du curseur
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Désactivation des Canvas
        if (reticleCanvas != null) reticleCanvas.SetActive(false);
        if (puzzleCanvas != null) puzzleCanvas.SetActive(false);
        if (settingsCanvas != null) settingsCanvas.SetActive(false);
        if (subtitlesCanvas != null) subtitlesCanvas.SetActive(false);

        // Affichage et fade-in de GameOverTextToCatch
        if (gameOverTextToCatch != null)
        {
            gameOverTextToCatch.gameObject.SetActive(true);
            StartCoroutine(FadeInText(gameOverTextToCatch));
        }

        // Désactivation de l'élément UI supplémentaire
        if (additionalUIElement != null)
        {
            additionalUIElement.gameObject.SetActive(false);
        }

        // Début du tremblement de la caméra
        var cameraShake = enemyCamera.GetComponent<CameraShake>();
        if (cameraShake != null)
        {
            cameraShake.StartShake(2f, 0.05f); // Durée : 2s, Intensité : 0.05
        }

        // Appel des effets de Game Over
        StartCoroutine(FadeOutAndGameOver());
    }

    private void MuteAllAudioExceptEnemy()
    {
        // Récupérer toutes les AudioSources de la scène
        AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>();

        foreach (AudioSource audioSource in allAudioSources)
        {
            // Si l'AudioSource n'est pas attachée à l'ennemi, la couper
            if (audioSource.gameObject != gameObject)
            {
                audioSource.Stop();
            }
        }
    }

    private IEnumerator FadeInText(TextMeshProUGUI text)
    {
        float timer = 0f;
        text.alpha = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            text.alpha = Mathf.Clamp01(timer / fadeDuration);
            yield return null;
        }

        text.alpha = 1f; // Assure que l'alpha atteint 1
    }

    private IEnumerator FadeOutAndGameOver()
    {
        yield return new WaitForSeconds(2f);

        float startExposure = tonemapping != null ? tonemapping.exposureAdjustment : 1.5f;
        float timer = 0f;

        if (gameOverText != null)
        {
            gameOverText.gameObject.SetActive(true);
            gameOverText.alpha = 0f;
        }

        if (restartButton != null)
        {
            restartButton.SetActive(true);
        }

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float progress = timer / fadeDuration;

            if (gameOverText != null) gameOverText.alpha = Mathf.Clamp01(progress);
            if (tonemapping != null)
            {
                tonemapping.exposureAdjustment = Mathf.Lerp(startExposure, 0f, progress);
            }

            yield return null;
        }

        if (gameOverText != null) gameOverText.alpha = 1f;
        if (tonemapping != null) tonemapping.exposureAdjustment = 0f;
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
