using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    // Volume sliders variables
    [Range(0, 1)] public float masterVolume = 0.5f;
    [Range(0, 1)] public float sfxVolume = 0.5f;
    [Range(0, 1)] public float musicVolume = 0.5f;

    // UI Sliders
    public Slider masterVolumeSlider;
    public Slider sfxVolumeSlider;
    public Slider musicVolumeSlider;

    // Lists to manage audio sources
    public List<AudioSource> sfxAudioSources = new List<AudioSource>();
    public List<AudioSource> musicAudioSources = new List<AudioSource>();

    // Dictionary for sound effects
    public Dictionary<string, AudioClip> sfxClips = new Dictionary<string, AudioClip>();

    void Start()
    {
        LoadVolumeSettings();

        // Assign slider values to initial volumes
        if (masterVolumeSlider != null)
        {
            masterVolumeSlider.value = masterVolume;
            masterVolumeSlider.onValueChanged.AddListener(SetMasterVolume);
        }

        if (sfxVolumeSlider != null)
        {
            sfxVolumeSlider.value = sfxVolume;
            sfxVolumeSlider.onValueChanged.AddListener(SetSFXVolume);
        }

        if (musicVolumeSlider != null)
        {
            musicVolumeSlider.value = musicVolume;
            musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
        }

        UpdateAudioSourcesVolume();
    }

    void Update()
    {
        UpdateAudioSourcesVolume();
    }

    private void UpdateAudioSourcesVolume()
    {
        foreach (AudioSource sfxSource in sfxAudioSources)
        {
            if (sfxSource != null)
            {
                sfxSource.volume = sfxVolume * masterVolume;
            }
        }

        foreach (AudioSource musicSource in musicAudioSources)
        {
            if (musicSource != null)
            {
                musicSource.volume = musicVolume * masterVolume;
            }
        }
    }

    public void SetMasterVolume(float value)
    {
        masterVolume = value;
        PlayerPrefs.SetFloat("MasterVolume", masterVolume);
        PlayerPrefs.Save();
        UpdateAudioSourcesVolume();
    }

    public void SetSFXVolume(float value)
    {
        sfxVolume = value;
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
        PlayerPrefs.Save();
        UpdateAudioSourcesVolume();
    }

    public void SetMusicVolume(float value)
    {
        musicVolume = value;
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
        PlayerPrefs.Save();
        UpdateAudioSourcesVolume();
    }

    private void LoadVolumeSettings()
    {
        masterVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);
        sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);
        musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
    }

    public void AddSFXAudioSource(AudioSource audioSource)
    {
        if (!sfxAudioSources.Contains(audioSource))
        {
            sfxAudioSources.Add(audioSource);
        }
    }

    public void AddMusicAudioSource(AudioSource audioSource)
    {
        if (!musicAudioSources.Contains(audioSource))
        {
            musicAudioSources.Add(audioSource);
        }
    }

    public void RemoveSFXAudioSource(AudioSource audioSource)
    {
        if (sfxAudioSources.Contains(audioSource))
        {
            sfxAudioSources.Remove(audioSource);
        }
    }

    public void RemoveMusicAudioSource(AudioSource audioSource)
    {
        if (musicAudioSources.Contains(audioSource))
        {
            musicAudioSources.Remove(audioSource);
        }
    }

    public AudioClip GetKnockSound(string tag)
    {
        if (sfxClips.ContainsKey(tag))
        {
            return sfxClips[tag];
        }
        return null;
    }

    public void RegisterSFX(string tag, AudioClip clip)
    {
        if (!sfxClips.ContainsKey(tag))
        {
            sfxClips.Add(tag, clip);
        }
    }
}
