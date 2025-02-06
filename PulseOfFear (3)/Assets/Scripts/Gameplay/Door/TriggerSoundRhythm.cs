using UnityEngine;

public class TriggerSoundRhythm : MonoBehaviour
{
    public AudioClip son; // Son joué lorsque le joueur entre dans le trigger
    public AudioClip sonEchec; // Son joué si le joueur échoue
    public AudioClip sonOuverture; // Son joué quand la porte s'ouvre
    public GameObject porte; // Référence à la porte
    public float bpm = 60f; // BPM ajusté à 60 pour un rythme plus facile
    public int nombreDeCoups = 3; // Nombre de clics nécessaires pour ouvrir la porte
    public float toleranceTempo = 0.75f; // Tolérance de timing encore plus large (en secondes)
    public float rotationSpeed = 90f; // Vitesse de rotation de la porte en degrés par seconde

    private AudioSource audioSource;
    private bool dansTrigger = false;
    private bool sequenceEnCours = false;
    private bool porteOuverte = false; // Indique si la porte est ouverte ou non
    private int coupsEffectues = 0;
    private float dureeEntreCoups; // Calculée en fonction du BPM
    private float dernierCoupTemps = 0f; // Le temps du dernier coup
    private float tempsDeDemarrage = 0f; // Le temps auquel la séquence a commencé
    private float[] tempsDesCoups; // Tableau pour enregistrer les temps des 3 clics
    private Quaternion initialRotation; // Rotation initiale de la porte
    private Quaternion openRotation; // Rotation cible de la porte (ouverte)
    private bool estEnTrainDouvrir = false; // Indique si la porte est en train de s'ouvrir

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource component not found!");
        }

        dureeEntreCoups = 60f / bpm; // Calcul de l'intervalle entre chaque coup
        tempsDesCoups = new float[nombreDeCoups]; // Initialisation du tableau de temps des coups
        Debug.Log("Duree entre les coups : " + dureeEntreCoups);

        // Initialiser les rotations de la porte
        initialRotation = porte.transform.rotation;
        openRotation = initialRotation * Quaternion.Euler(0, 90, 0); // Rotation de 90° pour ouvrir la porte
        Debug.Log("Rotation initiale de la porte: " + initialRotation.eulerAngles);
        Debug.Log("Rotation cible de la porte: " + openRotation.eulerAngles);
    }

    void Update()
    {
        if (estEnTrainDouvrir)
        {
            // Ouvrir la porte progressivement
            if (Quaternion.Angle(porte.transform.rotation, openRotation) > 0.01f)
            {
                porte.transform.rotation = Quaternion.RotateTowards(porte.transform.rotation, openRotation, rotationSpeed * Time.deltaTime);
                Debug.Log("Rotation actuelle: " + porte.transform.rotation.eulerAngles);
            }
            else
            {
                // La porte est complètement ouverte
                porte.transform.rotation = openRotation;
                estEnTrainDouvrir = false;
                Debug.Log("La porte est maintenant complètement ouverte.");
            }
        }

        if (porteOuverte) return; // Si la porte est ouverte, arrêter toute nouvelle interaction

        if (dansTrigger && Input.GetMouseButtonDown(1) && sequenceEnCours)
        {
            float tempsActuel = Time.time;

            // Enregistrer le temps du coup
            if (coupsEffectues < nombreDeCoups)
            {
                tempsDesCoups[coupsEffectues] = tempsActuel;
                coupsEffectues++;
                Debug.Log("Coup " + coupsEffectues + " enregistré à " + tempsActuel);
            }

            // Si le nombre de coups est atteint, vérifier si la séquence est correcte
            if (coupsEffectues == nombreDeCoups)
            {
                VerifierSequence();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !porteOuverte)
        {
            dansTrigger = true;
            // Démarrer la séquence lorsque le joueur entre dans le trigger
            Debug.Log("Le joueur est entré dans le trigger.");
            audioSource.PlayOneShot(son); // Jouer le son pour signaler le début
            coupsEffectues = 0; // Réinitialiser les coups
            tempsDeDemarrage = Time.time; // Enregistrer le moment où la séquence commence
            sequenceEnCours = true; // Démarrer la séquence
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            dansTrigger = false;
            // Arrêter le son lorsque le joueur quitte le trigger
            audioSource.Stop();
            // Réinitialiser la séquence si le joueur quitte la zone
            Debug.Log("Le joueur a quitté le trigger.");
            ResetSequence();
        }
    }

    void VerifierSequence()
    {
        bool sequenceValide = true;

        // Vérification des intervalles entre les clics
        for (int i = 1; i < tempsDesCoups.Length; i++)
        {
            float delaiEntreCoups = tempsDesCoups[i] - tempsDesCoups[i - 1];
            Debug.Log("Délai entre le coup " + i + " et le coup " + (i - 1) + ": " + delaiEntreCoups);

            if (Mathf.Abs(delaiEntreCoups - dureeEntreCoups) > toleranceTempo)
            {
                sequenceValide = false;
                Debug.Log("Erreur! Délai trop grand entre deux clics. Tolérance: " + toleranceTempo + " secondes.");
                break;
            }
        }

        if (sequenceValide)
        {
            Debug.Log("Succès! Rythme correct.");
            OuvrirPorte();
        }
        else
        {
            Debug.Log("Échec! Rythme incorrect.");
            audioSource.PlayOneShot(sonEchec);
            ResetSequence(); // Réinitialiser la séquence
        }
    }

    void OuvrirPorte()
    {
        // Logique pour commencer à ouvrir la porte progressivement
        Debug.Log("Porte en train de s'ouvrir.");
        audioSource.PlayOneShot(sonOuverture);
        estEnTrainDouvrir = true; // Commencer l'ouverture progressive
        porteOuverte = true; // Marquer que la porte doit s'ouvrir
        ResetSequence(); // Réinitialiser la séquence après avoir déclenché l'ouverture
    }

    void ResetSequence()
    {
        // Réinitialise l'état de la séquence pour recommencer
        sequenceEnCours = false;
        coupsEffectues = 0;
        tempsDesCoups = new float[nombreDeCoups]; // Réinitialiser le tableau de temps des coups
        Debug.Log("Séquence réinitialisée.");
    }
}
