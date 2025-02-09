using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickeringLight : MonoBehaviour
{
    private Light pointLight; // Référence à la lumière
    public float minIntensity = 0.2f; // Intensité minimale de la lumière
    public float maxIntensity = 1.2f; // Intensité maximale de la lumière
    public float flickerSpeedMin = 0.05f; // Vitesse minimale entre les changements
    public float flickerSpeedMax = 0.3f; // Vitesse maximale entre les changements

    void Start()
    {
        pointLight = GetComponent<Light>(); // Récupère la lumière attachée
        if (pointLight == null)
        {
            Debug.LogError("Aucune Light n'est attachée à cet objet !");
            return;
        }

        StartCoroutine(FlickerLight()); // Démarre l'effet de grésillement
    }

    IEnumerator FlickerLight()
    {
        while (true)
        {
            float randomIntensity = Random.Range(minIntensity, maxIntensity); // Génère une intensité aléatoire
            pointLight.intensity = randomIntensity; // Applique l'intensité
            float randomWaitTime = Random.Range(flickerSpeedMin, flickerSpeedMax); // Définit un délai aléatoire
            yield return new WaitForSeconds(randomWaitTime); // Attend avant le prochain grésillement
        }
    }
}
