using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtelierBureau3 : MonoBehaviour
{
    private Animator animator;
    private bool isOpen = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void ToggleBureau3()
    {
        if (isOpen)
        {
            animator.Play("Tiroir_bureau_open3");
        }
        else
        {
            animator.Play("Tiroir_bureau_close3");
        }
        isOpen = !isOpen;
    }
}