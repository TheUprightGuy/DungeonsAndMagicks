using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimatorFunctions : MonoBehaviour
{
    public Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void SetWeaponOff()
    {
        animator.SetBool("WeaponOut", false);
    }

    public void SetWeaponOn()
    {
        animator.SetBool("WeaponOut", true);
    }
}
