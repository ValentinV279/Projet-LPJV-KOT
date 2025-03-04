using System.Collections;
using UnityEngine;

public class coiffeuse1 : MonoBehaviour
{
    private Animator animator;
    private bool isOpen = false;
    private bool isAnimating = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Togglecoiffeuse1()
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
            animator.Play("coiffeuseclose2");
        }
        else
        {
            animator.Play("coiffeuseopen2");
        }

        isOpen = !isOpen;

        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        isAnimating = false;
    }
}
