using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using ClockSample;

public class InteractionManager : MonoBehaviour
{
    public float raycastDistance = 2f;
    public Image reticleBase;
    public Image reticleInteraction;
    public GameObject bookPreviewPanel;

    private bool isLookingAtClapBook = false;
    private bool isPreviewVisible = false;
    private Drawer currentDrawer = null;
    private WardrobeDoor currentWardrobeDoor = null;
    private BasicDoorController currentDoor = null;
    private ChestLid currentChestLid = null;
    private int currentLeverIndex = -1;

    public GearPuzzleManager gearPuzzleManager;
    private GameObject currentGear = null;
    private bool isLookingAtGearPanel = false;

    public SubtitleManager subtitleManager;
    public SecondaryClock clockManager;

    private DoorController specialDoor = null;
    private ExitDoubleDoor exitDoor = null;
    private bool hasShownSpecialDoorSubtitle = false;
    private bool hasShownExitDoorSubtitle = false;
    public AudioSource leverAudioSource;
    public AudioClip leverSound;

    void Start()
    {
        reticleBase.gameObject.SetActive(true);
        reticleInteraction.gameObject.SetActive(false);
        if (bookPreviewPanel) bookPreviewPanel.SetActive(false);

        if (!subtitleManager)
            Debug.LogError("Aucune référence à SubtitleManager n'a été assignée.");
        
        if (!gearPuzzleManager)
            Debug.LogError("Aucune référence à GearPuzzleManager !");

        // Initialisation de l'AudioSource pour le levier
        if (leverAudioSource == null)
        {
            leverAudioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        HandleRaycast();

        // Prise en charge du bouton d'interaction clavier et manette
        if (Input.GetKeyDown(KeyCode.E) || Input.GetButtonDown("Interact"))  
        {
            if (isLookingAtClapBook)
            {
                TogglePreview();
            }
            else if (currentDrawer != null)
            {
                currentDrawer.ToggleDrawer();
            }
            else if (currentWardrobeDoor != null)
            {
                currentWardrobeDoor.ToggleDoor();
            }
            else if (currentChestLid != null)
            {
                currentChestLid.ToggleLid();
            }
            else if (currentDoor != null)
            {
                currentDoor.ToggleDoor();
            }
            else if (specialDoor != null && !hasShownSpecialDoorSubtitle)
            {
                subtitleManager.ShowSubtitle("This door doesn't seem to open, so let's find a way");
                hasShownSpecialDoorSubtitle = true;
            }
            else if (exitDoor != null && !hasShownExitDoorSubtitle)
            {
                subtitleManager.ShowSubtitle("This door doesn't seem to open, so let's find a way");
                hasShownExitDoorSubtitle = true;
            }
            else if (currentLeverIndex >= 0)
            {
                ToggleClock(currentLeverIndex);
            }
            else if (currentGear != null && gearPuzzleManager != null)
            {
                gearPuzzleManager.CollectGear(currentGear);
                currentGear = null;
            }
            else if (isLookingAtGearPanel && gearPuzzleManager != null)
            {
                gearPuzzleManager.InteractWithGearPanel();
            }
        }
    }

    void HandleRaycast()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, raycastDistance))
        {
            ResetInteractionState();

            if (hit.collider.CompareTag("ClapBook"))
            {
                isLookingAtClapBook = true;
                SetInteractionReticle(true);
            }
            else if (hit.collider.CompareTag("Gear"))
            {
                currentGear = hit.collider.gameObject;
                SetInteractionReticle(true);
            }
            else if (hit.collider.CompareTag("Drawer"))
            {
                currentDrawer = hit.collider.GetComponent<Drawer>();
                SetInteractionReticle(true);
            }
            else if (hit.collider.CompareTag("WardrobeRightDoor") || hit.collider.CompareTag("WardrobeLeftDoor"))
            {
                currentWardrobeDoor = hit.collider.GetComponent<WardrobeDoor>();
                SetInteractionReticle(true);
            }
            else if (hit.collider.CompareTag("Lever"))
            {
                currentLeverIndex = GetLeverIndex(hit.collider.gameObject);
                SetInteractionReticle(true);
            }
            else if (hit.collider.CompareTag("Chest"))
            {
                currentChestLid = hit.collider.GetComponent<ChestLid>();
                SetInteractionReticle(true);
            }
            else if (hit.collider.CompareTag("Door"))
            {
                currentDoor = hit.collider.GetComponent<BasicDoorController>();
                SetInteractionReticle(true);
            }
            else if (hit.collider.CompareTag("SpecialDoor"))
            {
                specialDoor = hit.collider.GetComponent<DoorController>();
                SetInteractionReticle(true);
            }
            else if (hit.collider.CompareTag("ExitLeftDoor") || hit.collider.CompareTag("ExitRightDoor"))
            {
                exitDoor = hit.collider.GetComponent<ExitDoubleDoor>();
                SetInteractionReticle(true);
            }
            else if (hit.collider.CompareTag("GearPanel"))
            {
                isLookingAtGearPanel = true;
                SetInteractionReticle(true);
            }
        }
        else
        {
            ResetInteractionState();
        }
    }

    void ResetInteractionState()
    {
        isLookingAtClapBook = false;
        currentDrawer = null;
        currentWardrobeDoor = null;
        currentChestLid = null;
        currentDoor = null;
        specialDoor = null;
        exitDoor = null;
        currentLeverIndex = -1;
        currentGear = null;
        isLookingAtGearPanel = false;
        SetInteractionReticle(false);
    }

    void PlayLeverSound()
    {
        if (leverAudioSource != null && leverSound != null)
        {
            leverAudioSource.PlayOneShot(leverSound);
        }
    }

    void SetInteractionReticle(bool isActive)
    {
        reticleBase.gameObject.SetActive(!isActive);
        reticleInteraction.gameObject.SetActive(isActive);
    }

    int GetLeverIndex(GameObject lever)
    {
        if (lever.name.Contains("Lever1")) return 0;
        if (lever.name.Contains("Lever2")) return 1;
        if (lever.name.Contains("Lever3")) return 2;
        return -1;
    }

    void ToggleClock(int leverIndex)
    {
        if (clockManager != null)
        {
            bool isRunning = clockManager.ToggleClockState(leverIndex);
            Debug.Log($"Horloge {leverIndex + 1} {(isRunning ? "repartie" : "arrêtée")}");
        }
    }

    void TogglePreview()
    {
        isPreviewVisible = !isPreviewVisible;
        bookPreviewPanel?.SetActive(isPreviewVisible);
        Time.timeScale = isPreviewVisible ? 0 : 1;
    }
}
