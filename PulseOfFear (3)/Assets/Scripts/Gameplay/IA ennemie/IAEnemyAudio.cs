using UnityEngine;

public class IAEnemyAudio : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip captureSound; // Son joué lors de la capture
    [SerializeField] private AudioClip stepSound; // Son joué à chaque étape de désactivation
    [SerializeField] private AudioClip accelerationSound; // Son joué lors de l'accélération

    /// <summary>
    /// Joue le son de capture.
    /// </summary>
    public void PlayCaptureSound()
    {
        if (audioSource != null && captureSound != null)
        {
            float originalVolume = audioSource.volume;
            audioSource.volume *= 0.2f; // Réduction temporaire du volume
            audioSource.PlayOneShot(captureSound);
            audioSource.volume = originalVolume; // Restauration du volume
        }
    }

    /// <summary>
    /// Joue le son correspondant à une étape de désactivation.
    /// </summary>
    public void PlayStepSound()
    {
        if (audioSource != null && stepSound != null)
        {
            audioSource.PlayOneShot(stepSound);
        }
    }

    void FmodEnemyStepShoe()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Units/Automate/Automate_footstepshoe", GetComponent<Transform>().position);
    }

    void FmodEnemyStepMetal()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Units/Automate/Automate_footstepmetal", GetComponent<Transform>().position);
    }
    void FmodEnemyStepMetalRun()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Units/Automate/Automate_footstepmetalrun", GetComponent<Transform>().position);
    }

    void FmodEnemyStepShoeRun()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Units/Automate/Automate_footstepshoerun", GetComponent<Transform>().position);
    }

    /// <summary>
    /// Joue le son d'accélération.
    /// </summary>
    public void PlayAccelerationSound()
    {
        if (audioSource != null && accelerationSound != null)
        {
            audioSource.PlayOneShot(accelerationSound);
        }
    }
}
