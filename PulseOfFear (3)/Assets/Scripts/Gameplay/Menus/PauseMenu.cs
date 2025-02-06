using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class PauseMenu : MonoBehaviour
{
    public GameObject settingsPanel; // Panel des paramètres
    public GameObject reticleCanvas; // Canvas du réticule
    public GameObject puzzleCanvas; // Canvas des puzzles
    public AudioListener playerAudioListener; // AudioListener du joueur
    public TMP_Text countdownText; // Texte du décompte

    private bool isPaused = false; // État du jeu (en pause ou non)
    private bool isCountingDown = false; // Empêche de rouvrir le menu pendant le décompte

    void Start()
    {
        // Vérifie que le panel est assigné
        if (settingsPanel == null)
        {
            Debug.LogError("Le panel des paramètres n'est pas assigné dans l'Inspector.");
            return;
        }

        // Vérifie que l'AudioListener est assigné
        if (playerAudioListener == null)
        {
            Debug.LogError("L'AudioListener du joueur n'est pas assigné dans l'Inspector.");
            return;
        }

        // Vérifie que le ReticleCanvas est assigné
        if (reticleCanvas == null)
        {
            Debug.LogError("Le ReticleCanvas n'est pas assigné dans l'Inspector.");
            return;
        }

        // Vérifie que le PuzzleCanvas est assigné
        if (puzzleCanvas == null)
        {
            Debug.LogError("Le PuzzleCanvas n'est pas assigné dans l'Inspector.");
            return;
        }

        // Vérifie que le texte du décompte est assigné
        if (countdownText == null)
        {
            Debug.LogError("Le texte du décompte n'est pas assigné dans l'Inspector.");
            return;
        }

        // Assure que le panel et le texte sont désactivés au début
        settingsPanel.SetActive(false);
        countdownText.gameObject.SetActive(false);

        // Verrouille la souris au démarrage
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Vérifie si le joueur appuie sur Echap (clavier) ou sur le bouton pause de la manette via l'Input Manager
        if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("Pause")) && !isCountingDown)
        {
            TogglePauseMenu();
        }
    }

    void TogglePauseMenu()
    {
        isPaused = !isPaused;

        // Active ou désactive le panel
        settingsPanel.SetActive(isPaused);

        // Active ou désactive le ReticleCanvas et le PuzzleCanvas
        reticleCanvas.SetActive(!isPaused);

        if (isPaused)
        {
            puzzleCanvas.SetActive(false);
        }

        // Met le jeu en pause ou le relance
        Time.timeScale = isPaused ? 0 : 1;

        // Active ou désactive l'AudioListener
        playerAudioListener.enabled = !isPaused;

        // Gère l'état de la souris
        if (isPaused)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            StartCoroutine(ResumeWithCountdown());
        }
    }

    IEnumerator ResumeWithCountdown()
    {
        isCountingDown = true;

        // Désactive le ReticleCanvas, le PuzzleCanvas et l'AudioListener pendant le décompte
        reticleCanvas.SetActive(false);
        playerAudioListener.enabled = false;

        // Affiche le texte du décompte
        countdownText.gameObject.SetActive(true);

        // Assure que le jeu reste en pause pendant le décompte
        Time.timeScale = 0;

        for (int i = 3; i > 0; i--)
        {
            countdownText.text = i.ToString();
            yield return new WaitForSecondsRealtime(1f);
        }

        // Cache le texte du décompte
        countdownText.gameObject.SetActive(false);

        // Relance le jeu après le décompte
        isPaused = false;
        settingsPanel.SetActive(false);
        reticleCanvas.SetActive(true);
        puzzleCanvas.SetActive(true);
        playerAudioListener.enabled = true;
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        isCountingDown = false;
    }

    public void ResumeGame() //Relancer le jeu
    {
        // Méthode appelée via un bouton pour reprendre le jeu
        isPaused = false;
        settingsPanel.SetActive(false);
        reticleCanvas.SetActive(true);
        puzzleCanvas.SetActive(true);
        StartCoroutine(ResumeWithCountdown());
    }
}
