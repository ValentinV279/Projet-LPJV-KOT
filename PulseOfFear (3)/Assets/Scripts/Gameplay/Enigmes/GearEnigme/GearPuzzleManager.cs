using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GearPuzzleManager : MonoBehaviour
{
    public GameObject gear1, gear2, gear3; // Les engrenages sur le panneau
    public Light spotlight; // Lumière qui change de couleur une fois les engrenages placés

    public AudioSource gearPanelAudioSource; // AudioSource attachée au GearPanel
    public AudioClip gearPlacementSound; // Son de placement d'engrenage
    public AudioClip panelCompletionLoopSound; // Son en boucle après le placement complet des engrenages
    public AudioClip gearCollectSound; // Son de collecte d'engrenage

    public TextMeshProUGUI gearCountText; // Texte affichant le nombre d'engrenages collectés
    public Image gearImage; // UI représentant les engrenages collectés

    public SubtitleManager subtitleManager; // Référence au gestionnaire de sous-titres

    public TextMeshProUGUI collectGearsText; // Texte barré lorsqu'on collecte tous les engrenages
    public TextMeshProUGUI repairMachineText; // Texte barré lorsqu'on répare la machine

    private int collectedGears = 0; // Nombre d'engrenages collectés
    private int totalGears = 3; // Nombre total d'engrenages nécessaires
    private int activatedGears = 0; // Compteur des engrenages activés sur le panneau

    private float lastSubtitleTime = -20f; // Temps du dernier affichage d'un sous-titre
    private float subtitleCooldown = 20f; // Temps de recharge avant d'afficher un nouveau sous-titre

    void Start()
    {
        // Désactiver les engrenages sur le panneau au départ
        if (gear1) gear1.SetActive(false);
        if (gear2) gear2.SetActive(false);
        if (gear3) gear3.SetActive(false);

        // Désactiver l'affichage du nombre d'engrenages collectés au début
        if (gearCountText) gearCountText.gameObject.SetActive(false);
        if (gearImage) gearImage.gameObject.SetActive(false);

        // Vérification des composants
        if (gearPanelAudioSource == null)
        {
            Debug.LogError("Aucune AudioSource assignée au GearPanel.");
        }
        else
        {
            gearPanelAudioSource.spatialBlend = 1.0f; // Son 3D
        }

        if (subtitleManager == null)
        {
            Debug.LogError("Aucune référence à SubtitleManager n'a été assignée.");
        }
    }

    public void CollectGear(GameObject gear)
    {
        Destroy(gear);
        collectedGears++;
        UpdateGearUI();
        Debug.Log($"Engrenage collecté ! Total : {collectedGears}/{totalGears}");
        
        // Jouer le son de collecte d'engrenage
        if (gearPanelAudioSource != null && gearCollectSound != null)
        {
            gearPanelAudioSource.PlayOneShot(gearCollectSound);
        }

        if (collectedGears == totalGears)
        {
            TaskManager.Instance.CompleteTask();
            if (collectGearsText != null)
            {
                collectGearsText.fontStyle = FontStyles.Strikethrough;
            }
        }
    }

    void UpdateGearUI()
    {
        if (gearCountText) gearCountText.text = $"{collectedGears}/{totalGears}";
        if (gearImage && !gearImage.gameObject.activeSelf) gearImage.gameObject.SetActive(true);
        if (gearCountText && !gearCountText.gameObject.activeSelf) gearCountText.gameObject.SetActive(true);
    }

    public void InteractWithGearPanel()
    {
        if (collectedGears < totalGears)
        {
            if (Time.time - lastSubtitleTime >= subtitleCooldown)
            {
                lastSubtitleTime = Time.time;
                string subtitle = $"Hmm... It seems I'm missing {totalGears - collectedGears} gear(s) to restart the mechanism.";
                subtitleManager.ShowSubtitle(subtitle);
            }
            return;
        }

        activatedGears++;

        if (gearPanelAudioSource != null && gearPlacementSound != null)
        {
            gearPanelAudioSource.spatialBlend = 1.0f;
            gearPanelAudioSource.PlayOneShot(gearPlacementSound);
        }

        if (activatedGears == 1 && gear1 != null)
        {
            gear1.SetActive(true);
            Debug.Log("Premier engrenage activé.");
        }
        else if (activatedGears == 2 && gear2 != null)
        {
            gear2.SetActive(true);
            Debug.Log("Deuxième engrenage activé.");
        }
        else if (activatedGears == 3 && gear3 != null)
        {
            gear3.SetActive(true);
            Debug.Log("Troisième engrenage activé.");

            if (spotlight != null)
            {
                spotlight.color = Color.blue;
                Debug.Log("Spotlight passé en bleu avec succès.");
            }
            else
            {
                Debug.LogWarning("Le Spotlight n'est pas assigné dans l'inspecteur !");
            }

            if (gearPanelAudioSource != null && panelCompletionLoopSound != null)
            {
                gearPanelAudioSource.clip = panelCompletionLoopSound;
                gearPanelAudioSource.loop = true;
                gearPanelAudioSource.Play();
                Debug.Log("Son de complétion du panneau joué en boucle.");
            }

            subtitleManager.ShowSubtitle("Perfect! The machine seems repaired.");
            TaskManager.Instance.CompleteTask();
            if (repairMachineText != null)
            {
                repairMachineText.fontStyle = FontStyles.Strikethrough;
            }
        }
    }
}
