using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButton : MonoBehaviour
{
    // Local Variables
    MenuButtonController menuButtonController;
    [HideInInspector] public Animator animator;
    [HideInInspector] public AnimatorFunctions animatorFunctions;
    [HideInInspector] public int thisIndex;

    #region Setup
    private void Awake()
    {
        animator = GetComponent<Animator>();
        animatorFunctions = GetComponent<AnimatorFunctions>();
    }
    private void Start()
    {
        menuButtonController = MenuButtonController.instance;
    }
    #endregion Setup

    private void Update()
    {
        if (menuButtonController.index == thisIndex)
        {
            animator.SetBool("Selected", true);

            if (Input.GetAxis("Submit") == 1)
            {
                animator.SetBool("Pressed", true);
            }
            else if (animator.GetBool("Pressed"))
            {
                animator.SetBool("Pressed", false);
            }
        }
        else
        {
            animator.SetBool("Selected", false);
        }
    }
}
