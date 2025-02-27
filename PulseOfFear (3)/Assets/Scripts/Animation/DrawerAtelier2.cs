using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawerAtelier2 : MonoBehaviour
{
    private Animator animator;
    private bool isOpen = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void ToggleAtelier2()
    {
        if (isOpen)
        {
            animator.Play("Tiroir_atelier_open2");
        }
        else
        {
            animator.Play("Tiroir_atelier_close2");
        }
        isOpen = !isOpen;
    }
}