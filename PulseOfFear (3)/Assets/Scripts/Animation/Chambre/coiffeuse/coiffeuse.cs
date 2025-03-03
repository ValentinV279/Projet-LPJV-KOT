using System.Collections;
using UnityEngine;

public class coiffeuse : MonoBehaviour
{
    private Animator animator;
    private bool isOpen = false;
    private bool isAnimating = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Togglecoiffeuse()
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
            animator.Play("coiffeuseclose");
        }
        else
        {
            animator.Play("coiffeuseopen");
        }

        isOpen = !isOpen;

        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        isAnimating = false;
    }
}
