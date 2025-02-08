using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSoundPlayer : MonoBehaviour
{
    public AudioSource audioSource; // L'AudioSource à rattacher
    public AudioClip sound1; // Premier son
    public AudioClip sound2; // Deuxième son

    void Start()
    {
        StartCoroutine(PlayRandomSound());
    }

    IEnumerator PlayRandomSound()
    {
        while (true)
        {
            float waitTime = Random.Range(5f, 10f); // Attente entre 5 et 10 secondes
            yield return new WaitForSeconds(waitTime);
            
            if (audioSource != null && (sound1 != null || sound2 != null))
            {
                AudioClip clipToPlay = Random.Range(0, 2) == 0 ? sound1 : sound2;
                audioSource.clip = clipToPlay;
                audioSource.Play();
            }
        }
    }
}
