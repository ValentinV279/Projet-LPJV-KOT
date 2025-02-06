using UnityEngine;
using TMPro;

public class DoorController : MonoBehaviour
{
    public PressurePlate plate1;    // Référence à la première plaque de pression
    public PressurePlate plate2;    // Référence à la deuxième plaque de pression
    public float rotationSpeed = 90f; // Vitesse de rotation de la porte en degrés par seconde

    public TextMeshProUGUI doorTaskText; // Texte barré lorsque la porte s'ouvre

    private Quaternion initialRotation;  // Rotation initiale de la porte
    private Quaternion openRotation;     // Rotation où la porte doit être quand elle est ouverte
    private bool doorIsOpen = false;     // Indique si la porte est ouverte

    private void Start()
    {
        // Sauvegarder la rotation initiale de la porte
        initialRotation = transform.rotation;
        // Définir la rotation ouverte (rotation de 90 degrés sur l'axe Z)
        openRotation = initialRotation * Quaternion.Euler(0, 0, 90); // 90° pour une porte standard
    }

    private void Update()
    {
        // Si les deux plaques sont activées et que la porte n'est pas encore ouverte
        if (plate1.isActivated && plate2.isActivated && !doorIsOpen)
        {
            OpenDoor();
        }
    }

    void OpenDoor()
    {
        // Rotation progressive de la porte vers la position ouverte
        if (Quaternion.Angle(transform.rotation, openRotation) > 0.01f)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, openRotation, rotationSpeed * Time.deltaTime);
            Debug.Log("La porte s'ouvre.");
        }
        else
        {
            // Fixer la rotation exactement à la position ouverte
            transform.rotation = openRotation;
            doorIsOpen = true; // Marquer que la porte est maintenant ouverte
            Debug.Log("La porte est maintenant ouverte et restera ouverte.");
            TaskManager.Instance.CompleteTask();
            if (doorTaskText != null)
            {
                doorTaskText.fontStyle = FontStyles.Strikethrough;
            }
        }
    }
}
