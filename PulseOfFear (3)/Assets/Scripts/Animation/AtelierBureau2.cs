using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtelierBureau2 : MonoBehaviour
{
    private Animator animator;
    private bool isOpen = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void ToggleBureau2()
    {
        if (isOpen)
        {
            animator.Play("Tiroir_bureau_open2");
        }
        else
        {
            animator.Play("Tiroir_bureau_close2");
        }
        isOpen = !isOpen;
    }
}