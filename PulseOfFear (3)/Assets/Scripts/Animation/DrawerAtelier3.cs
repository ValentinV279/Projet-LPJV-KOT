using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawerAtelier3 : MonoBehaviour
{
    private Animator animator;
    private bool isOpen = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void ToggleAtelier3()
    {
        if (isOpen)
        {
            animator.Play("Tiroir_atelier_open3");
        }
        else
        {
            animator.Play("Tiroir_atelier_close3");
        }
        isOpen = !isOpen;
    }
}