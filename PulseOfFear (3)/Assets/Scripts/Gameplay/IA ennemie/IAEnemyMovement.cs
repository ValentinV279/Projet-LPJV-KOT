using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class IAEnemyMovement : MonoBehaviour
{
    [SerializeField] Transform target; // Cible actuelle (le joueur)
    [SerializeField] Transform spawnPoint;
    [SerializeField] Animator animator;

    private NavMeshAgent agent;
    private Rigidbody rb;
    private IAEnemyAudio enemyAudio;

    private bool isDeactivated = false;
    private bool hasCapturedPlayer = false;
    private bool returningToSpawn = false; // Indique si l'IA retourne à son spawn
    private bool isAccelerating = false; // Pour éviter de relancer l'accélération pendant l'effet
    private bool isWaitingAtStart = true; // Indique si l'IA est en phase d'attente initiale

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        enemyAudio = GetComponent<IAEnemyAudio>();

        if (rb != null) rb.isKinematic = true;

        GameObject spawnObject = GameObject.Find("IASpawnPoint");
        if (spawnObject != null)
        {
            spawnPoint = spawnObject.transform;
        }
        else
        {
            Debug.LogError("IASpawnPoint non trouvé dans la scène !");
        }

        if (agent == null)
        {
            Debug.LogError("NavMeshAgent non trouvé sur l'IA !");
            return;
        }
        if (animator == null)
        {
            Debug.LogError("Animator non assigné sur l'IA !");
            return;
        }

        // Désactiver immédiatement tout mouvement
        agent.enabled = false;
        ForceIdleState(true);
        isDeactivated = true;

        // Lancer le timer d'attente initiale
        StartCoroutine(InitialIdlePhase());
    }

    void Update()
    {
        if (isWaitingAtStart) return;

        if (!isDeactivated && !hasCapturedPlayer && agent != null)
        {
            if (agent.enabled)
            {
                if (returningToSpawn)
                {
                    agent.SetDestination(spawnPoint.position);

                    if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
                    {
                        returningToSpawn = false;
                        ForceIdleState(true);
                    }
                }
                else
                {
                    agent.SetDestination(target.position);

                    if (agent.pathStatus == NavMeshPathStatus.PathPartial || agent.pathStatus == NavMeshPathStatus.PathInvalid)
                    {
                        MoveToSpawn();
                    }
                    else
                    {
                        ForceIdleState(false);

                        // ✅ Ajout : Forcer l'animation "Run" si la vitesse est > 2.5f
                        if (agent.speed > 2.5f)
                        {
                            animator.SetBool("Run", true);
                            animator.SetBool("Walk", false);
                            Debug.Log("L'IA passe en mode course !");
                        }
                        else
                        {
                            animator.SetBool("Run", false);
                            animator.SetBool("Walk", true);
                        }
                    }
                }
            }
        }
    }

    private IEnumerator InitialIdlePhase()
    {
        yield return new WaitForSeconds(30f);

        isWaitingAtStart = false;
        isDeactivated = false;

        agent.enabled = true;
        agent.isStopped = false;

        ForceIdleState(false);

        StartCoroutine(AccelerationRoutine());
    }

    private void MoveToSpawn()
    {
        if (spawnPoint != null)
        {
            returningToSpawn = true;
            agent.SetDestination(spawnPoint.position);
            agent.isStopped = false;

            ForceIdleState(false);
            animator.SetBool("Walk", true);
            animator.SetBool("Run", false);
        }
        else
        {
            Debug.LogError("Le point de spawn n'est pas assigné !");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isDeactivated || hasCapturedPlayer) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            hasCapturedPlayer = true;

            var ui = GetComponent<IAEnemyUI>();
            if (ui != null)
            {
                ui.CapturePlayerEffects();
            }

            if (enemyAudio != null)
            {
                enemyAudio.PlayCaptureSound();
            }

            StopMovement();
        }
    }

    public void SetDeactivatedState(bool state)
    {
        if (isWaitingAtStart) return;

        isDeactivated = state;
        if (state)
        {
            StopMovement();
            animator.SetBool("Desactivate", true);
        }
        else
        {
            agent.enabled = true;
            agent.isStopped = false;
            animator.SetBool("Desactivate", false);
        }
    }

    public bool IsDeactivated()
    {
        return isDeactivated;
    }

    private void StopMovement()
    {
        if (agent != null && agent.enabled)
        {
            agent.isStopped = true;
        }

        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    private void ForceIdleState(bool state)
    {
        if (animator != null)
        {
            animator.SetBool("Idle", state);
            animator.SetBool("Walk", !state);
            animator.SetBool("Run", false);
        }

        if (agent != null && agent.enabled)
        {
            agent.isStopped = state;
        }
    }

    private IEnumerator AccelerationRoutine()
    {
        yield return new WaitForSeconds(30f);

        while (true)
        {
            float delay = Random.Range(10f, 25f);
            yield return new WaitForSeconds(delay);

            if (!isDeactivated && !hasCapturedPlayer && !isAccelerating)
            {
                StartCoroutine(Accelerate());
            }
        }
    }

    public IEnumerator Accelerate()
    {
        isAccelerating = true;

        if (enemyAudio != null)
        {
            enemyAudio.PlayAccelerationSound();
        }

        if (agent != null)
        {
            agent.speed = 5f;
        }

        if (animator != null)
        {
            animator.SetBool("Run", true);
            animator.SetBool("Walk", false);
            Debug.Log("L'IA est en accélération et passe en mode course !");
        }

        float duration = Random.Range(5f, 10f);
        yield return new WaitForSeconds(duration);

        if (agent != null) agent.speed = 2f;
        if (animator != null)
        {
            animator.SetBool("Run", false);
            animator.SetBool("Walk", true);
            Debug.Log("L'IA repasse en mode marche.");
        }

        isAccelerating = false;
    }
}
