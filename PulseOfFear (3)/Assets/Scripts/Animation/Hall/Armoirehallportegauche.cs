using System.Collections;
using UnityEngine;

public class Armoirehallportegauche : MonoBehaviour
{
    private Animator animator;
    private bool isOpen = false;
    private bool isAnimating = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void ToggleArmoirhallLeft()
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
            animator.Play("armoirHallLeftclose");
            FMODUnity.RuntimeManager.PlayOneShot("event:/Envt/Enviro_action_interract_porte_close", GetComponent<Transform>().position);
        }
        else
        {
            animator.Play("armoirHallLeftopen");
            FMODUnity.RuntimeManager.PlayOneShot("event:/Envt/Enviro_action_interract_porte_open", GetComponent<Transform>().position);
        }

        isOpen = !isOpen;

        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        isAnimating = false;
    }
}
