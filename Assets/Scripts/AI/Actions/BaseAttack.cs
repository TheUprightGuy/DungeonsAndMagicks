using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaseAttack : AIAction
{
    public override void Attack(Vector3 _target, ref NavMeshAgent _baseAgent)
    {
        base.Attack(_target, ref _baseAgent);
    }
}
