using UnityEngine;

public class ChestLid : MonoBehaviour
{
    public Transform pivotPoint;
    public float rotationSpeed = 50f;
    public float finalDecelerationFactor = 0.1f;
    public float rotationDuration = 3f;
    public float openAngle = 70f;

    public AudioClip openSound;
    public AudioClip closeSound;

    private bool isRotating = false;
    public AudioSource audioSource;
    private float currentAngle = 0f;

    void Start()
    {
        audioSource = GetComponent<AudioSource>() ?? gameObject.AddComponent<AudioSource>();
    }

    public void ToggleLid()
    {
        if (isRotating) return;

        float targetAngle = Mathf.Approximately(currentAngle, 0f) ? openAngle : 0f;
        openAngle *= -1; // Inverse l'angle d'ouverture
        PlaySound(targetAngle == openAngle ? openSound : closeSound);
        StartCoroutine(RotateLid(targetAngle));
    }

    private System.Collections.IEnumerator RotateLid(float targetAngle)
    {
        isRotating = true;

        float elapsedTime = 0f;
        float initialSpeed = rotationSpeed;

        while (elapsedTime < rotationDuration)
        {
            float step = initialSpeed * Time.deltaTime;
            float remainingAngle = Mathf.Abs(targetAngle - currentAngle);

            if (remainingAngle < 20f)
            {
                step *= Mathf.Lerp(1f, finalDecelerationFactor, 1f - remainingAngle / 20f);
            }

            float deltaAngle = step * Mathf.Sign(targetAngle - currentAngle);
            currentAngle += deltaAngle;
            transform.RotateAround(pivotPoint.position, Vector3.right, deltaAngle);

            if (Mathf.Abs(currentAngle - targetAngle) < 0.1f) break;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.RotateAround(pivotPoint.position, Vector3.right, targetAngle - currentAngle);
        currentAngle = targetAngle;
        isRotating = false;
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null) audioSource.PlayOneShot(clip);
    }
}
