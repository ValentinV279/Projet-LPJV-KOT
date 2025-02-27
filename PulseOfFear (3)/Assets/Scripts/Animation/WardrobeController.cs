using UnityEngine;

public class WardrobeController : MonoBehaviour
{
    private Animator animator;
    private bool isOpen = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void ToggleWardrobe()
    {
        if (isOpen)
        {
            animator.Play("Amoire_chambre_droite_open");
        }
        else
        {
            animator.Play("Amoire_chambre_droite_close");
        }
        isOpen = !isOpen;
    }
}
