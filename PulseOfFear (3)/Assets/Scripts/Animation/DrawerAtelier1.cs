using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawerAtelier1: MonoBehaviour
{
    private Animator animator;
    private bool isOpen = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void ToggleAtelier1()
    {
        if (isOpen)
        {
            animator.Play("Tiroir_atelier_open1");
        }
        else
        {
            animator.Play("Tiroir_atelier_close1");
        }
        isOpen = !isOpen;
    }
}