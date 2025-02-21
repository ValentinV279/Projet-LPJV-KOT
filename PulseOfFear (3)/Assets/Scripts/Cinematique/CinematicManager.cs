using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using System.Collections;

public class CinematicManager : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public AudioSource voiceOverSource; // Voix off
    public AudioSource bgmSource;       // Musique de fond
    public VideoClip[] videoClips;
    public AudioClip[] voiceOverClips;
    
    public AudioClip bgm1; // Musique pour les plans 1-2
    public AudioClip bgm2; // Musique pour les plans 4-6
    
    private int currentClipIndex = 0;
    private bool isSkipping = false;
    private Coroutine currentCoroutine;

    void Start()
    {
        if (videoClips.Length == 0 || voiceOverClips.Length == 0)
        {
            Debug.LogError("Les tableaux videoClips ou voiceOverClips sont vides !");
            return;
        }

        PlayClip(currentClipIndex);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && !isSkipping) // Skip vidéo avec Entrée
        {
            isSkipping = true;
            SkipClip();
        }
    }

    void PlayClip(int index)
    {
        if (index >= videoClips.Length || index >= voiceOverClips.Length) 
        {
            Debug.Log("Cinematic finished!");
            return; // Fin de la cinématique
        }

        Debug.Log("Lecture du plan " + index);

        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine); // Stopper la lecture actuelle proprement
        }
        currentCoroutine = StartCoroutine(PlayClipWithAudio(index));
    }

    IEnumerator PlayClipWithAudio(int index)
    {
        isSkipping = false;

        // Charger la vidéo et la voix off
        videoPlayer.Stop();
        voiceOverSource.Stop();

        if (videoClips[index] == null || voiceOverClips[index] == null)
        {
            Debug.LogError("Le clip vidéo ou audio à l'index " + index + " est manquant !");
            NextClip();
            yield break;
        }

        videoPlayer.clip = videoClips[index];
        voiceOverSource.clip = voiceOverClips[index];

        videoPlayer.Prepare();
        while (!videoPlayer.isPrepared)
        {
            yield return null;
        }

        // Démarrer la vidéo et la voix off
        videoPlayer.Play();
        voiceOverSource.Play();

        // Gestion de la musique de fond (ne pas perturber la voix off)
        if (index == 0) 
        {
            PlayBGM(bgm1); // Démarrer la musique 1
        }
        else if (index == 2) 
        {
            StopBGM(); // Arrêter la musique 1 à la fin du plan 2
        }
        else if (index == 3) 
        {
            PlayBGM(bgm2); // Démarrer la musique 2
        }
        else if (index == 6) 
        {
            StopBGM(); // Arrêter la musique 2 à la fin du plan 6
        }

        // Attendre uniquement la fin de la voix off avant de passer au prochain plan
        while (voiceOverSource.isPlaying && !isSkipping)
        {
            yield return null;
        }

        NextClip();
    }

    void NextClip()
    {
        if (isSkipping)
        {
            isSkipping = false; // Réinitialiser le skip pour éviter plusieurs sauts
        }

        currentClipIndex++;

        if (currentClipIndex < videoClips.Length && currentClipIndex < voiceOverClips.Length)
        {
            PlayClip(currentClipIndex);
        }
        else
        {
            Debug.Log("Fin de la cinématique !");
            StartCoroutine(LoadNextScene());
        }
    }

    void SkipClip()
    {
        if (currentClipIndex < videoClips.Length - 1) // Vérifier qu'on ne dépasse pas
        {
            currentClipIndex++;
            PlayClip(currentClipIndex);
        }
        else
        {
            Debug.Log("Fin de la cinématique !");
            StartCoroutine(LoadNextScene());
        }
    }

    void PlayBGM(AudioClip clip)
    {
        if (clip != null && bgmSource.clip != clip) 
        {
            bgmSource.clip = clip;
            bgmSource.loop = true;
            bgmSource.Play();
        }
    }

    void StopBGM()
    {
        bgmSource.Stop(); // Arrêt immédiat de la musique
    }

    IEnumerator LoadNextScene()
    {
        // Ajouter un petit délai pour laisser la musique se couper avant de charger la scène
        yield return new WaitForSeconds(1.0f);
        // Charger la prochaine scène (ajouter le nom de votre scène ici)
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
