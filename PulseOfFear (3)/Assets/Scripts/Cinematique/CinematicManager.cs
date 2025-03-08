using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using System.Collections;

public class CinematicManager : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public AudioSource bgmSource;       // Musique de fond
    public VideoClip[] videoClips;
    
    public AudioClip bgm1; // Musique pour les plans 1-2
    public AudioClip bgm2; // Musique pour les plans 4-6
    
    private int currentClipIndex = 0;
    private bool isSkipping = false;
    private Coroutine currentCoroutine;

    void Start()
    {
        currentClipIndex = 0;
        
        if (videoClips.Length == 0)
        {
            Debug.LogError("Le tableau videoClips est vide !");
            return;
        }

        PlayClip(currentClipIndex);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && !isSkipping)
        {
            isSkipping = true;
            SkipClip();
        }
    }

    void PlayClip(int index)
    {
        if (index >= videoClips.Length)
        {
            Debug.Log("Cinematic finished!");
            StartCoroutine(LoadNextScene());
            return;
        }

        Debug.Log("Lecture du plan " + index);

        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }
        currentCoroutine = StartCoroutine(PlayClipWithDuration(index));
    }

    IEnumerator PlayClipWithDuration(int index)
    {
        isSkipping = false;

        videoPlayer.Stop();
        
        if (videoClips[index] == null)
        {
            Debug.LogError("Le clip vidéo à l'index " + index + " est manquant !");
            NextClip();
            yield break;
        }

        videoPlayer.clip = videoClips[index];

        videoPlayer.Prepare();
        while (!videoPlayer.isPrepared)
        {
            yield return null;
        }

        videoPlayer.Play();

        if (index == 0) 
        {
            PlayBGM(bgm1);
        }
        else if (index == 2) 
        {
            StopBGM();
        }
        else if (index == 3) 
        {
            PlayBGM(bgm2);
        }
        else if (index == 6) 
        {
            StopBGM();
        }

        float clipDuration = (float)videoPlayer.clip.length;
        float elapsedTime = 0f;

        while (elapsedTime < clipDuration && !isSkipping)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        NextClip();
    }

    void NextClip()
    {
        if (isSkipping)
        {
            isSkipping = false;
        }

        currentClipIndex++;

        if (currentClipIndex < videoClips.Length)
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
        if (currentClipIndex < videoClips.Length - 1)
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
        bgmSource.Stop();
    }

    IEnumerator LoadNextScene()
    {
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
