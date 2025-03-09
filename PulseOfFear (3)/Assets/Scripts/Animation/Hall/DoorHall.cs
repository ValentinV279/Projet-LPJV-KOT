using System.Collections;
using UnityEngine;

public class DoorHall1 : MonoBehaviour
{
    private Animator animator;
    private bool isOpen = false; // Indique si la porte est ouverte ou fermée
    private bool isAnimating = false; // Vérifie si une animation est en cours

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void ToggleDoorHall()
    {
        // Ne déclenche l'animation que si aucune animation n'est en cours
        if (!isAnimating)
        {
            StartCoroutine(PlayAnimation());
        }
    }

    private IEnumerator PlayAnimation()
    {
        isAnimating = true; // Empêche d'autres animations de démarrer

        // Si la porte est ouverte, on joue l'animation de fermeture, sinon l'animation d'ouverture
        if (isOpen)
        {
            animator.Play("doorhallclose");
        }
        else
        {
            animator.Play("doorhallopen");
        }

        // Attend la fin de l'animation avant de changer l'état de la porte
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        // Inverse l'état de la porte seulement après l'animation
        isOpen = !isOpen;

        isAnimating = false; // Permet de rejouer l'animation si nécessaire
    }
}
