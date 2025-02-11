using UnityEngine;
using UnityStandardAssets.ImageEffects;

public class IAProximityNoise : MonoBehaviour
{
    [SerializeField] private GameObject enemy; // L'IA ennemie
    [SerializeField] private float maxDistance = 5f; // Distance maximale où l'effet commence
    [SerializeField] private float maxIntensity = 20f; // Intensité maximale du Noise and Grain
    private NoiseAndGrain noiseAndGrain;

    void Start()
    {
        // Vérifie si NoiseAndGrain existe et est correctement initialisé
        noiseAndGrain = GetComponent<NoiseAndGrain>();
        if (noiseAndGrain == null)
        {
            Debug.LogWarning("Aucun composant NoiseAndGrain trouvé sur la caméra !");
            enabled = false; // Désactive le script si le composant est manquant
        }
    }


    void Update()
    {
        if (enemy == null || noiseAndGrain == null) return;

        // Calcule la distance entre le joueur et l'IA ennemie
        float distance = Vector3.Distance(transform.position, enemy.transform.position);

        // Ajuste l'intensité en fonction de la distance
        if (distance <= maxDistance)
        {
            // Interpolation linéaire entre 0 et maxIntensity en fonction de la distance
            float intensity = Mathf.Lerp(maxIntensity, 0f, distance / maxDistance);
            noiseAndGrain.intensityMultiplier = intensity;
        }
        else
        {
            // Réinitialise à 0 si le joueur est à plus de maxDistance
            noiseAndGrain.intensityMultiplier = 0f;
        }
    }
}
