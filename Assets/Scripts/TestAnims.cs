using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAnims : MonoBehaviour
{
    public Animator animator;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public bool attack;
    public bool run;


    private void Update()
    {
        animator.SetBool("Attack", attack);
        animator.SetBool("InRange", attack);

        animator.SetFloat("MovementSpeed", run ? 1.0f : 0.0f);
    }
}
