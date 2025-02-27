using UnityEngine;

public class DoorLeftController : MonoBehaviour
{
    private Animator animator;
    private bool isOpen = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void ToggleDoor()
    {
        if (isOpen)
        {
            animator.Play("Armoir_chambre_gauche_open");
        }
        else
        {
            animator.Play("Armoir_chambre_gauche_close");
        }
        isOpen = !isOpen;
    }
}