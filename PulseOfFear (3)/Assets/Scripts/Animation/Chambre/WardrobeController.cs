using System.Collections;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class WardrobeController : MonoBehaviour
{
    private Animator animator;
    private bool isOpen = false;
    private bool isAnimating = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void ToggleWardrobe()
    {
        if (!isAnimating)
        {
            StartCoroutine(PlayAnimation());
        }
    }

    private IEnumerator PlayAnimation()
    {
        isAnimating = true;

        if (isOpen)
        {
            animator.Play("Amoire_chambre_droite_close");
            FMODUnity.RuntimeManager.PlayOneShot("event:/Envt/Enviro_action_interract_porte_open", GetComponent<Transform>().position);
            
        }
        else
        {
            animator.Play("Amoire_chambre_droite_open");
            FMODUnity.RuntimeManager.PlayOneShot("event:/Envt/Enviro_action_interract_porte_close", GetComponent<Transform>().position);
        }

        isOpen = !isOpen;

        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        isAnimating = false;
    }
}
