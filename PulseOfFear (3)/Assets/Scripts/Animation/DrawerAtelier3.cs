using System.Collections;
using UnityEngine;

public class DrawerAtelier3 : MonoBehaviour
{
    private Animator animator;
    private bool isOpen = false;
    private bool isAnimating = false; // Empêche une nouvelle animation pendant que l'actuelle est en cours

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void ToggleAtelier3()
    {
        if (!isAnimating) // Vérifie si une animation est déjà en cours
        {
            StartCoroutine(PlayAnimation());
        }
    }

    private IEnumerator PlayAnimation()
    {
        isAnimating = true; // Empêche d'autres animations de démarrer

        if (isOpen)
        {
            animator.Play("Tiroir_atelier_close3");
        }
        else
        {
            animator.Play("Tiroir_atelier_open3");
        }

        isOpen = !isOpen; // Bascule l'état d'ouverture

        // Attendre la fin de l'animation avant de permettre une nouvelle interaction
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        isAnimating = false; // Permet à nouveau les animations
    }
}