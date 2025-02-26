using UnityEngine;

public class DrawerController : MonoBehaviour
{
    private Animator animator;
    private bool isOpen = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void ToggleDrawer()
    {
        if (isOpen)
        {
            animator.Play("open_Tiroir");
        }
        else
        {
            animator.Play("close_Tiroir");
        }
        isOpen = !isOpen;
    }
}
