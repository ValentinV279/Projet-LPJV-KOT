using System.Collections;
using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    private Rigidbody rb;
    public bool playerCanMove = true;
    public float walkSpeed = 5f;
    public Camera playerCamera;
    public float mouseSensitivity = 2f;
    public float controllerSensitivity = 2f;
    public float maxLookAngle = 50f;
    public Transform joint;
    public float bobSpeed = 10f;
    public Vector3 bobAmount = new Vector3(.15f, .05f, 0f);

    public AudioSource footstepAudioSource;
    public AudioClip[] footstepSounds;
    public float stepInterval = 0.5f;

    private Vector3 jointOriginalPos;
    private float timer = 0;
    private float yaw = 0.0f;
    private float pitch = 0.0f;
    private bool isGrounded = false;
    private float stepTimer = 0f;

    public Animator animator; // Public pour assigner dans l'Inspector

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        jointOriginalPos = joint.localPosition;
        Cursor.lockState = CursorLockMode.Locked;

        if (animator == null)
            Debug.LogError("Aucun Animator trouvé sur cet objet.");

        if (footstepAudioSource == null)
            footstepAudioSource = gameObject.AddComponent<AudioSource>();

        if (footstepSounds == null || footstepSounds.Length == 0)
            Debug.LogError("Aucun son de pas assigné dans l'inspecteur.");
    }

    void Update()
    {
        CameraRotation();
        CheckGround();

        float moveX = Input.GetAxis("JoystickHorizontal") + Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("JoystickVertical") + Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveX, 0, moveZ);
        bool isWalking = movement.magnitude > 0.1f && isGrounded; // Ajout d'un seuil pour éviter des valeurs proches de 0
        bool isClapping = Input.GetMouseButtonDown(0);

        if (animator != null)
        {
            animator.SetFloat("Speed", isWalking ? 1f : 0f);
            
            if (isClapping)
            {
                animator.SetTrigger("Clap");
                StartCoroutine(ResetClapTrigger());
            }
        }

        if (isWalking)
        {
            stepTimer += Time.deltaTime;
            if (stepTimer >= stepInterval)
            {
                PlayFootstepSound();
                FMODUnity.RuntimeManager.PlayOneShot("event:/Units/Alyssa/Charater_footsteps");
                stepTimer = 0f;
            }
            HeadBob();
        }
        else
        {
            stepTimer = 0f;
            StopFootstepSound();
        }
    }

    void FixedUpdate()
    {
        if (playerCanMove) MovePlayer();
    }

    private void MovePlayer()
    {
        float moveX = Input.GetAxis("JoystickHorizontal") + Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("JoystickVertical") + Input.GetAxis("Vertical");

        Vector3 targetVelocity = new Vector3(moveX, 0, moveZ);
        if (targetVelocity.magnitude > 1)
        {
            targetVelocity.Normalize();
        }

        targetVelocity = transform.TransformDirection(targetVelocity) * walkSpeed;

        Vector3 velocity = rb.velocity;
        Vector3 velocityChange = (targetVelocity - velocity);
        velocityChange.x = Mathf.Clamp(velocityChange.x, -10f, 10f);
        velocityChange.z = Mathf.Clamp(velocityChange.z, -10f, 10f);
        velocityChange.y = 0;
        rb.AddForce(velocityChange, ForceMode.VelocityChange);
    }

    private void CameraRotation()
    {
        float lookXMouse = Input.GetAxis("Mouse X") * mouseSensitivity;
        float lookYMouse = Input.GetAxis("Mouse Y") * mouseSensitivity;
        float lookXController = Input.GetAxis("RightStickHorizontal") * controllerSensitivity;
        float lookYController = Input.GetAxis("RightStickVertical") * controllerSensitivity;

        float horizontalRotation = lookXMouse + lookXController;
        float verticalRotation = lookYMouse + lookYController;

        yaw += horizontalRotation;
        pitch -= verticalRotation;
        pitch = Mathf.Clamp(pitch, -maxLookAngle, maxLookAngle);

        transform.localEulerAngles = new Vector3(0, yaw, 0);
        playerCamera.transform.localEulerAngles = new Vector3(pitch, 0, 0);
    }

    private void CheckGround()
    {
        Vector3 origin = new Vector3(transform.position.x, transform.position.y - (transform.localScale.y * .5f), transform.position.z);
        isGrounded = Physics.Raycast(origin, Vector3.down, 0.75f);
    }

    private void HeadBob()
    {
        timer += Time.deltaTime * bobSpeed;
        joint.localPosition = jointOriginalPos + new Vector3(Mathf.Sin(timer) * bobAmount.x, Mathf.Sin(timer) * bobAmount.y, Mathf.Sin(timer) * bobAmount.z);
    }

    private void PlayFootstepSound()
    {
        if (footstepSounds != null && footstepSounds.Length > 0 && !footstepAudioSource.isPlaying)
        {
            AudioClip randomFootstep = footstepSounds[Random.Range(0, footstepSounds.Length)];
            footstepAudioSource.clip = randomFootstep;
            footstepAudioSource.loop = false;
            footstepAudioSource.Play();
        }
    }

    private void StopFootstepSound()
    {
        if (footstepAudioSource.isPlaying)
        {
            footstepAudioSource.Stop();
        }
    }

    IEnumerator ResetClapTrigger()
    {
        yield return new WaitForSeconds(0.1f); // Attendre un peu avant de reset
        animator.ResetTrigger("Clap");
    }
}
