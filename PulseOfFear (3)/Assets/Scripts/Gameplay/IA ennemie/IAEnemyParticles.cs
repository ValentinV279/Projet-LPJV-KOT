using System.Collections;
using UnityEngine;

public class IAEnemyParticles : MonoBehaviour
{
    [SerializeField] private ParticleSystem smokeParticles;
    [SerializeField] private float particleDuration = 1f; // Durée d'apparition des particules par étape

    /// <summary>
    /// Joue l'effet de particules de fumée.
    /// </summary>
    public void TriggerSmokeEffect()
    {
        if (smokeParticles != null && !smokeParticles.isPlaying)
        {
            smokeParticles.Play(); // Joue les particules
            StartCoroutine(StopSmokeAfterDelay());
        }
    }

    /// <summary>
    /// Arrête l'effet de particules après la durée définie.
    /// </summary>
    private IEnumerator StopSmokeAfterDelay()
    {
        yield return new WaitForSeconds(particleDuration);

        if (smokeParticles != null)
        {
            smokeParticles.Stop(); // Arrête les particules
        }
    }
}
