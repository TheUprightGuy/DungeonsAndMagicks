using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAnimatorFunctions : MonoBehaviour
{
    public Animator animator;
    public NavMeshAgent agent;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponentInParent<NavMeshAgent>();
    }

    public void Update()
    {
        if (agent)
        {
            animator.SetFloat("MovementSpeed", agent.velocity.magnitude);
        }
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
