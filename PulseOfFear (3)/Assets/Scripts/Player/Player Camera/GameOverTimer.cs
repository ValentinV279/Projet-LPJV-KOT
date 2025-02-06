using System.Collections;
using UnityEngine;
using TMPro;
using UnityStandardAssets.ImageEffects;
using UnityEngine.SceneManagement;

public class GameOverTimer : MonoBehaviour
{
    public float levelDuration = 300f;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI additionalText;
    public Tonemapping tonemapping;
    public float fadeDuration = 3f;
    public GameObject gearUI;
    public GameObject gearCountUI;
    public GameObject playerObject;
    public GameObject restartButton;

    public GameObject reticleCanvas;
    public GameObject puzzleCanvas;
    public GameObject settingsCanvas;
    public GameObject subtitlesCanvas;
    
    public SubtitleManager subtitleManager;

    private bool isGameOver = false;
    private Camera playerCamera;
    private MonoBehaviour cameraControlScript;
    private bool subtitleDisplayed = false;

    void Start()
    {
        if (gameOverText != null) gameOverText.gameObject.SetActive(false);
        if (additionalText != null) additionalText.gameObject.SetActive(false);
        if (restartButton != null) restartButton.SetActive(false);
        
        if (timerText != null)
        {
            timerText.gameObject.SetActive(false); // Cache l'UI du timer au départ
            timerText.alpha = 0f; // Définit la transparence à 0
        }

        if (playerObject != null)
        {
            playerCamera = playerObject.GetComponentInChildren<Camera>();
            cameraControlScript = playerObject.GetComponentInChildren<MonoBehaviour>();
        }

        // Démarrer le chronomètre et afficher l'UI progressivement
        StartCoroutine(StartLevelTimer());
        StartCoroutine(FadeInTimerUI()); // Lancement du fondu après 3 secondes
    }

    void Update()
    {
        if (isGameOver && Input.GetKeyDown(KeyCode.Return))
        {
            RestartLevel();
        }
    }

    IEnumerator StartLevelTimer()
    {
        float timeRemaining = levelDuration;

        while (timeRemaining > 0)
        {
            UpdateTimerUI(timeRemaining);
            
            if (timeRemaining <= 60f && !subtitleDisplayed && subtitleManager != null)
            {
                subtitleManager.ShowSubtitle("I don't have much time left !");
                subtitleDisplayed = true;
            }
            
            yield return new WaitForSeconds(1f);
            timeRemaining--;
        }

        if (!isGameOver)
        {
            TriggerGameOver();
        }
    }

    IEnumerator FadeInTimerUI()
    {
        yield return new WaitForSeconds(5f); // Attendre 3 secondes avant l'affichage

        if (timerText != null)
        {
            timerText.gameObject.SetActive(true);
            float fadeTime = 1.5f; // Durée du fondu
            float elapsedTime = 0f;

            while (elapsedTime < fadeTime)
            {
                elapsedTime += Time.deltaTime;
                timerText.alpha = Mathf.Clamp01(elapsedTime / fadeTime);
                yield return null;
            }

            timerText.alpha = 1f; // Assure que l'UI est complètement visible
        }
    }

    void UpdateTimerUI(float time)
    {
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(time / 60);
            int seconds = Mathf.FloorToInt(time % 60);
            timerText.text = $"{minutes}:{seconds:D2}"; // Format MM:SS
        }
    }

    void TriggerGameOver()
    {
        isGameOver = true;

        if (timerText != null) timerText.gameObject.SetActive(false); // Cache le timer après le Game Over

        if (playerObject != null)
        {
            var rb = playerObject.GetComponent<Rigidbody>();
            if (rb != null) rb.isKinematic = true;

            playerObject.GetComponent<Collider>().enabled = false;
        }

        if (cameraControlScript != null) cameraControlScript.enabled = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        MuteAllAudioExceptPlayerCameraAndParent();

        if (gearUI != null) gearUI.SetActive(false);
        if (gearCountUI != null) gearCountUI.SetActive(false);

        if (reticleCanvas != null) reticleCanvas.SetActive(false);
        if (puzzleCanvas != null) puzzleCanvas.SetActive(false);
        if (settingsCanvas != null) settingsCanvas.SetActive(false);
        if (subtitlesCanvas != null) subtitlesCanvas.SetActive(false);

        if (gameOverText != null)
        {
            gameOverText.gameObject.SetActive(true);
            gameOverText.alpha = 0f;
        }

        if (additionalText != null)
        {
            additionalText.gameObject.SetActive(true);
            additionalText.alpha = 0f;
        }

        if (restartButton != null)
        {
            restartButton.SetActive(true);
        }

        StartCoroutine(HandleGameOverEffects());
        StartCoroutine(RotateCameraDown());
    }

    void MuteAllAudioExceptPlayerCameraAndParent()
    {
        AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>();

        foreach (AudioSource audioSource in allAudioSources)
        {
            if (audioSource.gameObject != playerCamera.gameObject && audioSource.gameObject != playerObject)
            {
                audioSource.Stop();
            }
        }
    }

    IEnumerator HandleGameOverEffects()
    {
        float timer = 0f;
        float startExposure = tonemapping != null ? tonemapping.exposureAdjustment : 1.5f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float progress = timer / fadeDuration;

            if (gameOverText != null) gameOverText.alpha = Mathf.Clamp01(progress);
            if (additionalText != null) additionalText.alpha = Mathf.Clamp01(progress);
            if (tonemapping != null) tonemapping.exposureAdjustment = Mathf.Lerp(startExposure, 0f, progress);

            yield return null;
        }

        if (gameOverText != null) gameOverText.alpha = 1f;
        if (additionalText != null) additionalText.alpha = 1f;
        if (tonemapping != null) tonemapping.exposureAdjustment = 0f;
    }

    IEnumerator RotateCameraDown()
    {
        float duration = 3f;
        float elapsed = 0f;

        Vector3 startRotation = playerCamera.transform.localEulerAngles;
        float startX = startRotation.x > 180 ? startRotation.x - 360 : startRotation.x;
        float endRotationX = 55f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            float newRotationX = Mathf.Lerp(startX, endRotationX, t);
            playerCamera.transform.localEulerAngles = new Vector3(newRotationX, startRotation.y, startRotation.z);

            yield return null;
        }

        playerCamera.transform.localEulerAngles = new Vector3(55f, startRotation.y, startRotation.z);
    }

    public void RestartLevel()
    {
        Debug.Log("Bouton cliqué ou Enter pressé !");
        Time.timeScale = 1f;

        if (tonemapping != null)
        {
            tonemapping.exposureAdjustment = 0.001f;
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
