using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorOverrider : MonoBehaviour
{

    #region VARIABLES

    //Variables
    private Animator animator;

    [SerializeField] private AnimatorOverrideController[] overrideControllers;
    [SerializeField] private AnimatorOverrider overrider;

    #endregion
    // Start is called before the first frame update
    void Start()
    {
        #region ASSIGN COMPONENTS

        animator = transform.GetComponent<Animator>();

        #endregion
    }

    public void SetAnimations(AnimatorOverrideController overrideController)
    {
        animator.runtimeAnimatorController = overrideController;
    }

    public void SetAnimationToValueInList(int animationCount)
    {
        overrider.SetAnimations(overrideControllers[animationCount]);
    }
}
