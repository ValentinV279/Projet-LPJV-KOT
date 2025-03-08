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
            
        }
        else
        {
            animator.Play("Amoire_chambre_droite_open");
            FMODUnity.RuntimeManager.PlayOneShot("event:/Envt/Enviro_action_armoire_closed");
        }

        isOpen = !isOpen;

        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        isAnimating = false;
    }
}
