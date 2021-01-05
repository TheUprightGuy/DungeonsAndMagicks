using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButton : MonoBehaviour
{
    // Local Variables
    [HideInInspector] public MenuButtonController menuButtonController;
    [HideInInspector] public Animator animator;
    [HideInInspector] public AnimatorFunctions animatorFunctions;
    [HideInInspector] public int thisIndex;

    public MenuOptions menuOptions;

    #region Setup
    private void Awake()
    {
        animator = GetComponent<Animator>();
        animatorFunctions = GetComponent<AnimatorFunctions>();
    }
    #endregion Setup

    private void Update()
    {
        if (menuButtonController.index == thisIndex)
        {
            animator.SetBool("Selected", true);

            if (Input.GetKeyDown(KeyCode.Return))
            {
                animator.SetBool("Pressed", true);
                CanvasController.instance.ToggleMenuOption(menuOptions);
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
