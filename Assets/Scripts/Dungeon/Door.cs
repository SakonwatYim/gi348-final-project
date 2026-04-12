using UnityEngine;

public class Door : MonoBehaviour
{
   private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void ShowCloseAnimtion()
    {
        animator.SetTrigger("CloseDoor");
    }

    public void ShowOpenAnimtion()
    {
        animator.SetTrigger("OpenDoor");
    }
}
