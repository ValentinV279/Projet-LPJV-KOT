using UnityEngine;
using TMPro;

public class DoorController : MonoBehaviour
{
    public PressurePlate plate1;
    public PressurePlate plate2;
    public float rotationSpeed = 90f;

    public TextMeshProUGUI doorTaskText;

    private Quaternion initialRotation;
    private Quaternion openRotation;
    private bool doorIsOpen = false;
    private bool platesActivated = false;  // Nouveau booléen pour suivre l'activation des plaques

    private void Start()
    {
        initialRotation = transform.rotation;
        openRotation = initialRotation * Quaternion.Euler(0, 0, 90);
    }

    private void Update()
    {
        // Vérifie si les plaques sont activées et si l'ouverture n'a pas encore été déclenchée
        if (plate1.isActivated && plate2.isActivated && !platesActivated)
        {
            platesActivated = true; // Marque que les plaques ont été activées
            OpenDoor();
        }

        // Si la porte doit s'ouvrir, applique l'animation
        if (platesActivated && !doorIsOpen)
        {
            AnimateDoorOpening();
        }
    }

    void OpenDoor()
    {
        Debug.Log("Les plaques sont activées, ouverture de la porte...");
    }

    void AnimateDoorOpening()
    {
        // Rotation progressive de la porte vers la position ouverte
        if (Quaternion.Angle(transform.rotation, openRotation) > 0.01f)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, openRotation, rotationSpeed * Time.deltaTime);
        }
        else
        {
            transform.rotation = openRotation;
            doorIsOpen = true; // Marquer la porte comme ouverte
            Debug.Log("La porte est maintenant ouverte.");
            TaskManager.Instance.CompleteTask();

            if (doorTaskText != null)
            {
                doorTaskText.fontStyle = FontStyles.Strikethrough;
            }
        }
    }
}
