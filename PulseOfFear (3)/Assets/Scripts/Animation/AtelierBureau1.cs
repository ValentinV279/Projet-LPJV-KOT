using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtelierBureau1 : MonoBehaviour
{
    private Animator animator;
    private bool isOpen = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void ToggleBureau1()
    {
        if (isOpen)
        {
            animator.Play("Tiroir_bureau_open1");
        }
        else
        {
            animator.Play("Tiroir_bureau_close1");
        }
        isOpen = !isOpen;
    }
}