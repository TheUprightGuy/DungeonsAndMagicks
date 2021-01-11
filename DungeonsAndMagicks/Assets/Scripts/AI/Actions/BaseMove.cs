using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaseMove : AIAction
{
    public override void Move(Vector3 _target, ref NavMeshAgent _baseAgent)
    {
        //base.Move(_target, ref _baseAgent);
        _baseAgent.SetDestination(_target);
    }
}
