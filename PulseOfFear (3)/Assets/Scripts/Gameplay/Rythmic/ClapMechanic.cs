using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ClapMechanic : MonoBehaviour
{
    public IAEnemyMovement enemyMovement; // Référence au script de mouvement
    public IAEnemyAudio enemyAudio; // Référence au script audio de l'ennemi
    public IAEnemyParticles enemyParticles; // Référence au script de particules
    public AudioClip successClapSound; // Son pour clap réussi
    public AudioClip failClapSound; // Son pour clap échoué
    public AudioClip lookAtEnemySound; // Son lorsque le joueur doit regarder l'ennemi
    public AudioSource audioSource; // Source audio pour jouer les sons

    public Scrollbar clapScrollbar; // Scrollbar UI pour le feedback visuel
    public Image clapHandleImage; // Image du handle de la ScrollBar
    public TMP_Text step1Text; // Texte UI pour la première étape
    public TMP_Text step2Text; // Texte UI pour la deuxième étape
    public TMP_Text step3Text; // Texte UI pour la troisième étape
    public EnemyVisibilityCheck visibilityCheck; // Référence au script de visibilité de l'ennemi

    private int clapStep = 0;
    private float[] clapPattern = new float[3]; // Durée des étapes de désactivation
    private bool isClapActive = false; // Indique si le joueur peut interagir avec le clap
    private bool isFirstClap = true; // Indique si c'est le premier clap

    private Color defaultHandleColor = Color.blue; // Couleur par défaut
    private Color failHandleColor = Color.red; // Couleur en cas d'échec

    private float scrollbarMaxTime; // Temps lorsque la ScrollBar atteint 1
    private float timeSinceLastClap = 0f; // Temps écoulé depuis le dernier clap
    private float clapTimeout = 3f; // Temps maximum avant de considérer un échec (3 secondes)

    void Start()
    {
        RandomizeClapPattern();
        UpdateClapBookUI();
        ResetScrollbar();
    }

    void Update()
    {
        if (enemyMovement != null && enemyMovement.IsDeactivated()) return; // Ne rien faire si l'ennemi est désactivé

        if (Input.GetMouseButtonDown(0)) // Clic gauche pour activer le clap
        {
            if (isFirstClap)
            {
                HandleFirstClap();
            }
            else if (isClapActive)
            {
                HandleClap();
            }
        }

        if (isClapActive)
        {
            UpdateScrollbar();

            // Incrémenter le temps écoulé
            timeSinceLastClap += Time.deltaTime;

            // Vérifie si le joueur n'a pas clappé depuis 3 secondes
            if (timeSinceLastClap >= clapTimeout)
            {
                ResetClapSequence(); // Réinitialise la séquence et masque l'UI
            }
        }
    }

    void RandomizeClapPattern()
    {
        for (int i = 0; i < clapPattern.Length; i++)
        {
            clapPattern[i] = Random.Range(0.5f, 3f); // Randomise entre 0.5 et 3 secondes
        }
    }

    void UpdateClapBookUI()
    {
        if (step1Text != null) step1Text.text = clapPattern[0].ToString("F1") + "s";
        if (step2Text != null) step2Text.text = clapPattern[1].ToString("F1") + "s";
        if (step3Text != null) step3Text.text = clapPattern[2].ToString("F1") + "s";
    }

    void ResetScrollbar()
    {
        if (clapScrollbar != null)
        {
            clapScrollbar.value = 0;
            clapScrollbar.gameObject.SetActive(false);
        }

        if (clapHandleImage != null)
        {
            clapHandleImage.color = defaultHandleColor; // Remet la couleur par défaut
        }

        scrollbarMaxTime = 0; // Réinitialise le temps de dépassement
    }

    void UpdateScrollbar()
    {
        if (clapScrollbar != null && clapScrollbar.value < 1)
        {
            clapScrollbar.value += Time.deltaTime / clapPattern[clapStep];
        }
        else if (clapScrollbar.value >= 1 && scrollbarMaxTime == 0)
        {
            scrollbarMaxTime = Time.time; // Enregistre le moment où la valeur atteint 1
        }
    }

    void HandleFirstClap()
    {
        if (visibilityCheck != null && !visibilityCheck.IsEnemyVisible())
        {
            // Joue un son pour indiquer que l'ennemi doit être visible
            audioSource.PlayOneShot(lookAtEnemySound);
            return; // Empêche le clap
        }

        isFirstClap = false;
        isClapActive = true;

        // Joue le son de clap réussi pour signaler l'activation
        audioSource.PlayOneShot(successClapSound);

        // Active l'UI de la ScrollBar
        if (clapScrollbar != null)
        {
            clapScrollbar.gameObject.SetActive(true);
        }

        // Réinitialise le compteur de temps depuis le dernier clap
        timeSinceLastClap = 0f;
    }

    void HandleClap()
    {
        if (clapScrollbar == null) return;

        // Vérifie si le joueur regarde l'ennemi pour chaque clap
        if (visibilityCheck != null && !visibilityCheck.IsEnemyVisible())
        {
            // Joue un son pour indiquer que l'ennemi doit être visible
            audioSource.PlayOneShot(lookAtEnemySound);
            return; // Empêche le clap
        }

        if (Mathf.Abs(clapScrollbar.value - 1f) <= 0.1f && (scrollbarMaxTime == 0 || Time.time - scrollbarMaxTime <= 0.2f)) // Clap réussi
        {
            audioSource.PlayOneShot(successClapSound);
            clapScrollbar.value = 0;
            clapStep++;

            if (clapStep >= clapPattern.Length) // Si tous les claps sont réussis
            {
                StartCoroutine(DeactivateEnemy());
                ResetClapSequence();
            }

            // Réinitialise le compteur de temps depuis le dernier clap
            timeSinceLastClap = 0f;
        }
        else // Clap échoué
        {
            StartCoroutine(HandleClapFail());
        }
    }

    IEnumerator HandleClapFail()
    {
        // Change la couleur du handle
        if (clapHandleImage != null)
        {
            clapHandleImage.color = failHandleColor;
        }

        audioSource.PlayOneShot(failClapSound);

        // Active l'accélération de l'ennemi
        if (enemyMovement != null)
        {
            StartCoroutine(enemyMovement.Accelerate());
        }

        // Attend 0.5 seconde avant de cacher la barre
        yield return new WaitForSeconds(0.5f);
        ResetClapSequence();
    }

    IEnumerator DeactivateEnemy()
    {
        if (enemyMovement != null)
        {
            enemyMovement.SetDeactivatedState(true);
        }

        if (enemyParticles != null)
        {
            enemyParticles.TriggerSmokeEffect();
        }

        yield return new WaitForSeconds(20f);

        if (enemyMovement != null)
        {
            enemyMovement.SetDeactivatedState(false);
        }
    }

    void ResetClapSequence()
    {
        clapStep = 0;
        isFirstClap = true;
        ResetScrollbar();
        isClapActive = false;
    }
}
