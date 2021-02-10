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

    public void Attack()
    {
        animator.SetBool("Attack", true);
        animator.SetBool("InRange", true);
    }
    public void AttackFinish()
    {
        animator.SetBool("Attack", false);
        animator.SetBool("InRange", false);
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
