using System.Collections;
using UnityEngine;

public class Commodehall : MonoBehaviour
{
    private Animator animator;
    private bool isOpen = false;
    private bool isAnimating = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void TogglecommodeHall1()
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
            animator.Play("commodehallopen");
        }
        else
        {
            animator.Play("commodehallclose");
        }

        isOpen = !isOpen;

        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        isAnimating = false;
    }
}
