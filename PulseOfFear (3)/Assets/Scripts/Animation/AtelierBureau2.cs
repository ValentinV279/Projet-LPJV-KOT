using System.Collections;
using UnityEngine;

public class AtelierBureau2 : MonoBehaviour
{
    private Animator animator;
    private bool isOpen = false;
    private bool isAnimating = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void ToggleBureau2()
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
            animator.Play("Tiroir_bureau_close2");
        }
        else
        {
            animator.Play("Tiroir_bureau_open2");
        }

        isOpen = !isOpen;

        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        isAnimating = false;
    }
}
