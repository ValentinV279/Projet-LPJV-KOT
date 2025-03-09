using System.Collections;
using UnityEngine;

public class DoorHall1 : MonoBehaviour
{
    private Animator animator;
    private bool isOpen = false; // Indique si la porte est ouverte ou ferm�e
    private bool isAnimating = false; // V�rifie si une animation est en cours

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void ToggleDoorHall()
    {
        // Ne d�clenche l'animation que si aucune animation n'est en cours
        if (!isAnimating)
        {
            StartCoroutine(PlayAnimation());
        }
    }

    private IEnumerator PlayAnimation()
    {
        isAnimating = true; // Emp�che d'autres animations de d�marrer

        // Si la porte est ouverte, on joue l'animation de fermeture, sinon l'animation d'ouverture
        if (isOpen)
        {
            animator.Play("doorhallclose");
            FMODUnity.RuntimeManager.PlayOneShot("event:/Envt/Enviro_action_interract_porte_close", GetComponent<Transform>().position);
        }
        else
        {
            animator.Play("doorhallopen");
            FMODUnity.RuntimeManager.PlayOneShot("event:/Envt/Enviro_action_interract_porte_open", GetComponent<Transform>().position);
        }

        // Attend la fin de l'animation avant de changer l'�tat de la porte
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        // Inverse l'�tat de la porte seulement apr�s l'animation
        isOpen = !isOpen;

        isAnimating = false; // Permet de rejouer l'animation si n�cessaire
    }
}
