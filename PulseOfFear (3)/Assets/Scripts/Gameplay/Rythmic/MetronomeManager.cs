using UnityEngine;
using System.Collections;

public class MetronomeManager : MonoBehaviour
{
    public AudioClip bpm40; // Audio clip pour 40 BPM
    public AudioClip bpm50; // Audio clip pour 50 BPM
    public AudioClip bpm60; // Audio clip pour 60 BPM
    public AudioClip bpm70; // Audio clip pour 70 BPM
    public AudioClip bpm80; // Audio clip pour 80 BPM

    private AudioSource audioSource;
    private float levelDuration = 300f; // Durée du niveau en secondes (5 minutes)
    private float bpmInterval = 60f; // Intervalle de changement de BPM (1 minute)
    private int currentBPMIndex = 0; // Index actuel du BPM

    private AudioClip[] bpmClips; // Tableau pour stocker les clips
    private int[] bpmValues = { 40, 50, 60, 70, 80 }; // Correspondance des BPM

    void Start()
    {
        // Initialisation de l'AudioSource et des clips
        audioSource = GetComponent<AudioSource>();
        bpmClips = new AudioClip[] { bpm40, bpm50, bpm60, bpm70, bpm80 };

        // Lancer la boucle pour le premier clip
        PlayMetronome();
        InvokeRepeating(nameof(UpdateMetronome), bpmInterval, bpmInterval); // Appelle toutes les 60s
    }

    void PlayMetronome()
    {
        if (currentBPMIndex < bpmClips.Length)
        {
            audioSource.clip = bpmClips[currentBPMIndex];
            audioSource.loop = true; // Active la boucle
            audioSource.Play();
        }
    }

    void UpdateMetronome()
    {
        if (currentBPMIndex < bpmClips.Length - 1) // Vérifie s'il reste des clips
        {
            currentBPMIndex++; // Passe au clip suivant
            PlayMetronome(); // Relance le métronome avec le nouveau clip
        }
        else
        {
            CancelInvoke(nameof(UpdateMetronome)); // Arrête les mises à jour après 5 minutes
        }
    }

    public int GetCurrentBPM()
    {
        return bpmValues[currentBPMIndex]; // Retourne le BPM actuel
    }
}
