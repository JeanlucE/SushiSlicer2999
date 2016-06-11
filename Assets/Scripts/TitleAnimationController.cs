using UnityEngine;
using System.Collections;

public class TitleAnimationController : MonoBehaviour 
{
    public Animator animator;

    public void SetSlice(bool slice)
    {
        //animator.SetBool("slice", slice);
        animator.Play("Slice");
    }
}
