using UnityEngine;

public class ChestController : MonoBehaviour
{
    private Animator animator;
    private bool isOpen = false; // Gérer l'état du coffre (ouvert/fermé)

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void ToggleChest()
    {
        if (isOpen)
        {
            Debug.Log("Fermeture du coffre.");
            animator.Play("Coffre_open");
        }
        else
        {
            Debug.Log("Ouverture du coffre.");
            animator.Play("Coffre_close");
        }
        isOpen = !isOpen;
    }

}
