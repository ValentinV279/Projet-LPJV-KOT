using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockTrigger : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();

        if (animator == null)
        {
            Debug.LogError("Aucun Animator trouvé sur " + gameObject.name);
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1)) // 1 = Clic droit
        {
            if (animator != null)
            {
                animator.SetTrigger("Knock");
            }
        }
    }
}
