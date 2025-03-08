using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class PauseMenu : MonoBehaviour
{
    public GameObject settingsPanel; // Panel des paramètres
    public GameObject journalPanel; // Panel du journal de bord et des missions
    public GameObject reticleCanvas; // Canvas du réticule
    public GameObject puzzleCanvas; // Canvas des puzzles
    public TMP_Text countdownText; // Texte du décompte
    public Animator playerAnimator; // Référence à l'Animator du joueur

    private bool isPaused = false; // État du jeu (en pause ou non)
    private bool isCountingDown = false; // Empêche de rouvrir le menu pendant le décompte
    private float timeSinceSceneStart = 0f; // Temps écoulé depuis le lancement de la scène
    private bool canPause = false; // Permet ou non de mettre en pause
    private bool isSettingsPanelActive = true; // Indique quel panel est actif (true = settingsPanel, false = journalPanel)
    public MonoBehaviour cameraControlScript; // Référence au script qui gère la rotation de la caméra
    private FMOD.Studio.EventInstance musicInstance;//

    void Start()
    {
        musicInstance = FMODUnity.RuntimeManager.CreateInstance("event:/System/Music/Music_gameplay");///
        musicInstance.start(); ///

        // Vérifie que les panels sont assignés
        if (settingsPanel == null)
        {
            Debug.LogError("Le panel des paramètres n'est pas assigné dans l'Inspector.");
            return;
        }

        if (journalPanel == null)
        {
            Debug.LogError("Le panel du journal n'est pas assigné dans l'Inspector.");
            return;
        }

        if (reticleCanvas == null)
        {
            Debug.LogError("Le ReticleCanvas n'est pas assigné dans l'Inspector.");
            return;
        }

        if (puzzleCanvas == null)
        {
            Debug.LogError("Le PuzzleCanvas n'est pas assigné dans l'Inspector.");
            return;
        }

        if (countdownText == null)
        {
            Debug.LogError("Le texte du décompte n'est pas assigné dans l'Inspector.");
            return;
        }

        // Désactive les panels au démarrage
        settingsPanel.SetActive(false);
        journalPanel.SetActive(false);
        countdownText.gameObject.SetActive(false);

        // Verrouille la souris au démarrage
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Mets à jour le temps écoulé depuis le début de la scène
        timeSinceSceneStart += Time.deltaTime;

        // Permet de mettre en pause uniquement après 6 secondes
        if (timeSinceSceneStart >= 6f)
        {
            canPause = true;
        }

        // Ouvre ou ferme le menu pause
        if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("Pause")) && !isCountingDown && canPause)
        {
            TogglePauseMenu();
        }

        // Switch entre les panels seulement si le menu est ouvert
        if (isPaused)
        {
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                SwitchPanel(false); // Aller au panel du journal
            }
            else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                SwitchPanel(true); // Aller au panel des paramètres
            }
        }
    }

    void TogglePauseMenu()
    {
        isPaused = !isPaused;

        // Active ou désactive le panel
        settingsPanel.SetActive(isPaused);
        journalPanel.SetActive(false); // Toujours commencer sur le settingsPanel
        isSettingsPanelActive = true;

        // Active ou désactive le ReticleCanvas et le PuzzleCanvas
        reticleCanvas.SetActive(!isPaused);

        if (isPaused)
        {
            puzzleCanvas.SetActive(false);
            musicInstance.setPaused(true); ///
            FMODUnity.RuntimeManager.PlayOneShot("event:/System/Music/Music_gameplay_pause");
        }

        // Désactive l'Animator du joueur si le jeu est en pause
        if (playerAnimator != null)
        {
            playerAnimator.enabled = !isPaused;
        }

        // Désactive le script de contrôle de la caméra lorsque le jeu est en pause
        if (cameraControlScript != null) cameraControlScript.enabled = !isPaused;

        // Met le jeu en pause ou le relance
        Time.timeScale = isPaused ? 0 : 1;

        // Gère l'état de la souris
        if (isPaused)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            StartCoroutine(ResumeWithCountdown());
            FMODUnity.RuntimeManager.PlayOneShot("event:/System/Music/Music_gameplay_play_decompte");
        }
    }

    void SwitchPanel(bool showSettingsPanel)
    {
        isSettingsPanelActive = showSettingsPanel;
        settingsPanel.SetActive(isSettingsPanelActive);
        journalPanel.SetActive(!isSettingsPanelActive);
    }

    IEnumerator ResumeWithCountdown()
    {
        isCountingDown = true;

        // Désactive le ReticleCanvas et le PuzzleCanvas pendant le décompte
        reticleCanvas.SetActive(false);

        // Affiche le texte du décompte
        countdownText.gameObject.SetActive(true);

        // Assure que le jeu reste en pause pendant le décompte
        Time.timeScale = 0;

        for (int i = 3; i > 0; i--)
        {
            countdownText.text = i.ToString();
            yield return new WaitForSecondsRealtime(1f);
            FMODUnity.RuntimeManager.PlayOneShot("event:/System/Music/Music_gameplay_play_decompte");
        }

        // Cache le texte du décompte
        countdownText.gameObject.SetActive(false);

        // Relance le jeu après le décompte
        isPaused = false;
        settingsPanel.SetActive(false);
        journalPanel.SetActive(false);
        reticleCanvas.SetActive(true);
        puzzleCanvas.SetActive(true);
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        isCountingDown = false;

        FMODUnity.RuntimeManager.PlayOneShot("event:/System/Music/Music_gameplay_play");
        musicInstance.setPaused(false); ///

        // Réactive le contrôle de la caméra
        StartCoroutine(EnableCameraControlWithDelay(3f));
    }

    public void ResumeGame() // Relancer le jeu
    {
        isPaused = false;
        settingsPanel.SetActive(false);
        journalPanel.SetActive(false);
        reticleCanvas.SetActive(true);
        puzzleCanvas.SetActive(true);
        StartCoroutine(ResumeWithCountdown());
    }

    private IEnumerator EnableCameraControlWithDelay(float delay) // Attendre 3sec avant de réactiver ce script
    {
        yield return new WaitForSecondsRealtime(delay);

        if (cameraControlScript != null)
            cameraControlScript.enabled = true;
    }
}
